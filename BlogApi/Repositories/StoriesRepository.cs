using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Rest_Api.Crypto;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace Blog_Rest_Api.Repositories
{
    public class StoriesRepository : IStoriesRepository
    {
        private BlogContext blogContext;
        public StoriesRepository(BlogContext blogContext)
        {
            this.blogContext = blogContext;
        }

        public async Task<DBStatus> AddStoryAsync(Story story)
        {
            blogContext.Stories.Add(story);
            story.LastModified = DateTime.UtcNow;

            var resultStatus = await blogContext.SaveChangesAsync();
            return resultStatus == 1 ? DBStatus.Added : DBStatus.Failed;
        }

        public async Task<KeyValuePair<int, IEnumerable<Story>>> GetAllAsync(int skip, int top)
        {
            IEnumerable<Story> stories = blogContext.Stories
                                    .AsNoTracking()
                                    .OrderByDescending(story => story.PublishedDate)
                                    .Skip(skip)
                                    .Take(top)
                                    .Include(story => story.Author).AsEnumerable();
            int totalStories = await blogContext.Stories.CountAsync();
            return new KeyValuePair<int, IEnumerable<Story>>(totalStories, stories);
        }

        public async Task<KeyValuePair<int, IEnumerable<Story>>> GetUserStoriesAsync(string userId, int skip, int top)
        {
            var query = blogContext.Stories
                                   .AsNoTracking()
                                   .Where(story => story.AuthorId == userId);

            IEnumerable<Story> stories = query
                                   .Include(story => story.Author)
                                   .OrderByDescending(story => story.PublishedDate)
                                   .Skip(skip)
                                   .Take(top).AsEnumerable();
            int totalStories = await query.CountAsync();
            return new KeyValuePair<int, IEnumerable<Story>>(totalStories, stories);
        }

        public async Task<Story> GetAsync(Guid storyId)
        {
            Story story = await blogContext.Stories
                                            .AsNoTracking()
                                            .Where(story => story.StoryId.Equals(storyId))
                                            .Include(story => story.Author)
                                            .FirstOrDefaultAsync();

            return story;
        }

        public async Task<KeyValuePair<int, IEnumerable<Story>>> SearchAsync(string content, int skip, int top)
        {
            //If Sql Server is not configured with full text search
            //Then Query throws error
            //Then Execute query in catch block
            IEnumerable<Story> stories = null;
            int totalStories = 0;
            try
            {
                var query = blogContext.Stories.Where(story => EF.Functions.FreeText(story.Title, content) || EF.Functions.FreeText(story.Body, content));

                stories = query.Include(story => story.Author)
                             .Skip(skip)
                             .Take(top)
                             .AsNoTracking()
                             .OrderByDescending(story => story.PublishedDate).AsEnumerable();
                totalStories = await query.CountAsync();
            }
            catch (System.Exception)
            {
                var query = blogContext.Stories.Where(story => EF.Functions.Like(story.Title, $"_%{content}_%") || EF.Functions.Like(story.Body, $"_%{content}_%"));

                stories = query
                            .Include(story => story.Author)
                            .AsNoTracking()
                            .OrderByDescending(story => story.PublishedDate)
                            .Skip(skip)
                            .Take(top).AsEnumerable();

                totalStories = await query.CountAsync();
            }

            return new KeyValuePair<int, IEnumerable<Story>>(totalStories, stories);
        }

        public async Task<KeyValuePair<int, IEnumerable<Story>>> SearchUserStoriesAsync(string userId, string content, int skip, int top)
        {
            //If Sql Server is not configured with full text search
            //Then Query throws error
            //Then Execute query in catch block
            IEnumerable<Story> stories = null;
            int totalStories = 0;
            try
            {
                var query = blogContext.Stories.Where(story => story.AuthorId == userId && (EF.Functions.FreeText(story.Title, content) || EF.Functions.FreeText(story.Body, content)));

                stories = query.Include(story => story.Author)
                             .Skip(skip)
                             .Take(top)
                             .AsNoTracking()
                             .OrderByDescending(story => story.PublishedDate).AsEnumerable();
                totalStories = await query.CountAsync();
            }
            catch (System.Exception)
            {
                var query = blogContext.Stories.Where(story => story.AuthorId == userId && (EF.Functions.Like(story.Title, $"_%{content}_%") || EF.Functions.Like(story.Body, $"_%{content}_%")));

                stories = query
                            .Include(story => story.Author)
                            .AsNoTracking()
                            .OrderByDescending(story => story.PublishedDate)
                            .Skip(skip)
                            .Take(top).AsEnumerable();

                totalStories = await query.CountAsync();
            }

            return new KeyValuePair<int, IEnumerable<Story>>(totalStories, stories);
        }

        public async Task<DBStatus> ReplaceStoryAsync(RequestStoryDTO storyDTO, string userId)
        {
            Story persistentStory = await blogContext.Stories.FirstOrDefaultAsync(story => story.StoryId == storyDTO.StoryId);
            if (persistentStory == null)
                return DBStatus.NotFound;


            if (userId != persistentStory.AuthorId)
                return DBStatus.Forbidden;

            persistentStory.Title = storyDTO.Title;
            persistentStory.Body = storyDTO.Body;
            persistentStory.PublishedDate = storyDTO.PublishedDate;
            persistentStory.LastModified = DateTime.UtcNow;

            var resultStatus = await blogContext.SaveChangesAsync();
            return resultStatus == 0 ? DBStatus.NotModified : DBStatus.Modified;
        }

        public async Task<DBStatus> RemoveStoryAsync(Guid storyId, string userId)
        {
            Story persistentStory = await blogContext.Stories.AsNoTracking().FirstOrDefaultAsync(s => s.StoryId == storyId);
            if (persistentStory == null)
                return DBStatus.NotFound;

            if (userId != persistentStory.AuthorId)
                return DBStatus.Forbidden;

            blogContext.Stories.Remove(persistentStory);
            var resultStatus = await blogContext.SaveChangesAsync();
            return resultStatus == 0 ? DBStatus.NotDeleted : DBStatus.Deleted;
        }


    }
}