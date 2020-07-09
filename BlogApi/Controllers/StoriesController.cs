using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Blog_Rest_Api.Crypto;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Services;
using Blog_Rest_Api.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;

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
                return BadRequest(new BadResponseDTO{Status=(int)status,Errors=new Errors{Message =new List<string>{status.ToString()}}});
            else 
                return CreatedAtAction(nameof(GetStory),new {storyId=storyDTO.StoryId},null);
        }

        [HttpGet("stories")]
        [HttpGet("stories/{skip}")]
        [HttpGet("stories/{skip}/{top}")]
        public async Task<IActionResult> GetStories([FromQuery]string query="",int skip=0,int top=50){
            StoriesWithCountDTO  stories=await storiesService.GetStoriesAsync(query,skip,top);
            return Ok(stories);
        }

        [HttpGet("user-stories/{userId}")]
        [HttpGet("user-stories/{userId}/{skip}")]
        [HttpGet("user-stories/{userId}/{skip}/{top}")]
        public async Task<IActionResult> GetUserStories([Required]string userId="",[FromQuery]string query="",int skip=0,int top=50){
            StoriesWithCountDTO  stories=await storiesService.GeUserStoriesAsync(userId,query,skip,top);
            return Ok(stories);
        }


        [HttpGet("story/{storyId}")]
        public async Task<IActionResult> GetStory([Required]Guid storyId){
            ResponseStoryDTO story=await storiesService.GetStoryAsync(storyId);
            if(story==null)
                return NotFound();
            
            string etag=ConverterSuit.ByteArrayToHex(HashSuit.ComputeMD5(Encoding.UTF8.GetBytes(story.ToString()))); 
            string ETag=HttpContext.Request.Headers["If-None-Match"];

            if(etag==ETag)
                return StatusCode(StatusCodes.Status304NotModified);

            HttpContext.Response.Headers.Add("ETag", new[] { etag });    
            return Ok(story); 
        }

        [HttpPut("story")]
        [Authorize]
        public async Task<IActionResult> UpdateStory([FromBody]RequestStoryDTO storyDTO){
            string userId=HttpContext.User.Claims.FirstOrDefault(c=>c.Type== System.Security.Claims.ClaimTypes.Sid).Value;
            
            DBStatus status=await storiesService.ReplaceStoryAsync(storyDTO,userId);
            ResponseStatusDTO responseStatusDTO= new ResponseStatusDTO((int)status,status.ToString());
            if(status==DBStatus.NotFound)
                return NotFound();
            else if(status==DBStatus.Forbidden)
                return Forbid();   
            else if(status==DBStatus.NotModified)
                return BadRequest(new BadResponseDTO{Status=(int)status,Errors=new Errors{Message =new List<string>{status.ToString()}}});
            else if(status==DBStatus.PreconditionFailed)
                return StatusCode(StatusCodes.Status412PreconditionFailed);
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
                return StatusCode(StatusCodes.Status403Forbidden); 
            else if(status==DBStatus.NotDeleted)
               return BadRequest(new BadResponseDTO{Status=(int)status,Errors=new Errors{Message =new List<string>{status.ToString()}}});
            else 
                return Ok(responseStatusDTO); 
        }
        
    }
}