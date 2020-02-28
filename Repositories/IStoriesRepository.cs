using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Utils;

namespace Blog_Rest_Api.Repositories{
    interface IStoriesRepository
    {
        Task<DBStatus> AddStoryAsync(StoryDTO story,string userId);
        Task<List<ResponseStoryDTO>> GetStoryAsync();
        Task<ResponseStoryDTO> GetStoryAsync(Guid storyId);
        Task<DBStatus> ReplaceStoryAsync(StoryDTO story,string userId);
        Task<DBStatus> RemoveStoryAsync(Guid storyId,string userId);
    }
}