using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.MailDelivery.MailStation.Constants;

namespace Fanex.MailDelivery.MailStation.Extensions
{
    public static class TimeZoneExtension
    {
        public static string GetTimeZoneString()
        {
            TimeZoneInfo.ClearCachedData();
            string standardName = TimeZone.CurrentTimeZone.StandardName;
            var timezone = TimeZoneConstants.Timezones.Where(timeZone => timeZone.StandardName.Equals(standardName)).FirstOrDefault();
            return timezone == null ? null : timezone.TimeZoneString;
        }
    }
}
