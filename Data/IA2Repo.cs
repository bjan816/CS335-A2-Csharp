using A2.Models;

namespace A2.Data
{
    public interface IA2Repo
    {
        Task<bool> UserExists(string userName);
        Task<User?> FindUser(string userName);
        Task RegisterUser(User user);

        Task<Product?> FindProduct(int productId);
    }
}