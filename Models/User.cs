using System.ComponentModel.DataAnnotations;

namespace A2.Models
{
    public class User
    {
        public User(string userName, string password, string address)
        {
            UserName = userName;
            Password = password;
            Address = address;
        }

        [Key] [Required] public string UserName { get; set; }

        // Todo: Hash and salt
        [Required] public string Password { get; set; }
        [Required] public string Address { get; set; }
    }
}