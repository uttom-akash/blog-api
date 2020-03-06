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
    public class StoriesService:IStoriesService
    {
        private readonly IStoriesRepository storiesRepository;
        private readonly IMapper mapper;

        public StoriesService(IStoriesRepository storiesRepository,IMapper mapper)
        {
            this.storiesRepository = storiesRepository;
            this.mapper = mapper;
        }

        public async Task<DBStatus> CreateStoryAsync(RequestStoryDTO storyDTO,string userId)
        {
            DBStatus status=await storiesRepository.AddStoryAsync(storyDTO,userId);
            return status;
        }

        public async Task<List<ResponseStoryDTO>> GetStoriesAsync(string query,int skip,int top)
        {
            List<ResponseStoryDTO> stories=new List<ResponseStoryDTO>();
            if(query.Length>0)
                stories=await storiesRepository.SearchAsync(query,skip,top);
            else
                stories=await storiesRepository.GetAllAsync<ResponseStoryDTO>(skip,top);
                
            return stories;
        }

        public async Task<ResponseStoryDTO> GetStoryAsync(Guid storyId)
        {
            ResponseStoryDTO story=mapper.Map<ResponseStoryDTO>(await storiesRepository.GetAsync(storyId));
            return story;
        }

        public async Task<DBStatus> ReplaceStoryAsync(RequestStoryDTO storyDTO,string userId)
        {
            DBStatus status=await storiesRepository.ReplaceStoryAsync(storyDTO,userId);
            return status;
        }

        public async Task<DBStatus> RemoveStoryAsync(Guid storyId,string userId){
            return await storiesRepository.RemoveStoryAsync(storyId,userId);
        }
    }
}