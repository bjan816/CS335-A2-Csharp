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

        public async Task<bool> IsUserNameRegistered(string userName)
        {
            return await _dbContext.Users.AnyAsync(u => u.UserName == userName);
        }

        public async Task<bool> IsUserRegistered(string userName, string password)
        {
            var user = await _dbContext.Users.FirstOrDefaultAsync(user => user.UserName == userName && user.Password == password);
            return user != null;
        }

        public async Task<bool> IsOrganizerNameRegistered(string userName)
        {
            return await _dbContext.Organizers.AnyAsync(u => u.Name == userName);
        }

        public async Task<bool> IsUserOrganizer(string userName, string password)
        {
            var organizer = await _dbContext.Organizers.FirstOrDefaultAsync(organizer => organizer.Name == userName && organizer.Password == password);
            return organizer != null;
        }

        public async Task<int> GetEventCount()
        {
            return await _dbContext.Events.CountAsync();
        }

        public async Task<User?> FindUser(string userName)
        {
            return await _dbContext.Users.FirstOrDefaultAsync(u => u.UserName == userName);
        }

        public async Task RegisterUser(User user)
        {
            _dbContext.Users.Add(user);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Product?> FindProduct(int productId)
        {
            return await _dbContext.Products.FirstOrDefaultAsync(p => p.Id == productId);
        }

        public async Task AddEvent(Event newEvent)
        {
            _dbContext.Events.Add(newEvent);
            await _dbContext.SaveChangesAsync();
        }
    }
}