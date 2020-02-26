using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;

namespace Blog_Rest_Api.Services{
    public interface IStoriesService
    {
        Task<bool> CreateStoryAsync(StoryDTO story);
        Task<List<StoryDTO>> GetStoryAsync();
        Task<StoryDTO> GetStoryAsync(Guid storyId);
        Task<string> ReplaceStoryAsync(StoryDTO storyDTO);
        Task<string> RemoveStoryAsync(Guid storyId);

    }
}