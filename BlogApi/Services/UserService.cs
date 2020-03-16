using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Rest_Api.Crypto;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Repositories;
using Blog_Rest_Api.Utils;

namespace Blog_Rest_Api.Services
{
    class UserService : IUserService
    {
        private readonly IUserRepository userRepository;
        private readonly IMapper mapper;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            this.userRepository = userRepository;
            this.mapper = mapper;
        }

        public async Task<UserInfoDTO> GetUserAsync(string userId)
        {
            User user = await userRepository.GetAsync(userId);
            return mapper.Map<UserInfoDTO>(user);
        }

        public async Task<IEnumerable<UserInfoDTO>> GetUsersAsync(int skip, int top)
        {
            IEnumerable<User> users = userRepository.GetAllAsync(skip, top);
            return users.Select(user => mapper.Map<UserInfoDTO>(user));
        }

        public Task<DBStatus> UpdateUserPasswordAsync(UpdateUserPasswordDTO passwordDTO)
        {
            passwordDTO.NewPassword = ConverterSuit.ByteArrayToHex(HashSuit.ComputeSha256(Encoding.UTF8.GetBytes(passwordDTO.NewPassword)));
            passwordDTO.OldPassword = ConverterSuit.ByteArrayToHex(HashSuit.ComputeSha256(Encoding.UTF8.GetBytes(passwordDTO.OldPassword)));
            return userRepository.UpdateUserPasswordAsync(passwordDTO);
        }
    }
}