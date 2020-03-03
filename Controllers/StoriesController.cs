using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Services;
using Blog_Rest_Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Rest_Api.Controllers{

    [Route("/v1/[controller]")]
    [Consumes("application/json", new string[]{"application/xml"})]
    [Produces("application/json", new string[]{"application/xml"})]    
    public class StoriesController : BaseController
    {
        private readonly IStoriesService storiesService;

        public StoriesController(IStoriesService storiesService)
        {
            this.storiesService = storiesService;
        }


        [HttpPost("story")]
        [Authorize]
        public async Task<IActionResult> CreateStory([FromBody]RequestStoryDTO storyDTO){
            string userId=HttpContext.User.Claims.FirstOrDefault(c=>c.Type== System.Security.Claims.ClaimTypes.Sid).Value;
            DBStatus status=await storiesService.CreateStoryAsync(storyDTO,userId);
            ResponseStatusDTO responseStatusDTO= new ResponseStatusDTO((int)status,status.ToString());
            if(status==DBStatus.Failed)
                return BadRequest(new BadResponseDTO{Status=(int)status,Errors=new {Message =new List<string>{status.ToString()}}});  
            else 
                return CreatedAtAction(nameof(GetStory),new {storyId=storyDTO.StoryId},null);
        }

        [HttpGet("stories")]
        [HttpGet("stories/{skip}")]
        [HttpGet("stories/{skip}/{top}")]
        public async Task<IActionResult> GetStories(int skip=0,int top=50){
            List<ResponseStoryDTO>  stories=await storiesService.GetStoryAsync(skip,top);
            return Ok(stories);
        }

        [HttpGet("story/{storyId}")]
        public async Task<IActionResult> GetStory([Required]Guid storyId){
            ResponseStoryDTO story=await storiesService.GetStoryAsync(storyId);
            if(story==null)
                return NotFound();
            return Ok(story); 
        }

        [HttpPut("story")]
        [Authorize]
        public async Task<IActionResult> UpdateStory([FromBody]RequestStoryDTO storyDTO){
            Console.WriteLine("akash in");
            string userId=HttpContext.User.Claims.FirstOrDefault(c=>c.Type== System.Security.Claims.ClaimTypes.Sid).Value;
            DBStatus status=await storiesService.ReplaceStoryAsync(storyDTO,userId);
            ResponseStatusDTO responseStatusDTO= new ResponseStatusDTO((int)status,status.ToString());
            if(status==DBStatus.NotFound)
                return NotFound();
            else if(status==DBStatus.Forbidden)
                return Forbid();   
            else if(status==DBStatus.NotModified)
                return BadRequest(new BadResponseDTO{Status=(int)status,Errors=new {Message =new List<string>{status.ToString()}}}); 
            else 
                return Ok(responseStatusDTO);
        }

        [HttpDelete("story/{storyId}")]
        [Authorize]
        public async Task<IActionResult> RemoveStory([Required]Guid storyId){
            string userId=HttpContext.User.Claims.FirstOrDefault(c=>c.Type== System.Security.Claims.ClaimTypes.Sid).Value;
            DBStatus status= await storiesService.RemoveStoryAsync(storyId,userId);
            ResponseStatusDTO responseStatusDTO= new ResponseStatusDTO((int)status,status.ToString());
            if(status==DBStatus.NotFound)
                return NotFound();
            else if(status==DBStatus.Forbidden)
                return Forbid(); 
            else if(status==DBStatus.NotDeleted)
                return BadRequest(new BadResponseDTO{Status=(int)status,Errors=new {Message =new List<string>{status.ToString()}}}); 
            else 
                return Ok(responseStatusDTO); 
        }
        
    }
}