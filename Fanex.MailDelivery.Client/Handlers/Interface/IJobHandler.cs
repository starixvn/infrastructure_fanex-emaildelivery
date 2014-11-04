namespace Fanex.MailDelivery.Client.Handlers.Interface
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// Job Handler interface.
    /// </summary>
    public interface IJobHandler
    {
        void Start();

        void Stop();

        void StartJobs(List<Schedule> schedules);

        void UpdateSchedule(Schedule schedule);

        void RetryJob(Schedule schedule);

        void RetryAllProcess(Schedule schedule);
    }
}
