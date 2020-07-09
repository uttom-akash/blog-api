using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Repositories;
using Blog_Rest_Api.Utils;

namespace Blog_Rest_Api.Services
{
    public class StoriesService : IStoriesService
    {
        private readonly IStoriesRepository storiesRepository;
        private readonly IMapper mapper;

        public StoriesService(IStoriesRepository storiesRepository, IMapper mapper)
        {
            this.storiesRepository = storiesRepository;
            this.mapper = mapper;
        }

        public async Task<DBStatus> CreateStoryAsync(RequestStoryDTO storyDTO, string userId)
        {
            Story story = mapper.Map<Story>(storyDTO);
            story.AuthorId = userId;
            DBStatus status = await storiesRepository.AddStoryAsync(story);
            return status;
        }

        public async Task<StoriesWithCountDTO> GetStoriesAsync(string query, int skip, int top)
        {
            KeyValuePair<int, IEnumerable<Story>> storiesAndCount;
            if (query != null && query.Length > 0)
            {
                storiesAndCount = await storiesRepository.SearchAsync(query, skip, top);
            }
            else
            {
                storiesAndCount = await storiesRepository.GetAllAsync(skip, top);
            }

            StoriesWithCountDTO storiesWithCountDTO = new StoriesWithCountDTO();
            storiesWithCountDTO.Total = storiesAndCount.Key;
            storiesWithCountDTO.Stories = storiesAndCount.Value.Select(story => mapper.Map<ResponseStoryDTO>(story));
            return storiesWithCountDTO;
        }

        public async Task<StoriesWithCountDTO> GeUserStoriesAsync(string userId, string query, int skip, int top)
        {
            KeyValuePair<int, IEnumerable<Story>> storiesAndCount;
            if (query != null && query.Length > 0)
            {
                storiesAndCount = await storiesRepository.SearchUserStoriesAsync(userId, query, skip, top);
            }
            else
            {
                storiesAndCount = await storiesRepository.GetUserStoriesAsync(userId, skip, top);
            }
            StoriesWithCountDTO storiesWithCountDTO = new StoriesWithCountDTO();
            storiesWithCountDTO.Total = storiesAndCount.Key;
            storiesWithCountDTO.Stories = storiesAndCount.Value.Select(story => mapper.Map<ResponseStoryDTO>(story));
            return storiesWithCountDTO;
        }

        public async Task<ResponseStoryDTO> GetStoryAsync(Guid storyId)
        {
            ResponseStoryDTO story = mapper.Map<ResponseStoryDTO>(await storiesRepository.GetAsync(storyId));
            return story;
        }

        public async Task<DBStatus> ReplaceStoryAsync(RequestStoryDTO storyDTO, string userId)
        {
            DBStatus status = await storiesRepository.ReplaceStoryAsync(storyDTO, userId);
            return status;
        }

        public async Task<DBStatus> RemoveStoryAsync(Guid storyId, string userId)
        {
            return await storiesRepository.RemoveStoryAsync(storyId, userId);
        }


    }
}