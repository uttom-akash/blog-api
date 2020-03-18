
using Blog_Rest_Api;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Repositories;
using Blog_Rest_Api.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace BlogRestAPiTest.RepositoryTesting
{
    public class StoriesRepositoryTest
    {
        private StoriesRepository storiesRepository { get; set; }
        private Guid expectedStoryId;
        private User expectedUser;
        private RequestStoryDTO expectedRequestStoryDTO;
        private Story expectedStory;
        private ITestOutputHelper testOutputHelper;

        public BlogContext dbContext { get; set; }

        public StoriesRepositoryTest(ITestOutputHelper testOutputHelper)
        {
            Initialize().Wait();
            this.testOutputHelper = testOutputHelper;
        }

        public async Task Initialize()
        {
            dbContext = CreateDBContext();
            storiesRepository = new StoriesRepository(dbContext);


            //arrange
            expectedStoryId = Guid.NewGuid();
            expectedUser = new User { UserId = "akash" };
            expectedRequestStoryDTO = new RequestStoryDTO { StoryId = expectedStoryId, Title = "Lorem Ipsum", Body = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaa", PublishedDate = DateTime.UtcNow };
            expectedStory = new Story { StoryId = expectedStoryId, Title = expectedRequestStoryDTO.Title, Body = expectedRequestStoryDTO.Body, PublishedDate = expectedRequestStoryDTO.PublishedDate };
            expectedStory.AuthorId = expectedUser.UserId;

            //insert user
            if (await dbContext.Users.CountAsync() < 1)
            {
                dbContext.Users.Add(expectedUser);
                dbContext.SaveChanges();
            }

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
        public async Task TestAddStory_Added()
        {
            //Arrange 
            DBStatus expectedStatus = DBStatus.Added;

            //Act
            DBStatus actualStatus = await storiesRepository.AddStoryAsync(expectedStory);

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
            Assert.Equal(expectedStory.StoryId, actualStory.StoryId);
        }

        [Fact]
        public async Task TestReplaceStory_Modified()
        {
            //Arrange 
            DBStatus expectedStatus = DBStatus.Modified;
            dbContext.Stories.Add(expectedStory);
            dbContext.SaveChanges();

            expectedRequestStoryDTO.Title = "changed";

            //Act
            DBStatus actualStatus = await storiesRepository.ReplaceStoryAsync(expectedRequestStoryDTO, "akash");

            //Assert
            Assert.Equal(expectedStatus, actualStatus);
        }



    }
}
