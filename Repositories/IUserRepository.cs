using System.Collections.Generic;
using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Utils;

namespace Blog_Rest_Api.Repositories{
    public interface IUserRepository
    {
        
        Task<List<UserInfoDTO>> GetUsersAsync(int skip,int top);
        Task<UserInfoDTO> GetUserAsync(string userId);
        Task<DBStatus> UpdateUserPasswordAsync(UpdateUserPasswordDTO passwordDTO);
    }
}