using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;

namespace Blog_Rest_Api.Services{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(UserRegistrationDTO userRegistrationDTO);
        Task<UserInfoDTO> LoginAsync(UserCredentialsDTO credentialsDTO);
        Task<bool> LogOutAsync();
    }
}