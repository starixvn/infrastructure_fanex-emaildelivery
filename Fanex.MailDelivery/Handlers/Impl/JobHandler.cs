namespace Fanex.MailDelivery.Handlers.Impl
{
    using Fanex.MailDelivery.Configurations;
    using Quartz;
    using Quartz.Impl;

    /// <summary>
    /// Job Handler.
    /// </summary>
    public class JobHandler
    {
        private IScheduler _scheduler;

        public void Start()
        {
            if (AppSettings.MailServiceSection.IsRunAutoMailStatusReport)
            {
                ISchedulerFactory schedFact = new StdSchedulerFactory();
                _scheduler = schedFact.GetScheduler();
                _scheduler.Start();
            }
        }

        public void Stop()
        {
            if (AppSettings.MailServiceSection.IsRunAutoMailStatusReport
                && _scheduler != null)
            {
                _scheduler.Shutdown();
            }
        }
    }
}