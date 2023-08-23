using A2.Models;

namespace A2.Data
{
    public interface IA2Repo
    {
        Task<bool> FindUser(string userName);
        Task RegisterUser(User user);
    }
}