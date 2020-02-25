using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Repositories;

namespace Blog_Rest_Api.Services{
    class StoriesService:IStoriesService
    {
        private readonly IStoriesRepository storiesRepository;

        public StoriesService(IStoriesRepository storiesRepository)
        {
            this.storiesRepository = storiesRepository;
        }

        public async Task<bool> CreateStoryAsync(StoryDTO storyDTO)
        {
            int status=await storiesRepository.AddStoryAsync(storyDTO);
            return status==1 && true;
        }
    }
}