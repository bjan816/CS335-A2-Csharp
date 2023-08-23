using A2.Models;
using Microsoft.EntityFrameworkCore;

namespace A2.Data
{
    public class A2Repo : IA2Repo
    {
        private readonly A2DbContext _dbContext;

        public A2Repo(A2DbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> FindUser(string userName)
        {
            return await _dbContext.Users.AnyAsync(u => u.UserName == userName);
        }

        public async Task RegisterUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }
    }
}