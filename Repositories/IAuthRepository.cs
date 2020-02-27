using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;

namespace Blog_Rest_Api.Repositories{
    public interface IAuthRepository
    {
        Task<int> RegisterAsync(User user);
        Task<User> LoginAsync(string userId,string passwordHash);
        Task<bool> LogOutAsync();
    }
}