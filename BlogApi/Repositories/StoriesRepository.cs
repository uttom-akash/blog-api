using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace Blog_Rest_Api.Repositories{
    public class StoriesRepository : IStoriesRepository
    {
        private BlogContext blogContext;
        private readonly IMapper mapper;

        public StoriesRepository(BlogContext blogContext,IMapper mapper)
        {
           this.blogContext=blogContext;
            this.mapper = mapper;
        }

        public async Task<DBStatus> AddStoryAsync(RequestStoryDTO storyDTO,string userId)
        {
            Story story=mapper.Map<Story>(storyDTO);
            story.AuthorId=userId;
            blogContext.Stories.Add(story);
            var resultStatus=await blogContext.SaveChangesAsync();
            return resultStatus==1 ? DBStatus.Added : DBStatus.Failed ;
        }

       public async Task<List<T>> GetAllAsync<T>(int skip, int top)
        {
            return await blogContext.Stories
                                    .Include(story=>story.Author)
                                    .AsNoTracking()
                                    .OrderByDescending(story=>story.PublishedDate)
                                    .Skip(skip)
                                    .Take(top)
                                    .Select(story=>mapper.Map<T>(story))
                                    .ToListAsync();
        }

        public async Task<List<ResponseStoryDTO>> GetUserStoriesAsync(string userId, int skip, int top)
        {
            return await blogContext.Stories
                                    .AsNoTracking()
                                    .Where(story=>story.AuthorId==userId)
                                    .Include(story=>story.Author)
                                    .OrderByDescending(story=>story.PublishedDate)
                                    .Skip(skip)
                                    .Take(top)
                                    .Select(story=>mapper.Map<ResponseStoryDTO>(story))
                                    .ToListAsync();
        }

        public async Task<Story> GetAsync(Guid storyId)
        {
            Story story=await blogContext.Stories.Include(story=>story.Author)
                                            .AsNoTracking()
                                            .FirstOrDefaultAsync(story=>story.StoryId.Equals(storyId));
            
            return story;
        }

        public async Task<List<ResponseStoryDTO>> SearchAsync(string content,int skip, int top)
        {
            //If Sql Server is not configured with full text search
            //Then Query throws error
            //Then Execute query in catch block
            try
            {
                return await blogContext.Stories.Where(story=>EF.Functions.FreeText(story.Title,content) || EF.Functions.FreeText(story.Body,content))
                                                .Include(story=>story.Author)
                                                .AsNoTracking()
                                                .OrderByDescending(story=>story.PublishedDate)
                                                .Skip(skip)
                                                .Take(top)
                                                .Select(story=>mapper.Map<ResponseStoryDTO>(story))
                                                .ToListAsync();   
            }
            catch (System.Exception)
            {
                
                return await blogContext.Stories.Where(story=>EF.Functions.Like(story.Title,$"_%{content}_%") || EF.Functions.Like(story.Body,$"_%{content}_%"))
                                                .Include(story=>story.Author)
                                                .AsNoTracking()
                                                .OrderByDescending(story=>story.PublishedDate)
                                                .Skip(skip)
                                                .Take(top)
                                                .Select(story=>mapper.Map<ResponseStoryDTO>(story))
                                                .ToListAsync();
            }
        }

        public async Task<DBStatus> ReplaceStoryAsync(RequestStoryDTO storyDTO,string userId)
        {
            Story persistentStory=await blogContext.Stories.FirstOrDefaultAsync(s=>s.StoryId==storyDTO.StoryId);
           
            if(persistentStory==null)
                return DBStatus.NotFound;

            if(userId!=persistentStory.AuthorId)
                return DBStatus.Forbidden;
            
            persistentStory.Title=storyDTO.Title;
            persistentStory.Body=storyDTO.Body;    
            persistentStory.PublishedDate=storyDTO.PublishedDate;

            var resultStatus=await blogContext.SaveChangesAsync();
            return resultStatus==0 ? DBStatus.NotModified : DBStatus.Modified ;
        }

        public async Task<DBStatus> RemoveStoryAsync(Guid storyId,string userId){
            Story persistentStory=await blogContext.Stories.AsNoTracking().FirstOrDefaultAsync(s=>s.StoryId==storyId);
            
            if(persistentStory==null)
                return DBStatus.NotFound;
            
            if(userId!=persistentStory.AuthorId)
                return DBStatus.Forbidden;
                
            blogContext.Stories.Remove(persistentStory);
            var resultStatus=await blogContext.SaveChangesAsync();
            return resultStatus==0 ? DBStatus.NotDeleted : DBStatus.Deleted ;
        }

        
    }
}