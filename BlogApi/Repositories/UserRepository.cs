using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using Blog_Rest_Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace Blog_Rest_Api.Repositories{
    public class UserRepository : IUserRepository
    {
        private readonly BlogContext blogContext;
        private readonly IMapper mapper;

        public UserRepository(BlogContext blogContext,IMapper mapper)
        {
           this.blogContext=blogContext;
            this.mapper = mapper;
        }


        public async Task<UserInfoDTO> GetAsync(string userId)
        {
            User user=await blogContext.Users.AsNoTracking().FirstOrDefaultAsync(user=>user.UserId==userId);
            return mapper.Map<UserInfoDTO>(user);
        }

        public async Task<List<UserInfoDTO>> GetAllAsync<UserInfoDTO>(int skip, int top)
        {
            return await blogContext.Users
                                    .AsNoTracking()
                                    .Skip(skip)
                                    .Take(top)
                                    .Select(user=>mapper.Map<UserInfoDTO>(user))
                                    .ToListAsync();
        }

        public async Task<DBStatus> UpdateUserPasswordAsync(UpdateUserPasswordDTO passwordDTO)
        {
            User user =await blogContext.Users.FirstOrDefaultAsync(user=>user.UserId==passwordDTO.UserId && user.PasswordHash==passwordDTO.OldPassword);
            if(user==null)
                return DBStatus.NotFound;
            user.PasswordHash=passwordDTO.NewPassword;
            int status =await blogContext.SaveChangesAsync();
            return status==0 ? DBStatus.NotModified : DBStatus.Modified;
        }
    }
}