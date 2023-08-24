using System.ComponentModel.DataAnnotations;

namespace A2.Dtos
{
    public class EventInput
    {
        public EventInput(string start, string end, string summary, string description, string location)
        {
            Start = start;
            End = end;
            Summary = summary;
            Description = description;
            Location = location;
        }

        [Required] public string Start { get; set; }

        [Required] public string End { get; set; }

        [Required] public string Summary { get; set; }

        [Required] public string Description { get; set; }

        [Required] public string Location { get; set; }
    }
}