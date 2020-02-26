using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;

namespace Blog_Rest_Api.Services{
    public interface IStoriesService
    {
        Task<bool> CreateStoryAsync(StoryDTO story);
    }
}