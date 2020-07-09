using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Utils;

namespace Blog_Rest_Api.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetAsync(string userId);
        IEnumerable<User> GetAllAsync(int skip, int top);
        Task<DBStatus> UpdateUserPasswordAsync(UpdateUserPasswordDTO passwordDTO);
    }
}