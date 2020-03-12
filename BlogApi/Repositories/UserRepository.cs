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
        public UserRepository(BlogContext blogContext)
        {
           this.blogContext=blogContext;
        }


        public async Task<User> GetAsync(string userId)
        {
            User user=await blogContext.Users.AsNoTracking().FirstOrDefaultAsync(user=>user.UserId==userId);
            return user;
        }

        public IQueryable<User> GetAllAsync(int skip, int top)
        {
            return  blogContext.Users
                                    .AsNoTracking()
                                    .Skip(skip)
                                    .Take(top).AsQueryable();
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