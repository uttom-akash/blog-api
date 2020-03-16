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
            IEnumerable<Story> stories = null;
            int total = 0;
            if (query != null && query.Length > 0)
            {
                var storiesObject = await storiesRepository.SearchAsync(query, skip, top);
                stories = storiesObject.Value;
                total = storiesObject.Key;
            }
            else
            {
                var storiesObject = await storiesRepository.GetAllAsync(skip, top);
                stories = storiesObject.Value;
                total = storiesObject.Key;
            }
            StoriesWithCountDTO storiesWithCountDTO = new StoriesWithCountDTO();
            storiesWithCountDTO.Total = total;
            storiesWithCountDTO.Stories = stories.Select(story => mapper.Map<ResponseStoryDTO>(story));
            return storiesWithCountDTO;
        }

        public async Task<StoriesWithCountDTO> GeUserStoriesAsync(string userId, string query, int skip, int top)
        {
            IEnumerable<Story> stories = null;
            int total = 0;
            if (query != null && query.Length > 0)
            {
                var storiesObject = await storiesRepository.SearchUserStoriesAsync(userId, query, skip, top);
                stories = storiesObject.Value;
                total = storiesObject.Key;
            }
            else
            {
                var storiesObject = await storiesRepository.GetUserStoriesAsync(userId, skip, top);
                stories = storiesObject.Value;
                total = storiesObject.Key;
            }
            StoriesWithCountDTO storiesWithCountDTO = new StoriesWithCountDTO();
            storiesWithCountDTO.Total = total;
            storiesWithCountDTO.Stories = stories.Select(story => mapper.Map<ResponseStoryDTO>(story));
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