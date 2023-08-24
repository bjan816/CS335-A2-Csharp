using System.Globalization;
using System.Text;
using A2.Data;
using A2.Dtos;
using A2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace A2.Controllers
{
    [ApiController]
    [Route("webapi")]
    public class A2Controller : Controller
    {
        private readonly IA2Repo _repository;

        public A2Controller(IA2Repo repository)
        {
            _repository = repository;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] User user)
        {
            if (await _repository.IsUserNameRegistered(user.UserName))
            {
                return Ok($"UserName {user.UserName} is not available.");
            }

            await _repository.RegisterUser(user);

            return Ok("User successfully registered.");
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "BasicAuthentication", Policy = "IsRegisteredUser")]
        [Route("PurchaseItem")]
        public async Task<IActionResult> PurchaseItem(int productId)
        {
            var product = await _repository.FindProduct(productId);

            if (product == null)
            {
                return BadRequest($"Product {productId} not found");
            }

            var purchaseOutput = new PurchaseOutput(User.Identity.Name, productId);

            return Ok(purchaseOutput);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = "BasicAuthentication", Policy = "IsOrganizer")]
        [Route("AddEvent")]
        public async Task<IActionResult> AddEvent([FromBody] EventInput eventInput)
        {
            const string format = "yyyyMMddTHHmmssZ";

            bool startMatchesFormat = DateTime.TryParseExact(eventInput.Start, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out _);
            bool endMatchesFormat = DateTime.TryParseExact(eventInput.End, format, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out _);

            if (!startMatchesFormat && !endMatchesFormat)
            {
                return BadRequest($"The format of Start and End should be {format}.");
            }

            if (!startMatchesFormat)
            {
                return BadRequest($"The format of Start should be {format}.");
            }

            if (!endMatchesFormat)
            {
                return BadRequest($"The format of End should be {format}.");
            }

            var newEvent = new Event
            (
                eventInput.Start,
                eventInput.End,
                eventInput.Summary,
                eventInput.Description,
                eventInput.Location
            );

            await _repository.AddEvent(newEvent);

            return Ok("Success.");
        }

        [HttpGet("EventCount")]
        [Authorize(Policy = "HasAuthority")]
        public async Task<IActionResult> GetEventCount()
        {
            int count = await _repository.GetEventCount();
            return Ok(count);
        }

        public static string ConvertEventToICalendar(Event e)
        {
            var dtStamp = DateTime.UtcNow;

            var builder = new StringBuilder();

            const string uid = "bjan816";

            builder.AppendLine("BEGIN:VCALENDAR");
            builder.AppendLine($"PRODID:-//{uid}");
            builder.AppendLine("VERSION:2.0");
            builder.AppendLine("BEGIN:VEVENT");

            builder.AppendLine($"UID:{e.Id}");
            builder.AppendLine($"DTSTAMP:{dtStamp:yyyyMMddTHHmmssZ}");
            builder.AppendLine($"DTSTART:{e.Start:yyyyMMddTHHmmssZ}");
            builder.AppendLine($"DTEND:{e.End:yyyyMMddTHHmmssZ}");
            builder.AppendLine($"SUMMARY:{e.Summary}");
            builder.AppendLine($"DESCRIPTION:{e.Description}");
            builder.AppendLine($"LOCATION:{e.Location}");

            builder.AppendLine("END:VEVENT");
            builder.AppendLine("END:VCALENDAR");

            return builder.ToString();
        }

        [Authorize(Policy = "HasAuthority")]
        [HttpGet("Event/{id}")]
        public async Task<IActionResult> Event(int id)
        {
            var e = await _repository.GetEvent(id);
            if (e == null)
            {
                return BadRequest($"Event {id} does not exist.");
            }

            var iCalendarContent = ConvertEventToICalendar(e);
            return Ok(iCalendarContent);
        }
    }
}