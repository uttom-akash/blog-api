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

namespace Blog_Rest_Api.Controllers{

    [Route("/v1/[controller]")]
    [Consumes("application/json", new string[]{"application/xml"})]
    [Produces("application/json", new string[]{"application/xml"})] 
    public class AuthController : BaseController
    {
        private readonly IAuthService authService;

        public AuthController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationDTO userRegistrationDTO){
            DBStatus status=await authService.RegisterAsync(userRegistrationDTO);
            ResponseStatusDTO responseStatusDTO= new ResponseStatusDTO((int)status,status.ToString());
            if(status==DBStatus.Failed)
                return BadRequest(new BadResponseDTO{Status=(int)status,Errors=new Errors{Message =new List<string>{status.ToString()}}});
            else if(status==DBStatus.Taken)
                 return BadRequest(new BadResponseDTO{Status=(int)status,Errors=new Errors{Message =new List<string>{"User Id already taken"}}});
            return  Ok(responseStatusDTO);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] UserCredentialsDTO userCredentialsDTO){
            UserInfoDTO userInfoDTO =await authService.LoginAsync(userCredentialsDTO);
            if(userInfoDTO==null)
                 return BadRequest(new BadResponseDTO{Status=(int)400,Errors=new Errors{Message =new List<string>{"Credentials are wrong"}}});
            return Ok(userInfoDTO);
        }

    }
}