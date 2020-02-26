using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Blog_Rest_Api.Custom_Attribute;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Blog_Rest_Api.Controllers{

    [Route("/v1/[controller]/[action]")]
    public class StoriesController : BaseController
    {
        private readonly IStoriesService storiesService;

        public StoriesController(IStoriesService storiesService)
        {
            this.storiesService = storiesService;
        }

        [HttpPost]
        [ValidateModel]
        // [Consumes("application/json", new string[]{"application/xml"})]
        public async Task<IActionResult> Story([FromBody]StoryDTO storyDTO){
            return Ok(await storiesService.CreateStoryAsync(storyDTO));
        }

        [HttpGet]
        [ValidateModel]
        // [Consumes("application/json", new string[]{"application/xml"})]
        public async Task<IActionResult> Stories(){
            return Ok(await storiesService.GetStoryAsync());
        }

         [HttpGet("{storyId}")]
         [ValidateModel]
        // [Consumes("application/json", new string[]{"application/xml"})]
        public async Task<IActionResult> Story([Required]Guid storyId){
            return Ok(await storiesService.GetStoryAsync(storyId)); 
        }
    }
}