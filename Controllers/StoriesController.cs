using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog_Rest_Api.Custom_Attribute;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Services;
using Blog_Rest_Api.Utils;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Rest_Api.Controllers{

    [Route("/v1/[controller]")]
    public class StoriesController : BaseController
    {
        private readonly IStoriesService storiesService;

        public StoriesController(IStoriesService storiesService)
        {
            this.storiesService = storiesService;
        }

        [HttpPost("story")]
        [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModel]
        // [Consumes("application/json", new string[]{"application/xml"})]
        public async Task<IActionResult> CreateStory([FromBody]StoryDTO storyDTO){
            string userId=HttpContext.User.Claims.FirstOrDefault(c=>c.Type== System.Security.Claims.ClaimTypes.Sid).Value;
            DBStatus status=await storiesService.CreateStoryAsync(storyDTO,userId);
            ResponseStatusDTO responseStatusDTO= new ResponseStatusDTO((int)status,status.ToString());
            if(status==DBStatus.Failed)
                return BadRequest(responseStatusDTO);
            else if(status==DBStatus.Forbidden)
                return Forbid();    
            else 
                return Ok(responseStatusDTO);
        }

        [HttpGet("stories")]
        [ValidateModel]
        // [Consumes("application/json", new string[]{"application/xml"})]
        public async Task<IActionResult> GetStories(){
            return Ok(await storiesService.GetStoryAsync());
        }

        [HttpGet("story/{storyId}")]
        [ValidateModel]
        // [Consumes("application/json", new string[]{"application/xml"})]
        public async Task<IActionResult> GetStory([Required]Guid storyId){
            ResponseStoryDTO story=await storiesService.GetStoryAsync(storyId);
            if(story==null)
                return NotFound();
            return Ok(story); 
        }

        [HttpPut("story")]
        [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModel]
        public async Task<IActionResult> UpdateStory([FromBody]StoryDTO storyDTO){
            string userId=HttpContext.User.Claims.FirstOrDefault(c=>c.Type== System.Security.Claims.ClaimTypes.Sid).Value;
            DBStatus status=await storiesService.ReplaceStoryAsync(storyDTO,userId);
            ResponseStatusDTO responseStatusDTO= new ResponseStatusDTO((int)status,status.ToString());
            if(status==DBStatus.NotFound)
                return NotFound(responseStatusDTO);
            else if(status==DBStatus.Forbidden)
                return Forbid();   
            else if(status==DBStatus.NotModified)
                return BadRequest(responseStatusDTO);
            else 
                return Ok(responseStatusDTO);
        }

        [HttpDelete("story/{storyId}")]
        [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
        [ValidateModel]
        // [Consumes("application/json", new string[]{"application/xml"})]
        public async Task<IActionResult> RemoveStory([Required]Guid storyId){
            string userId=HttpContext.User.Claims.FirstOrDefault(c=>c.Type== System.Security.Claims.ClaimTypes.Sid).Value;
            DBStatus status= await storiesService.RemoveStoryAsync(storyId,userId);
            ResponseStatusDTO responseStatusDTO= new ResponseStatusDTO((int)status,status.ToString());
            if(status==DBStatus.NotFound)
                return NotFound(responseStatusDTO);
            else if(status==DBStatus.Forbidden)
                return Forbid(); 
            else if(status==DBStatus.NotDeleted)
                return BadRequest(responseStatusDTO);
            else 
                return Ok(responseStatusDTO); 
        }
        
    }
}