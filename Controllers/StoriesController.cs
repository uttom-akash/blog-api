using System;
using System.ComponentModel.DataAnnotations;
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
    
    [Authorize(AuthenticationSchemes=JwtBearerDefaults.AuthenticationScheme)]
    public class StoriesController : BaseController
    {
        private readonly IStoriesService storiesService;

        public StoriesController(IStoriesService storiesService)
        {
            this.storiesService = storiesService;
        }

        [HttpPost("story")]
        [ValidateModel]
        // [Consumes("application/json", new string[]{"application/xml"})]
        public async Task<IActionResult> CreateStory([FromBody]StoryDTO storyDTO){
            DBStatus status=await storiesService.CreateStoryAsync(storyDTO);
            ResponseStatusDTO responseStatusDTO= new ResponseStatusDTO((int)status,status.ToString());
            if(status==DBStatus.Failed)
                return BadRequest(responseStatusDTO);
            else 
                return Ok(responseStatusDTO);
        }

        [HttpGet("stories")]
        [AllowAnonymous]
        [ValidateModel]
        // [Consumes("application/json", new string[]{"application/xml"})]
        public async Task<IActionResult> GetStories(){
            return Ok(await storiesService.GetStoryAsync());
        }

        [HttpGet("story/{storyId}")]
        [AllowAnonymous]
        [ValidateModel]
        // [Consumes("application/json", new string[]{"application/xml"})]
        public async Task<IActionResult> GetStory([Required]Guid storyId){
            StoryDTO story=await storiesService.GetStoryAsync(storyId);
            if(story==null)
                return NoContent();
            return Ok(story); 
        }

        [HttpPut("story")]
        [ValidateModel]
        public async Task<IActionResult> UpdateStory([FromBody]StoryDTO storyDTO){
            DBStatus status=await storiesService.ReplaceStoryAsync(storyDTO);
            ResponseStatusDTO responseStatusDTO= new ResponseStatusDTO((int)status,status.ToString());
            if(status==DBStatus.NotFound)
                return NotFound(responseStatusDTO);
            else if(status==DBStatus.NotModified)
                return BadRequest(responseStatusDTO);
            else 
                return Ok(responseStatusDTO);
        }

        [HttpDelete("story/{storyId}")]
        [ValidateModel]
        // [Consumes("application/json", new string[]{"application/xml"})]
        public async Task<IActionResult> RemoveStory([Required]Guid storyId){
            DBStatus status= await storiesService.RemoveStoryAsync(storyId);
            ResponseStatusDTO responseStatusDTO= new ResponseStatusDTO((int)status,status.ToString());
            if(status==DBStatus.NotFound)
                return NotFound(responseStatusDTO);
            else if(status==DBStatus.NotDeleted)
                return BadRequest(responseStatusDTO);
            else 
                return Ok(responseStatusDTO); 
        }
        
    }
}