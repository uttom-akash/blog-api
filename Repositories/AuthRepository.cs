using System.Threading.Tasks;
using Blog_Rest_Api.DTOModels;
using Blog_Rest_Api.Persistent_Model;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Blog_Rest_Api.Repositories{
    public class AuthRepository : IAuthRepository
    {
        private BlogContext blogContext;
        public AuthRepository(BlogContext blogContext)
        {
           this.blogContext=blogContext;
        }

        public async Task<int> RegisterAsync(User user)
        {
            await blogContext.Users.AddAsync(user);
            var resultStatus=await blogContext.SaveChangesAsync();
            return resultStatus;
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