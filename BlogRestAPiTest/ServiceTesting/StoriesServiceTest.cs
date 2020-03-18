using AutoMapper;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Repositories;
using Blog_Rest_Api.Services;
using Blog_Rest_Api.Utils;
using BlogRestAPiTest.Data;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace BlogRestAPiTest.ServiceTesting
{
    public class StoriesServiceTest
    {
        private readonly Mock<IStoriesRepository> storiesRepository;
        private readonly IStoriesService storiesService;
        private readonly Mock<IMapper> mapper;
        private readonly ITestOutputHelper testOutputHelper;

        public StoriesServiceTest(ITestOutputHelper testOutputHelper)
        {

            storiesRepository = new Mock<IStoriesRepository>();
            mapper = new Mock<IMapper>();
            storiesService = new StoriesService(storiesRepository.Object, mapper.Object);


            this.testOutputHelper = testOutputHelper;
        }



        [Theory]
        [ClassData(typeof(RequestStoriesAndStoriesTestData))]
        public async Task TestCreateStory_Added(RequestStoryDTO requestStoryDTO, Story story)
        {
            //Arrange
            string userId = "akash";
            DBStatus expectedStatus = DBStatus.Added;
            mapper.Setup(m => m.Map<Story>(requestStoryDTO)).Returns(story);
            storiesRepository.Setup(x => x.AddStoryAsync(story))
                             .ReturnsAsync(expectedStatus);
            //Act
            DBStatus status = await storiesService.CreateStoryAsync(requestStoryDTO, userId);

            //Assert
            Assert.Equal(expectedStatus, status);
        }

        [Theory]
        [ClassData(typeof(RequestStoriesAndStoriesTestData))]
        public async Task TestCreateStory_Failed(RequestStoryDTO requestStoryDTO, Story story)
        {
            //Arrange
            string userId = "akash";
            DBStatus expectedStatus = DBStatus.Failed;
            mapper.Setup(m => m.Map<Story>(requestStoryDTO)).Returns(story);
            storiesRepository.Setup(x => x.AddStoryAsync(story))
                             .ReturnsAsync(expectedStatus);
            //Act
            DBStatus status = await storiesService.CreateStoryAsync(requestStoryDTO, userId);

            //Assert
            Assert.Equal(expectedStatus, status);
        }


        [Theory]
        [ClassData(typeof(RequestStoriesTestData))]
        public async Task TestReplaceStory_NotModified(RequestStoryDTO requestStoryDTO)
        {
            //Arrange
            string userId = "akash";
            DBStatus expectedStatus = DBStatus.NotModified;
            storiesRepository.Setup(x => x.ReplaceStoryAsync(requestStoryDTO, userId)).ReturnsAsync(expectedStatus);

            //Act
            DBStatus actualStatus = await storiesService.ReplaceStoryAsync(requestStoryDTO, userId);

            //Assert
            Assert.Equal(expectedStatus, actualStatus);
        }

        [Theory]
        [ClassData(typeof(Data.RequestStoriesTestData))]
        public async Task TestReplaceStory_Modified(RequestStoryDTO requestStoryDTO)
        {
            //Arrange
            string userId = "akash";
            DBStatus expectedStatus = DBStatus.Modified;
            storiesRepository.Setup(x => x.ReplaceStoryAsync(requestStoryDTO, userId)).ReturnsAsync(expectedStatus);


            //Act
            DBStatus actualStatus = await storiesService.ReplaceStoryAsync(requestStoryDTO, userId);

            //Assert
            Assert.Equal(expectedStatus, actualStatus);
        }


        [Fact]
        public async Task TestRemoveStory_Deleted()
        {
            //Arrange
            string userId = "akash";
            DBStatus expectedStatus = DBStatus.Deleted;
            Guid storyId = Guid.NewGuid();
            storiesRepository.Setup(x => x.RemoveStoryAsync(storyId, userId)).ReturnsAsync(expectedStatus);

            //Act
            DBStatus actualStatus = await storiesService.RemoveStoryAsync(storyId, userId);

            //Assert
            Assert.Equal(expectedStatus, actualStatus);
        }

        [Fact]
        public async Task TestRemoveStory_NotDeleted()
        {
            //Arrange
            string userId = "akash";
            DBStatus expectedStatus = DBStatus.NotDeleted;
            Guid storyId = Guid.NewGuid();
            storiesRepository.Setup(x => x.RemoveStoryAsync(storyId, userId)).ReturnsAsync(expectedStatus);

            //Act
            DBStatus actualStatus = await storiesService.RemoveStoryAsync(storyId, userId);

            //Assert
            Assert.Equal(expectedStatus, actualStatus);
        }

        [Fact]
        public async Task TestRemoveStory_Forbidden()
        {
            //Arrange
            string userId = "akash";
            DBStatus expectedStatus = DBStatus.Forbidden;
            Guid storyId = Guid.NewGuid();
            storiesRepository.Setup(x => x.RemoveStoryAsync(storyId, userId)).ReturnsAsync(expectedStatus);

            //Act
            DBStatus actualStatus = await storiesService.RemoveStoryAsync(storyId, userId);

            //Assert
            Assert.Equal(expectedStatus, actualStatus);
        }

    }


}
