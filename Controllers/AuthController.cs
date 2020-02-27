using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog_Rest_Api.Custom_Attribute;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Services;
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
            return Ok(await authService.RegisterAsync(userRegistrationDTO));
        }

        [HttpPost("[action]")]
        [ValidateModel]
        public async Task<IActionResult> Login([FromBody] UserCredentialsDTO userCredentialsDTO){
            return Ok(await authService.LoginAsync(userCredentialsDTO));
        }

    }
}