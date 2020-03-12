using AutoMapper;
using Blog_Rest_Api;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Repositories;
using Blog_Rest_Api.Services;
using Blog_Rest_Api.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BlogRestAPiTest.ServiceTesting
{
    public class StoriesServiceTest
    {
        private readonly Mock<IStoriesRepository> storiesRepository;
        private readonly IStoriesService storiesService;
        private readonly Guid expectedStoryId;
        private readonly User expectedUser;
        private readonly RequestStoryDTO expectedRequestStoryDTO;
        private readonly ResponseStoryDTO expectedResponseStoryDTO;
        private readonly Story expectedStory;
        private readonly Mock<IMapper> mapper;

        public StoriesServiceTest()
        {
            
            storiesRepository=new Mock<IStoriesRepository>();
            mapper = new Mock<IMapper>();
            storiesService = new StoriesService(storiesRepository.Object,mapper.Object);
       
            expectedStoryId = Guid.NewGuid();
            expectedUser = new User { UserId = "akash" };
            expectedRequestStoryDTO = new RequestStoryDTO {StoryId=expectedStoryId,Title="Lorem Ipsum",Body="aaaaaaaaaaaaaaaaaaaaaaaaaaaaa",PublishedDate=DateTime.UtcNow};
            expectedResponseStoryDTO = new ResponseStoryDTO { StoryId = expectedStoryId, Title = "Lorem Ipsum", Body = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaa", PublishedDate = DateTime.UtcNow };
            expectedStory = new Story { StoryId = expectedStoryId, Title = expectedRequestStoryDTO.Title, Body = expectedRequestStoryDTO.Body, PublishedDate = expectedRequestStoryDTO.PublishedDate,Author =expectedUser};
           
        }

        public BlogContext CreateDBContext()
        {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            IOptions<DatabaseInfo> databaseInfo = Options.Create<DatabaseInfo>(new DatabaseInfo { Host = "localhost", DatabaseName = "BlogDB" });
            var dbContext = new BlogContext(options, databaseInfo);
            return dbContext;
        }

        [Fact]
        public async Task TestCreateStoryAdded()
        {
            //Arrange
            string userId="akash";
            DBStatus expectedStatus = DBStatus.Added;
            mapper.Setup(m => m.Map<Story>(expectedRequestStoryDTO)).Returns(expectedStory);
            storiesRepository.Setup(x => x.AddStoryAsync(expectedStory))
                             .ReturnsAsync(expectedStatus);
            //Act
            DBStatus  status=await storiesService.CreateStoryAsync(expectedRequestStoryDTO,userId);

            //Assert
            Assert.Equal(expectedStatus,status);
        }

        [Fact]
        public async Task TestCreateStoryFailed()
        {
             //Arrange
            string userId="akash";
            DBStatus expectedStatus = DBStatus.Failed;
            mapper.Setup(m => m.Map<Story>(expectedRequestStoryDTO)).Returns(expectedStory);
            storiesRepository.Setup(x => x.AddStoryAsync(expectedStory))
                             .ReturnsAsync(expectedStatus);
            //Act
            DBStatus  status=await storiesService.CreateStoryAsync(expectedRequestStoryDTO,userId);

            //Assert
            Assert.Equal(expectedStatus,status);
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
            yield return new object[] { new KeyValuePair<int,IQueryable<Story>>(1,new List<Story>{new Story()}.AsQueryable())};
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    }
}
