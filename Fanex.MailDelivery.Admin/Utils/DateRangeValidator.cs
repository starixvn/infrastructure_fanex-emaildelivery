#region

using System;

#endregion

namespace Fanex.MailDelivery.Admin.Utils
{
    public class DateRangeValidator
    {
        public static void ValidateMinute(int minute)
        {
            if (minute < 0 || minute > 59)
            {
                throw new ArgumentException("Invalid minute (must be >= 0 and <= 59).");
            }
        }

        public static void ValidateHour(int hour)
        {
            if (hour < 0 || hour > 23)
            {
                throw new ArgumentException("Invalid hour (must be >= 0 and <= 23).");
            }
        }

        public static void ValidateDayOfMonth(int day)
        {
            if (day < 1 || day > 31)
            {
                throw new ArgumentException("Invalid day of month.");
            }
        }

        public static void ValidateMonth(int month)
        {
            if (month < 1 || month > 12)
            {
                throw new ArgumentException("Invalid month (must be >= 1 and <= 12).");
            }
        }

        public static void ValidateMinDayOfMonth(int day)
        {
            if (day < 1 || day > 28)
            {
                throw new ArgumentException("Date should not be greated than 28");
            }
        }
    }
}