namespace Fanex.MailDelivery.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAP.Framework.Command;
    using Fanex.MailDelivery.Client.Enumerations;

    /// <summary>
    /// Mail Service.
    /// </summary>
    public class MailService : ServiceBase, IMailService
    {
        /// <summary>
        /// Data stimulate, should get data from database here.
        /// </summary>
        private static List<Schedule> _schedules = new List<Schedule> { 
            new Schedule
            {
                Id = 1,
                SiteId = 1,
                FunctionId = 1,
                Name = "Trigger1",
                //RepeatExpression = "0 0/3 * * * ?",
                RepeatExpression = "0/30 * * * * ?",
                StartTime = DateTime.Now,
                EndTime = DateTime.Now.AddDays(30),
                RetryIntervalInMinutes = 5,
                MaxRetryTimes = 2,
                Active = true
            },
            new Schedule
            {
                Id = 2,
                SiteId = 2,
                FunctionId = 2,
                Name = "Trigger2",
                RepeatExpression = "0/30 * * * * ?",
                StartTime = DateTime.Now,
                RetryIntervalInMinutes = 3,
                MaxRetryTimes = 4,
                Active = true
            },
            new Schedule
            {
                Id = 3,
                SiteId = 3,
                FunctionId = 3,
                Name = "Trigger3",
                RepeatExpression = "0 0/1 * * * ?",
                StartTime = DateTime.Now,
                RetryIntervalInMinutes = 4,
                MaxRetryTimes = 5,
                Active = true
            }
        };

        public List<Schedule> GetSchedules(List<int> scheduleIds)
        {
            List<Schedule> schedules = new List<Schedule>();

            try
            {
                foreach (int scheduleId in scheduleIds)
                {
                    schedules.Add(_schedules.Where(sc => sc.Id == scheduleId).FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return schedules;
        }

        public CommandResult UpdateJobStatus(Schedule schedule, JobStatus jobStatus)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                switch (jobStatus)
                {
                    case JobStatus.Success:
                        var jobLog = string.Format(
                                           "Job Completed: \n\tSite Id: {0}, Function Id: {1}, Schedule: {2}. \n\tSent at {3}",
                                           schedule.SiteId,
                                           schedule.FunctionId,
                                           schedule.Id,
                                           DateTime.Now);
                        
                        Console.WriteLine(jobLog);
                        
                        break;
                    case JobStatus.Running:
                    case JobStatus.DataIsNoReady:
                        var jobRetryLog = string.Format(
                                           "Job Retry: \n\tSite Id: {0}, Function Id: {1}, Schedule: {2}. \n\tRetrytimes: {3}.\n\tMaxRetrytimes: {4}.",
                                           schedule.SiteId,
                                           schedule.FunctionId,
                                           schedule.Id,
                                           schedule.CurrentRetryTimes,
                                           schedule.MaxRetryTimes);
                        Console.WriteLine(jobRetryLog);
                        if (!schedule.IsRunning())
                        {
                            Console.WriteLine("Data is not ready, completed running!");
                        }
                        break;
                    case JobStatus.Failed:
                        break;
                }
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return null;
        }

        public CommandResult SendMail(Mail mail)
        {
            try
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine(string.Format("Sent {0}", mail.Subject));
                Console.ForegroundColor = ConsoleColor.White;
            }
            catch (Exception ex)
            {
                LogError(ex);
            }

            return null;
        }
    }
}
