using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Client.Handlers.Interface
{
    using System.Collections.Generic;

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
