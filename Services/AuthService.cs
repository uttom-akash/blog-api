using System;
using System.Text;
using System.Threading.Tasks;
using Blog_Rest_Api.Crypto;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Jwt;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Repositories;
using Blog_Rest_Api.Utils;

namespace Blog_Rest_Api.Services{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository authRepository;
        private readonly JwtSuit jwtSuit;

        public AuthService(IAuthRepository authRepository,JwtSuit jwtSuit)
        {
            this.authRepository = authRepository;
            this.jwtSuit = jwtSuit;
        }

        public async Task<DBStatus> RegisterAsync(UserRegistrationDTO userRegistrationDTO)
        {
            string passwordHash=ConverterSuit.ByteArrayToHex(HashSuit.ComputeSha256(Encoding.UTF8.GetBytes(userRegistrationDTO.Password)));
            User user=new User{
                UserId=userRegistrationDTO.UserId,
                FirstName=userRegistrationDTO.FirstName,
                LastName=userRegistrationDTO.LastName,
                PasswordHash=passwordHash
            };

            DBStatus status=await authRepository.RegisterAsync(user);
            return status;
        }
        public async Task<UserInfoDTO> LoginAsync(UserCredentialsDTO credentialsDTO)
        {
            string passwordHash=ConverterSuit.ByteArrayToHex(HashSuit.ComputeSha256(Encoding.UTF8.GetBytes(credentialsDTO.Password)));
            User user=await authRepository.LoginAsync(credentialsDTO.UserId,passwordHash);
            
            if(user==null)
                return null;

            UserInfoDTO userInfo=new UserInfoDTO{
                UserId=user.UserId,
                FirstName=user.FirstName,
                LastName=user.LastName,
                JwtToken=jwtSuit.GetToken(user)
            };
            return userInfo;
        }

        public Task<bool> LogOutAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}