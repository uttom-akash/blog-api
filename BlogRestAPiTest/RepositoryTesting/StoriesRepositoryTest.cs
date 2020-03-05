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
using Xunit.Abstractions;

namespace BlogRestAPiTest.RepositoryTesting
{
    public class StoriesRepositoryTest
    {
        private readonly Mock<IMapper> mapper;
        private readonly StoriesRepository storiesRepository;
        private readonly Guid expectedStoryId;
        private readonly User expectedUser;
        private readonly RequestStoryDTO expectedRequestStoryDTO;
        private readonly Story expectedStory;
        private readonly ITestOutputHelper testOutputHelper;

        public BlogContext dbContext { get; }

        public StoriesRepositoryTest(ITestOutputHelper testOutputHelper)
        {
            
            dbContext = CreateDBContext();
            mapper = new Mock<IMapper>();
            storiesRepository = new StoriesRepository(dbContext,mapper.Object);

            //arrange
            expectedStoryId = Guid.NewGuid();
            expectedUser = new User { UserId = "akash" };
            expectedRequestStoryDTO = new RequestStoryDTO {StoryId=expectedStoryId,Title="Lorem Ipsum",Body="aaaaaaaaaaaaaaaaaaaaaaaaaaaaa",PublishedDate=DateTime.UtcNow};
            expectedStory = new Story { StoryId = expectedStoryId, Title = expectedRequestStoryDTO.Title, Body = expectedRequestStoryDTO.Body, PublishedDate = expectedRequestStoryDTO.PublishedDate,Author =expectedUser};
            
            
            
            this.testOutputHelper = testOutputHelper;
        }


        public BlogContext CreateDBContext() {
            var options = new DbContextOptionsBuilder<BlogContext>()
                .UseInMemoryDatabase(databaseName: "Add_writes_to_database")
                .Options;

            IOptions<DatabaseInfo> databaseInfo = Options.Create<DatabaseInfo>(new DatabaseInfo { Host = "localhost", DatabaseName = "BlogDB" });
            var dbContext = new BlogContext(options,databaseInfo);
            return dbContext;
        }

        
        [Fact]
        public async Task TestAddStoryAdded()
        {
            //Arrange 
            DBStatus expectedStatus = DBStatus.Added;
            mapper.Setup(m => m.Map<Story>(It.IsAny<RequestStoryDTO>())).Returns(expectedStory);
            dbContext.Users.Add(expectedUser);
            dbContext.SaveChanges();

            //Act
            DBStatus actualStatus =await storiesRepository.AddStoryAsync(expectedRequestStoryDTO,"akash");
            
            //Assert
            Assert.Equal(expectedStatus, actualStatus);
        }

        [Fact]
        public async Task TestGetStory()
        {
            //Arrange
            dbContext.Stories.Add(expectedStory);
            dbContext.SaveChanges();


            //Act
            Story actualStory = await storiesRepository.GetAsync(expectedStoryId);

            //Assert
            Assert.NotNull(actualStory);
            Assert.Equal(expectedStory.StoryId,actualStory.StoryId);
        }

        [Fact]
        public async Task TestReplaceStoryModified()
        {
            //Arrange 
            DBStatus expectedStatus = DBStatus.Modified;
            dbContext.Users.Add(expectedUser);
            dbContext.Stories.Add(expectedStory);
            dbContext.SaveChanges();
            expectedRequestStoryDTO.Title = "changed";

            //Act
            DBStatus actualStatus = await storiesRepository.ReplaceStoryAsync(expectedRequestStoryDTO, "akash");

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
