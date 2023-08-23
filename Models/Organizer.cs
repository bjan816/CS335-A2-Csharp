using System.ComponentModel.DataAnnotations;

namespace A2.Models
{
    public class Organizer
    {
        public Organizer(string name, string password)
        {
            Name = name;
            Password = password;
        }

        [Key] [Required] public string Name { get; set; }

        // Todo: Hash and salt
        [Required] public string Password { get; set; }
    }
}