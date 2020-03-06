using Blog_Rest_Api.Controllers;
using Blog_Rest_Api.Services;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Utils;
using Xunit;
using Moq;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;
using Xunit.Abstractions;
using System.Collections;

namespace BlogRestAPiTest.ControllerTesting
{
    public class StoriesControllerTest
    {
        private readonly ITestOutputHelper testOutputHelper;
        private readonly Mock<IStoriesService> storiesService;
        private StoriesController storiesController;
        private readonly ControllerContext httpContext;

        public StoriesControllerTest(ITestOutputHelper testOutputHelper)
        {
            Claim claim = new Claim(ClaimTypes.Sid, "akash");
            List<Claim> claims = new List<Claim> { claim };
            this.testOutputHelper = testOutputHelper;

            httpContext = CreateHttpContext(claims);
            storiesService = new Mock<IStoriesService>();
            storiesController = new StoriesController(storiesService.Object);


        }

        public ControllerContext CreateHttpContext(List<Claim> claims)
        {
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims);
            return new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = new ClaimsPrincipal(claimsIdentity)
                }
            };
        } 







        [Theory]
        [ClassData(typeof(RequestStoriesTestData))]
        public async Task TestCreateStoryBadRequest(RequestStoryDTO story)
        {
            
            // Arrange
            RequestStoryDTO storyDTO = new RequestStoryDTO { StoryId = Guid.NewGuid(), Title = "", Body = "", PublishedDate = DateTime.UtcNow };
            DBStatus dbStatus = DBStatus.Failed;


            storiesService.Setup(x => x.CreateStoryAsync(storyDTO, "akash")).ReturnsAsync(dbStatus); 
            storiesController.ControllerContext = httpContext;

            //Act
            var actionResult =await storiesController.CreateStory(storyDTO) as BadRequestObjectResult;

            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal(400, actionResult.StatusCode);
            BadResponseDTO badResponseDTO = actionResult.Value as BadResponseDTO;
            Assert.Equal(1,badResponseDTO.Status);
        }



        [Fact]
        public async Task TestCreateStoryCreated()
        {

            // Arrange
            Guid expectedStoryId = Guid.NewGuid();
            RequestStoryDTO storyDTO = new RequestStoryDTO { StoryId = expectedStoryId, Title = "", Body = "", PublishedDate = DateTime.UtcNow };
            DBStatus dbStatus = DBStatus.Added;

            storiesService.Setup(x => x.CreateStoryAsync(storyDTO, "akash")).ReturnsAsync(dbStatus);
            storiesController.ControllerContext = httpContext;

            
            //Act
            var actionResult = await storiesController.CreateStory(storyDTO) as CreatedAtActionResult;
            
            //Assert
            Assert.NotNull(actionResult);
            Assert.Equal(201, actionResult.StatusCode);
            Assert.Equal(expectedStoryId.ToString(), actionResult.RouteValues.GetValueOrDefault("storyId").ToString());
        }



        [Fact]
        public async Task TestGetStories()
        {
            //Arrange
            string expectedTitle = "LoremIpsum";
            int expectedLength = 1;
            List<ResponseStoryDTO> storyDTOs = new List<ResponseStoryDTO> { new ResponseStoryDTO {Title= expectedTitle } };

            storiesService.Setup(x => x.GetStoriesAsync(It.IsAny<string>(),It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(storyDTOs);
            
            //Act
            var result=await storiesController.GetStories() as OkObjectResult;
            var actualStories = result.Value as List<ResponseStoryDTO>;
           
            //Assert
            Assert.NotNull(actualStories);
            Assert.Equal(200,result.StatusCode);
            Assert.Equal(expectedLength, actualStories.Count);
            Assert.Equal(storyDTOs,actualStories);

        }

        [Fact]
        public async Task TestUpdateStoryNotFound()
        {
            //Arrange
            string expectedTitle = "LoremIpsum";
            string userId = "akash";
            DBStatus status = DBStatus.NotFound;
            RequestStoryDTO storyDTO = new RequestStoryDTO { Title = expectedTitle } ;
            storiesService.Setup(x => x.ReplaceStoryAsync(storyDTO,userId)).ReturnsAsync(status);
            storiesController.ControllerContext = httpContext;

            //Act
            var result =await storiesController.UpdateStory(storyDTO) as NotFoundResult;

            //Assert
            Assert.NotNull(result);
            Assert.Equal(404, result.StatusCode);
        }

        [Fact]
        public async Task TestUpdateStoryOk()
        {
            //Arrange
            string expectedTitle = "LoremIpsum";
            string userId = "akash";
            DBStatus status = DBStatus.Modified;
            RequestStoryDTO storyDTO = new RequestStoryDTO { Title = expectedTitle };
            storiesService.Setup(x => x.ReplaceStoryAsync(storyDTO, userId)).ReturnsAsync(status);
            storiesController.ControllerContext = httpContext;

            //Act
            var result = await storiesController.UpdateStory(storyDTO) as OkObjectResult;
           
            //Assert
            Assert.NotNull(result);

            var response = result.Value as ResponseStatusDTO;


            Assert.Equal(200, result.StatusCode);
            Assert.Equal("Modified",response.Message);
        }

        [Fact]
        public async Task TestRemoveStoryForbidden()
        {
            //Arrange
            Guid expectedStoryId =Guid.NewGuid();
            storiesService.Setup(x => x.RemoveStoryAsync(expectedStoryId,"akash")).ReturnsAsync(DBStatus.Forbidden);
            storiesController.ControllerContext = httpContext;

            //Act
            var result =await storiesController.RemoveStory(expectedStoryId) as StatusCodeResult;

            testOutputHelper.WriteLine(result.ToString());
               
            //Assert
            Assert.NotNull(result);
            Assert.Equal(403,result.StatusCode);
        }
    }




    public class RequestStoriesTestData : IEnumerable<object[]>
    {
        
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return  new object[] { new RequestStoryDTO { StoryId = Guid.NewGuid(), Title = "", Body = "", PublishedDate = DateTime.UtcNow } };
            yield return new object[] { new RequestStoryDTO { StoryId = Guid.NewGuid(), Title = "", Body = "", PublishedDate = DateTime.UtcNow } };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }

   

    public class ResponseStories : IEnumerable<ResponseStoryDTO>
    {
        public readonly List<ResponseStoryDTO> stories = new List<ResponseStoryDTO> {
                               new ResponseStoryDTO {Title="LoremIpsum"  }
                            };
        public IEnumerator<ResponseStoryDTO> GetEnumerator() => stories.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
}
