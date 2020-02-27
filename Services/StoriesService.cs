using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Repositories;
using Blog_Rest_Api.Utils;

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

        public async Task<DBStatus> CreateStoryAsync(StoryDTO storyDTO)
        {
            DBStatus status=await storiesRepository.AddStoryAsync(storyDTO);
            return status;
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

        public async Task<DBStatus> ReplaceStoryAsync(StoryDTO storyDTO)
        {
            DBStatus status=await storiesRepository.ReplaceStoryAsync(storyDTO);
            return status;
        }

        public async Task<DBStatus> RemoveStoryAsync(Guid storyId){
            return await storiesRepository.RemoveStoryAsync(storyId);
        }
    }
}