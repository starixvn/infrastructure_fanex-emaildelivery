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

            string cronExpressionFormat = "0 {0} {1} {2} * ?";

            string cronExpression = repeatEvery == 1
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

            string dayOfWeekStr = "";
            for (int i = 1; i < daysOfWeek.Length; i++)
            {
                dayOfWeekStr += (int)daysOfWeek[i] + 1 + ",";
            }
            dayOfWeekStr = dayOfWeekStr.Trim(',');

            string cronExpression = string.Format("0 {0} {1} ? * {2}", minute, hour, dayOfWeekStr);

            return cronExpression;
        }

        public static string MonthlyOnDayAndHourAndMinute(int repeatEvery, int[] daysOfMonth, int hour, int minute)
        {
            DateRangeValidator.ValidateHour(hour);
            DateRangeValidator.ValidateMinute(minute);
            DateRangeValidator.ValidateMonth(repeatEvery);

            daysOfMonth.ToList().ForEach(DateRangeValidator.ValidateMinDayOfMonth);

            string monthlyRepeatExpression = repeatEvery == 1 ? "*" : "1/" + repeatEvery;
            string specificDaysExpression = string.Join(",", daysOfMonth);

            string cronExpression = string.Format("0 {0} {1} {2} {3} ?", minute, hour, specificDaysExpression, monthlyRepeatExpression);

            return cronExpression;
        }
    }
}
