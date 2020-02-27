using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Utils;

namespace Blog_Rest_Api.Services{
    public interface IStoriesService
    {
        Task<DBStatus> CreateStoryAsync(StoryDTO story);
        Task<List<StoryDTO>> GetStoryAsync();
        Task<StoryDTO> GetStoryAsync(Guid storyId);
        Task<DBStatus> ReplaceStoryAsync(StoryDTO storyDTO);
        Task<DBStatus> RemoveStoryAsync(Guid storyId);

    }
}