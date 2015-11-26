using System.Collections.Generic;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Client.Handlers
{
    /// <summary>
    /// JobRequest Handler interface.
    /// </summary>
    public interface IJobHandler
    {
        void Start();

        void Stop();

        void StartJobs(IList<Schedule> schedules);

        void UpdateSchedule(Schedule schedule);

        void RetryJob(Schedule schedule);

        void RetryAllProcess(Schedule schedule);
    }
}
