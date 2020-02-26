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
    }
}