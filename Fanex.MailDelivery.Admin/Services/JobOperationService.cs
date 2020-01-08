using EasyNetQ;
using Fanex.MailDelivery.Admin.Configurations;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging.Job;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Services.MailService;
using System;

namespace Fanex.MailDelivery.Admin.Services
{
    public interface IJobOperationService : IDisposable
    {
        void PublishScheduleChange(Schedule schedule);

        void RunNow(Schedule schedule);

        void Retry(Schedule schedule);

        void AbortJob(AbortJobRequest request);
    }

    public class JobOperationService : BaseAdminService, IJobOperationService
    {
        private static readonly IBus bus = RabbitHutch.CreateBus(
                AppSettings.MailAdminConfig.RabbitMQConnectionString,
                serviceRegister =>
                {
                    //serviceRegister.Register<IEasyNetQLogger>(_ => new MailAdminLogger());
                });
        private readonly IMailService mailService;

        public JobOperationService() : this(null)
        {
        }

        public JobOperationService(IMailService mailService)
        {
            this.mailService = mailService ?? new MailService();
        }

        public void PublishScheduleChange(Schedule schedule)
        {
            var updateSchedulePublishId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, schedule.SiteId);

            bus.Publish(schedule, updateSchedulePublishId);
        }

        public void RunNow(Schedule schedule)
        {
            var runnowJobPublishId = string.Format(MailDeliveryConstants.RunNowJobRoutingKeyFormat, schedule.SiteId);

            bus.Publish(schedule, runnowJobPublishId);
        }

        public void Retry(Schedule schedule)
        {
            var retryJobPublishId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, schedule.SiteId);

            bus.Publish(schedule, retryJobPublishId);
        }

        public void AbortJob(AbortJobRequest request)
        {
            var jobMessage = new JobMessage
            {
                JobStatus = JobStatus.Abort,
                Schedule = new Schedule { TaskId = request.TaskId }
            };

            mailService.UpdateJobStatus(jobMessage);
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // Free managed resources
                }

                bus?.Dispose();

                disposedValue = true;
            }
        }

        ~JobOperationService()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}