using System.ComponentModel.DataAnnotations;

namespace A2.Models
{
    public class Event
    {
        public Event(string start, string end, string summary, string description, string location)
        {
            Start = start;
            End = end;
            Summary = summary;
            Description = description;
            Location = location;
        }

        [Key] public int Id { get; set; }

        [Required] public string Start { get; set; }

        [Required] public string End { get; set; }

        [Required] public string Summary { get; set; }
        [Required] public string Description { get; set; }
        [Required] public string Location { get; set; }
    }
}