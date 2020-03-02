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
    class StoriesRepository : IStoriesRepository
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
            
            Story story=new Story{
                StoryId=storyDTO.StoryId,
                Title=storyDTO.Title,
                Body=storyDTO.Body,
                PublishedDate=storyDTO.PublishedDate,
                AuthorId=userId
            };   
            blogContext.Stories.Add(story);
            var resultStatus=await blogContext.SaveChangesAsync();
            return resultStatus==0 ? DBStatus.Failed : DBStatus.Added;
        }

        public async Task<List<ResponseStoryDTO>> GetStoryAsync()
        {
            return await blogContext.Stories
                                    .Include(story=>story.Author)
                                    .AsNoTracking()
                                    .Select(story=>new ResponseStoryDTO{
                                        StoryId=story.StoryId,
                                        Title=story.Title,
                                        Body=story.Body,
                                        PublishedDate=story.PublishedDate,
                                        Author=new AuthorDTO{AuthorId=story.AuthorId,FirstName=story.Author.FirstName,LastName=story.Author.LastName}
                                    })
                                    .ToListAsync();
        }

        public async Task<List<ResponseStoryDTO>> GetStoryAsync(int skip,int top)
        {
            return await blogContext.Stories
                                    .Include(story=>story.Author)
                                    .AsNoTracking()
                                    .Skip(skip)
                                    .Take(top)
                                    .Select(story=>new ResponseStoryDTO{
                                        StoryId=story.StoryId,
                                        Title=story.Title,
                                        Body=story.Body,
                                        PublishedDate=story.PublishedDate,
                                        Author=new AuthorDTO{AuthorId=story.AuthorId,FirstName=story.Author.FirstName,LastName=story.Author.LastName}
                                    })
                                    .ToListAsync();
        }

        public async Task<ResponseStoryDTO> GetStoryAsync(Guid storyId)
        {
            return await blogContext.Stories.Include(story=>story.Author)
                                            .AsNoTracking()
                                            .Select(story=>new ResponseStoryDTO{
                                                StoryId=story.StoryId,
                                                Title=story.Title,
                                                Body=story.Body,
                                                PublishedDate=story.PublishedDate,
                                                 Author=new AuthorDTO{AuthorId=story.AuthorId,FirstName=story.Author.FirstName,LastName=story.Author.LastName}
                                            }).FirstOrDefaultAsync(story=>story.StoryId==storyId);
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