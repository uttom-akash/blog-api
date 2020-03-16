using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Utils;

namespace Blog_Rest_Api.Repositories{
    public interface IStoriesRepository
    {
        Task<DBStatus> AddStoryAsync(Story story);
        Task<KeyValuePair<int,IQueryable<Story>>> SearchAsync(string content,int skip, int top);
        Task<KeyValuePair<int,IQueryable<Story>>> SearchUserStoriesAsync(string userId,string content,int skip, int top);
        Task<KeyValuePair<int,IQueryable<Story>>> GetAllAsync(int skip,int top);
        Task<KeyValuePair<int,IQueryable<Story>>> GetUserStoriesAsync(string userId,int skip, int top);
        
        Task<Story> GetAsync(Guid id);
        Task<DBStatus> ReplaceStoryAsync(RequestStoryDTO story,string userId);
        Task<DBStatus> RemoveStoryAsync(Guid storyId,string userId);
    }
}