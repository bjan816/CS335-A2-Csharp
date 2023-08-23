using System.ComponentModel.DataAnnotations;

namespace A2.Models
{
    public class Product
    {
        public Product(int id, string name, float price, string description)
        {
            Id = id;
            Name = name;
            Price = price;
            Description = description;
        }

        [Key] public int Id { get; set; }
        [Required] public string Name { get; set; }
        [Required] public float Price { get; set; }
        
        [Required] public string Description { get; set; }
    }
}