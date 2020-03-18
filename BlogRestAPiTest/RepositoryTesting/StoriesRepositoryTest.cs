
using Blog_Rest_Api;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Repositories;
using Blog_Rest_Api.Utils;
using BlogRestAPiTest.Data;
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
        private User expectedUser;
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
            expectedUser = new User { UserId = "akash" };

            //insert user once
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


        [Theory]
        [ClassData(typeof(StoriesTestData))]
        public async Task TestAddStory_Added(Story story)
        {
            //Arrange 
            DBStatus expectedStatus = DBStatus.Added;

            //Act
            DBStatus actualStatus = await storiesRepository.AddStoryAsync(story);

            //Assert
            Assert.Equal(expectedStatus, actualStatus);
        }

        [Theory]
        [ClassData(typeof(StoriesTestData))]
        public async Task TestGetStory(Story story)
        {
            //Arrange
            dbContext.Stories.Add(story);
            dbContext.SaveChanges();


            //Act
            Story actualStory = await storiesRepository.GetAsync(story.StoryId);

            //Assert
            Assert.NotNull(actualStory);
            Assert.Equal(story.StoryId, actualStory.StoryId);
        }

        [Theory]
        [ClassData(typeof(RequestStoriesAndStoriesTestData))]
        public async Task TestReplaceStory_Modified(RequestStoryDTO requestStoryDTO, Story story)
        {
            //Arrange 
            DBStatus expectedStatus = DBStatus.Modified;
            dbContext.Stories.Add(story);
            dbContext.SaveChanges();

            requestStoryDTO.Title = "changed";

            //Act
            DBStatus actualStatus = await storiesRepository.ReplaceStoryAsync(requestStoryDTO, "akash");

            //Assert
            Assert.Equal(expectedStatus, actualStatus);
        }



    }
}
