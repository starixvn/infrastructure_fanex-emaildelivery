namespace Fanex.MailDelivery.Client.Configurations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSettings
    {
        public static MailClientSection MailCientConfig { private set; get; }

        static AppSettings()
        {
            MailCientConfig = MailClientSection.GetConfig();
        }

        public static int GetFunctionId(string functionName)
        {
            FunctionElementCollection functions = MailCientConfig.Functions;

            foreach (FunctionElement function in functions)
            {
                if (functionName == function.Name)
                {
                    return function.Id;
                }
            }

            return 0;
        }

        public static Type GetJobType(int scheduleId)
        {
            var schedules = MailCientConfig.Schedules;

            foreach (ScheduleElement schedule in schedules)
            {
                if (scheduleId == schedule.Id)
                {
                    return Type.GetType(schedule.Job);
                }
            }

            return null;
        }
    }
}
