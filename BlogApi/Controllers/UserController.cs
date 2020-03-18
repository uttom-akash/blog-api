using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog_Rest_Api.Custom_Attribute;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Services;
using Blog_Rest_Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Rest_Api.Controllers
{

    [Route("/v1/[controller]")]
    public class UserController : BaseController
    {
        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        [HttpGet("users")]
        [HttpGet("users/{skip}")]
        [HttpGet("users/{skip}/{top}")]
        public IActionResult GetUsers(int skip = 0, int top = 50)
        {
            IEnumerable<UserInfoDTO> users = userService.GetUsers(skip, top);
            return Ok(users);
        }

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUser([Required]string userId)
        {
            UserInfoDTO user = await userService.GetUserAsync(userId);
            if (user == null)
                return NotFound();
            return Ok(user);
        }

        [HttpPut("user")]
        [Authorize]
        public async Task<IActionResult> UpdateUserPassword([FromBody]UpdateUserPasswordDTO updateUserPasswordDTO)
        {
            string userId = HttpContext.User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Sid).Value;
            updateUserPasswordDTO.UserId = userId;
            DBStatus status = await userService.UpdateUserPasswordAsync(updateUserPasswordDTO);
            ResponseStatusDTO responseStatusDTO = new ResponseStatusDTO((int)status, status.ToString());
            if (status == DBStatus.NotFound)
                return NotFound();
            else if (status == DBStatus.Forbidden)
                return Forbid();
            else if (status == DBStatus.NotModified)
                return BadRequest(new BadResponseDTO { Status = (int)status, Errors = new Errors { Message = new List<string> { status.ToString() } } });
            else
                return Ok(responseStatusDTO);
        }
    }
}