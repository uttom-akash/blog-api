using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Utils;

namespace Blog_Rest_Api.Repositories{
    public interface IAuthRepository
    {
        Task<DBStatus> RegisterAsync(User user);
        Task<User> LoginAsync(string userId,string passwordHash);
        Task<bool> LogOutAsync();
    }
}