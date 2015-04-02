namespace Fanex.MailDelivery.Client.Configurations
{
    using System;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class AppSettings
    {
        static AppSettings()
        {
            MailCientConfig = MailClientSection.GetConfig();
        }

        public static MailClientSection MailCientConfig { get; private set; }

        /// <summary>
        /// Gets the function identifier.
        /// </summary>
        /// <param name="functionName">Name of the function.</param>
        /// <returns>Function ID.</returns>
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

        /// <summary>
        /// Gets the type of the job.
        /// </summary>
        /// <param name="scheduleId">The schedule identifier.</param>
        /// <returns>Job Type return.</returns>
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

        /// <summary>
        /// Determines whether [has schedule identifier] [the specified schedule identifier].
        /// </summary>
        /// <param name="scheduleId">The schedule identifier.</param>
        /// <returns>Has schedule id or not.</returns>
        public static bool HasScheduleId(int scheduleId)
        {
            var schedules = MailCientConfig.Schedules;
            foreach (ScheduleElement schedule in schedules)
            {
                if (schedule.Id == scheduleId)
                {
                    return true;
                }
            }

            return false;
        }
    }
}