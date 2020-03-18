using Fanex.Data.Configuration;
using Fanex.MailDelivery.MailStationService.Jobs;
using Fanex.MailDelivery.MailStationService.MessageBrokers;
using Fanex.MailDelivery.Services.Services;
using Quartz;
using Quartz.Impl;
using System.ServiceProcess;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.MailStationService
{
    partial class MainService : ServiceBase
    {
        private MessageBroker messageBroker;
        private readonly LogService logService;
        private IScheduler scheduler;

        public MainService()
            : this(null, null)
        {
        }

        public MainService(MessageBroker messageBroker, LogService logService)
        {
            InitializeComponent();

            this.logService = logService ?? new LogServiceImpl();
            this.messageBroker = messageBroker ?? new MessageBrokerImpl(this.logService);
            DbSettingProviderManager.Current.Start();
        }

        protected override void OnStart(string[] args)
        {
            messageBroker.Start();

            RegisterJobs()
                .GetAwaiter()
                .GetResult();
        }

        protected override void OnStop()
        {
            messageBroker?.Dispose();
            scheduler?.Shutdown();
        }

        private async Task RegisterJobs()
        {
            StdSchedulerFactory factory = new StdSchedulerFactory();
            scheduler = await factory.GetScheduler();
            await scheduler.Shutdown();

            scheduler = await factory.GetScheduler();
            await scheduler.Start();

            IJobDetail job = JobBuilder.Create<UpdateFailedForLongRunningTaskJob>()
                .WithIdentity("UpdateFailedForLongRunningTaskJob", "MailStation")
                .Build();

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("UpdateFailedForLongRunningTaskJobTrigger", "MailStation")
                .WithSimpleSchedule(x => x.WithIntervalInMinutes(AppSettings.UpdateFailedForLongRunningTaskIntervalInMinutes).RepeatForever())
                .Build();

            await scheduler.ScheduleJob(job, trigger);
        }
    }
}