using System;
using System.Linq;

namespace Fanex.MailDelivery.Admin.Utils
{
    public class CronExpresionBuilder
    {
        public static string DailyAtHourAndMinute(int repeatEvery, int hour, int minute)
        {
            DateRangeValidator.ValidateHour(hour);
            DateRangeValidator.ValidateMinute(minute);
            DateRangeValidator.ValidateDayOfMonth(repeatEvery);

            var cronExpressionFormat = "0 {0} {1} {2} * ?";

            var cronExpression = repeatEvery == 1
                ? string.Format(cronExpressionFormat, minute, hour, "*")
                : string.Format(cronExpressionFormat, minute, hour, "1/" + repeatEvery);

            return cronExpression;
        }

        public static string WeeklyOnDayAndHourAndMinute(DayOfWeek[] daysOfWeek, int hour, int minute)
        {
            if (daysOfWeek == null || daysOfWeek.Length == 0)
            {
                throw new ArgumentException("You must specify at least one day of week.");
            }

            DateRangeValidator.ValidateHour(hour);
            DateRangeValidator.ValidateMinute(minute);

            var dayOfWeekStr = string.Empty;
            for (var i = 1; i < daysOfWeek.Length; i++)
            {
                dayOfWeekStr += (int)daysOfWeek[i] + 1 + ",";
            }

            dayOfWeekStr = dayOfWeekStr.Trim(',');

            var cronExpression = string.Format("0 {0} {1} ? * {2}", minute, hour, dayOfWeekStr);

            return cronExpression;
        }

        public static string MonthlyOnDayAndHourAndMinute(int repeatEvery, int[] daysOfMonth, int hour, int minute)
        {
            DateRangeValidator.ValidateHour(hour);
            DateRangeValidator.ValidateMinute(minute);
            DateRangeValidator.ValidateMonth(repeatEvery);

            daysOfMonth.ToList().ForEach(DateRangeValidator.ValidateMinDayOfMonth);

            var monthlyRepeatExpression = repeatEvery == 1 ? "*" : "1/" + repeatEvery;
            var specificDaysExpression = string.Join(",", daysOfMonth);

            var cronExpression = string.Format("0 {0} {1} {2} {3} ?", minute, hour, specificDaysExpression, monthlyRepeatExpression);

            return cronExpression;
        }
    }
}