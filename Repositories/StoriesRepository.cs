using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Rest_Api.Persistent_Model;
using Microsoft.EntityFrameworkCore;

namespace Blog_Rest_Api.Repositories{
    class StoriesRepository : IStoriesRepository
    {
        private BlogContext blogContext;
        public StoriesRepository(BlogContext blogContext)
        {
           this.blogContext=blogContext;
        }

        public async Task<int> AddStoryAsync(Story story)
        {
            blogContext.Stories.Add(story);
            var resultStatus=await blogContext.SaveChangesAsync();
            return resultStatus;
        }

        public async Task<List<Story>> GetStoryAsync()
        {
            return await blogContext.Stories.AsNoTracking().ToListAsync();
        }

        public async Task<Story> GetStoryAsync(Guid storyId)
        {
            return await blogContext.Stories.AsNoTracking().Where(story=>story.StoryId==storyId).FirstOrDefaultAsync();
        }

        public async Task<string> ReplaceStoryAsync(Story story)
        {
            
            Story persistentStory=await blogContext.Stories.AsNoTracking().FirstOrDefaultAsync(s=>s.StoryId==story.StoryId);
            if(persistentStory==null)
                return "notFound";
                
            blogContext.Stories.Attach(story).State=EntityState.Modified;
            var resultStatus=await blogContext.SaveChangesAsync();
            return resultStatus==0 ? "notModified" : "modified" ;
        }

    }
}