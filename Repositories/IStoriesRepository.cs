using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog_Rest_Api.Persistent_Model;

namespace Blog_Rest_Api.Repositories{
    interface IStoriesRepository
    {
        Task<int> AddStoryAsync(Story story);
        Task<List<Story>> GetStoryAsync();
        Task<Story> GetStoryAsync(Guid storyId);
    }
}