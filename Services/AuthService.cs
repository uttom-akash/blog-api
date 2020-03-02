using System;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
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
        private readonly IMapper mapper;

        public AuthService(IAuthRepository authRepository,JwtSuit jwtSuit,IMapper mapper)
        {
            this.authRepository = authRepository;
            this.jwtSuit = jwtSuit;
            this.mapper = mapper;
        }

        public async Task<DBStatus> RegisterAsync(UserRegistrationDTO userRegistrationDTO)
        {
            string passwordHash=ConverterSuit.ByteArrayToHex(HashSuit.ComputeSha256(Encoding.UTF8.GetBytes(userRegistrationDTO.Password)));
            User user=mapper.Map<User>(userRegistrationDTO);
            user.PasswordHash=passwordHash;
            DBStatus status=await authRepository.RegisterAsync(user);
            return status;
        }
        public async Task<LoggedInUserDTO> LoginAsync(UserCredentialsDTO credentialsDTO)
        {
            string passwordHash=ConverterSuit.ByteArrayToHex(HashSuit.ComputeSha256(Encoding.UTF8.GetBytes(credentialsDTO.Password)));
            User user=await authRepository.LoginAsync(credentialsDTO.UserId,passwordHash);
            
            if(user==null)
                return null;

            LoggedInUserDTO loggedInUser =mapper.Map<LoggedInUserDTO>(user);
            loggedInUser.JwtToken=jwtSuit.GetToken(user);
            return loggedInUser;
        }

        public Task<bool> LogOutAsync()
        {
            throw new System.NotImplementedException();
        }
    }
}