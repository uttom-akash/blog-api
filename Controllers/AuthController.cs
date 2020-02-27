using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog_Rest_Api.Custom_Attribute;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Services;
using Blog_Rest_Api.Utils;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Rest_Api.Controllers{

    [Route("/v1/[controller]")]
    public class AuthController : BaseController
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userRegistrationDTO){
            DBStatus status=await authService.RegisterAsync(userRegistrationDTO);
            ResponseStatusDTO responseStatusDTO= new ResponseStatusDTO((int)status,status.ToString());
            if(status==DBStatus.Failed)
                return BadRequest(responseStatusDTO);
            return  Ok(responseStatusDTO);
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] UserCredentialsDTO userCredentialsDTO){
            UserInfoDTO userInfoDTO =await authService.LoginAsync(userCredentialsDTO);
            if(userInfoDTO==null)
                return NoContent();
            return Ok(userInfoDTO);
        }

    }
}