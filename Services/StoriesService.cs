using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Repositories;

namespace Blog_Rest_Api.Services{
    class StoriesService:IStoriesService
    {
        private readonly IStoriesRepository storiesRepository;
        private readonly IMapper mapper;

        public StoriesService(IStoriesRepository storiesRepository,IMapper mapper)
        {
            this.storiesRepository = storiesRepository;
            this.mapper = mapper;
        }

        public async Task<bool> CreateStoryAsync(StoryDTO storyDTO)
        {
            int status=await storiesRepository.AddStoryAsync(storyDTO);
            return status==1 && true;
        }

        public async Task<List<StoryDTO>> GetStoryAsync()
        {
            List<Story> stories=await storiesRepository.GetStoryAsync();
            List<StoryDTO> storyDTOs=stories.Select(story=>mapper.Map<StoryDTO>(story)).ToList();
            return storyDTOs;
        }

        public async Task<StoryDTO> GetStoryAsync(Guid storyId)
        {
            Story story=await storiesRepository.GetStoryAsync(storyId);
            StoryDTO storyDTO=mapper.Map<StoryDTO>(story);
            return storyDTO;
        }

        public async Task<string> ReplaceStoryAsync(StoryDTO storyDTO)
        {
            string status=await storiesRepository.ReplaceStoryAsync(storyDTO);
            return status;
        }

        public async Task<string> RemoveStoryAsync(Guid storyId){
            return await storiesRepository.RemoveStoryAsync(storyId);
        }
    }
}