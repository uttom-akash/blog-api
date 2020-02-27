using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Utils;

namespace Blog_Rest_Api.Repositories{
    interface IStoriesRepository
    {
        Task<DBStatus> AddStoryAsync(Story story);
        Task<List<Story>> GetStoryAsync();
        Task<Story> GetStoryAsync(Guid storyId);
        Task<DBStatus> ReplaceStoryAsync(Story story);
        Task<DBStatus> RemoveStoryAsync(Guid storyId);
    }
}