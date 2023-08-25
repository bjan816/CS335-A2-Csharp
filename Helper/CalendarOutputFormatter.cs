﻿using System.Text;
using A2.Models;

namespace A2.Helper
{
    class CalendarOutputFormatter
    {
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
    }
}