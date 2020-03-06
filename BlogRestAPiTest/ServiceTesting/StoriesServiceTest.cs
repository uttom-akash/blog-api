using AutoMapper;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Repositories;
using Blog_Rest_Api.Services;
using Blog_Rest_Api.Utils;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlogRestAPiTest.ServiceTesting
{
    public class StoriesServiceTest
    {
        private readonly Mock<IStoriesRepository> storiesRepository;
        private readonly IStoriesService storiesService;
        private readonly Mock<IMapper> mapper;

        public StoriesServiceTest()
        {
            storiesRepository=new Mock<IStoriesRepository>();
            mapper = new Mock<IMapper>();
            storiesService = new StoriesService(storiesRepository.Object,mapper.Object);
        }

        [Fact]
        public async Task TestCreateStoryAdded()
        {
            //Arrange
            string userId = "akash";
            DBStatus expectedStatus = DBStatus.Added;
            RequestStoryDTO storyDTO = It.IsAny<RequestStoryDTO>();
            storiesRepository.Setup(x => x.AddStoryAsync(storyDTO, userId))
                             .ReturnsAsync(expectedStatus);
            //Act
            DBStatus  status=await storiesService.CreateStoryAsync(storyDTO, userId);

            //Assert
            Assert.Equal(expectedStatus,status);
        }

        [Fact]
        public async Task TestCreateStoryFailed()
        {
            //Arrange
            string userId = "akash";
            DBStatus expectedStatus = DBStatus.Failed;
            RequestStoryDTO storyDTO = It.IsAny<RequestStoryDTO>();
            storiesRepository.Setup(x => x.AddStoryAsync(storyDTO, userId))
                             .ReturnsAsync(expectedStatus);
            //Act
            DBStatus status = await storiesService.CreateStoryAsync(storyDTO, userId);

            //Assert
            Assert.Equal(expectedStatus, status);
        }

        [Theory]
        [ClassData(typeof(ResponseStoriesTestData))]
        public async Task TestGetStories(List<ResponseStoryDTO> expectedStories)
        {
            //Arrange
            storiesRepository.Setup(x => x.GetAllAsync<ResponseStoryDTO>(It.IsAny<int>(), It.IsAny<int>())).ReturnsAsync(expectedStories);

            //Act
            List<ResponseStoryDTO> actualStories =await storiesService.GetStoriesAsync("",0, 50);

            //Assert
            Assert.Equal(expectedStories.Count,actualStories.Count);
        }

        [Fact]
        public async Task TestReplaceStoryNotModified()
        {
            //Arrange
            string userId = "akash";
            RequestStoryDTO storyDTO = It.IsAny<RequestStoryDTO>();
            DBStatus expectedStatus = DBStatus.NotModified;
            storiesRepository.Setup(x => x.ReplaceStoryAsync(storyDTO, userId)).ReturnsAsync(expectedStatus);

            //Act
            DBStatus actualStatus =await storiesService.ReplaceStoryAsync(storyDTO, userId);

            //Assert
            Assert.Equal(expectedStatus,actualStatus);
        }

        [Fact]
        public async Task TestReplaceStoryModified()
        {
            //Arrange
            string userId = "akash";
            DBStatus expectedStatus = DBStatus.Modified;
            RequestStoryDTO storyDTO = It.IsAny<RequestStoryDTO>();
            storiesRepository.Setup(x => x.ReplaceStoryAsync(storyDTO, userId)).ReturnsAsync(expectedStatus);

            //Act
            DBStatus actualStatus = await storiesService.ReplaceStoryAsync(storyDTO, userId);

            //Assert
            Assert.Equal(expectedStatus, actualStatus);
        }


        [Fact]
        public async Task TestRemoveStoryDeleted()
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
        public async Task TestRemoveStoryNotDeleted()
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
        public async Task TestRemoveStoryForbidden()
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

    public class ResponseStoriesTestData : IEnumerable<object[]>
    {

        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { new List<ResponseStoryDTO> {
               new ResponseStoryDTO{ },
               new ResponseStoryDTO{ },
               new ResponseStoryDTO{ }
            } };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
