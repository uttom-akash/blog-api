using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Blog_Rest_Api.Utils;

namespace Blog_Rest_Api.Repositories{
    public class AuthRepository : IAuthRepository
    {
        private BlogContext blogContext;
        public AuthRepository(BlogContext blogContext)
        {
           this.blogContext=blogContext;
        }

        public async Task<DBStatus> RegisterAsync(User user)
        {
            bool userIdTaken=await blogContext.Users.AnyAsync(x=>x.UserId==user.UserId);
            if(userIdTaken)
                return DBStatus.Taken;

            await blogContext.Users.AddAsync(user);
            var resultStatus=await blogContext.SaveChangesAsync();
            return resultStatus==1 ? DBStatus.Added : DBStatus.Failed;
        }
        public async Task<User> LoginAsync(string userId,string passwordHash)
        {
            User user=await blogContext.Users.AsNoTracking().FirstOrDefaultAsync(user=>user.UserId==userId && user.PasswordHash==passwordHash);
            return user;
        }

        public Task<bool> LogOutAsync()
        {
            throw new System.NotImplementedException();
        }

        
    }
}