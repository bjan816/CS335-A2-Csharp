using A2.Models;

namespace A2.Data
{
    public interface IA2Repo
    {
        Task<bool> IsUserNameRegistered(string userName);
        Task<bool> IsUserRegistered(string userName, string password);
        Task<bool> IsOrganizerNameRegistered(string name);
        Task<bool> IsUserOrganizer(string userName, string password);
        Task<User?> FindUser(string userName);
        Task RegisterUser(User user);
        Task RegisterOrganizer(Organizer organizer);

        Task<Product?> FindProduct(int productId);

        Task AddEvent(Event newEvent);
    }
}