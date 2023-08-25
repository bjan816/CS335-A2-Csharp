using System.Text;
using A2.Models;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;

namespace A2.Helper
{
    class CalendarOutputFormatter : TextOutputFormatter
    {
        public CalendarOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/calendar"));
            SupportedEncodings.Add(Encoding.UTF8);
        }

        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            Event e = (Event)context.Object;

            var dtStamp = DateTime.UtcNow;

            var builder = new StringBuilder();

            const string upi = "bjan816";

            builder.AppendLine("BEGIN:VCALENDAR");
            builder.AppendLine("VERSION:2.0");
            builder.AppendLine($"PRODID:{upi}");
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

            var outString = builder.ToString();

            byte[] outBytes = selectedEncoding.GetBytes(outString);
            var response = context.HttpContext.Response.Body;

            return response.WriteAsync(outBytes, 0, outBytes.Length);
        }
    }
}