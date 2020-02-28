using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Utils;

namespace Blog_Rest_Api.Services{
    public interface IStoriesService
    {
        Task<DBStatus> CreateStoryAsync(StoryDTO story,string userId);
        Task<List<ResponseStoryDTO>> GetStoryAsync();
        Task<List<ResponseStoryDTO>> GetStoryAsync(int skip,int top);
        Task<ResponseStoryDTO> GetStoryAsync(Guid storyId);
        Task<DBStatus> ReplaceStoryAsync(StoryDTO storyDTO,string userId);
        Task<DBStatus> RemoveStoryAsync(Guid storyId,string userId);

    }
}