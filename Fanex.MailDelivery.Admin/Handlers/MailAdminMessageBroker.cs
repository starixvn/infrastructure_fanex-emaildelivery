using EasyNetQ;
using Fanex.MailDelivery.Admin.Configurations;
using Fanex.MailDelivery.Admin.Logging;
using Fanex.MailDelivery.Admin.Messaging.Job;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Admin.Messaging.ManageReport;
using Fanex.MailDelivery.Admin.Messaging.Schedule;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Messaging;

namespace Fanex.MailDelivery.Admin.Handlers
{
    /// <summary>
    /// Mail Admin Message Broker.
    /// </summary>
    public class MailAdminMessageBroker : IMailAdminMessageBroker
    {
        private IBus _bus;

        public void Start()
        {
            _bus = RabbitHutch.CreateBus(AppSettings.MailAdminConfig.RabbitMQConnectionString, serviceRegister =>
                 {
                     serviceRegister.Register<IEasyNetQLogger>(_ => new MailAdminLogger());
                 });
        }

        public void Stop()
        {
            _bus.Dispose();
        }

        public void RetryJobMessage(ScheduleRequest schedule)
        {
            string retryJobPublishId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, schedule.SiteId);
            _bus.Publish<ScheduleRequest>(schedule, retryJobPublishId);
        }

        public void PublishScheduleChangeMessage(ScheduleRequest schedule)
        {
            string updateSchedulePublishId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, schedule.SiteId);
            _bus.Publish<ScheduleRequest>(schedule, updateSchedulePublishId);
        }

        public void AbortJob(AbortJobRequest request)
        {
            JobResultMessage jobResultMessage = new JobResultMessage() { JobStatus = Core.Enumerations.JobStatus.Abort, Schedule = new Schedule() { TaskId = request.TaskId } };
            _bus.Publish<JobResultMessage>(jobResultMessage, MailDeliveryConstants.AbortJobRoutingKeyFormat);
        }

        #region Mail

        public GetMailListResponse GetMailList(GetMailListRequest request)
        {
            return _bus.Request<GetMailListRequest, GetMailListResponse>(request);
        }

        public ResultResponse CreateFunctionMail(CreateFunctionMailRequest request)
        {
            return _bus.Request<CreateFunctionMailRequest, ResultResponse>(request);
        }

        public ResultResponse CreateAutoMail(CreateAutoMailRequest request)
        {
            return _bus.Request<CreateAutoMailRequest, ResultResponse>(request);
        }

        public ResultResponse UpdateFunctionMail(UpdateFunctionMailRequest request)
        {
            return _bus.Request<UpdateFunctionMailRequest, ResultResponse>(request);
        }

        public ResultResponse UpdateAutoMail(UpdateAutoMailRequest request)
        {
            return _bus.Request<UpdateAutoMailRequest, ResultResponse>(request);
        }

        public ResultResponse DeleteMail(DeleteMailRequest request)
        {
            return _bus.Request<DeleteMailRequest, ResultResponse>(request);
        }

        #endregion

        #region Recipient

        public GetRecipientListResponse GetRecipientList(GetRecipientListRequest request)
        {
            return _bus.Request<GetRecipientListRequest, GetRecipientListResponse>(request);
        }

        public ResultResponse CreateRecipient(CreateRecipientRequest request)
        {
            return _bus.Request<CreateRecipientRequest, ResultResponse>(request);
        }

        public ResultResponse DeleteRecipient(DeleteRecipientRequest request)
        {
            return _bus.Request<DeleteRecipientRequest, ResultResponse>(request);
        }

        #endregion


        #region Group

        public GetGroupListResponse GetGroupList(GetGroupListRequest request)
        {
            return _bus.Request<GetGroupListRequest, GetGroupListResponse>(request);
        }

        public ResultResponse CreateGroup(CreateGroupRequest request)
        {
            return _bus.Request<CreateGroupRequest, ResultResponse>(request);
        }

        public ResultResponse UpdateGroup(UpdateGroupRequest request)
        {
            return _bus.Request<UpdateGroupRequest, ResultResponse>(request);
        }

        public ResultResponse DeleteGroup(DeleteGroupRequest request)
        {
            return _bus.Request<DeleteGroupRequest, ResultResponse>(request);
        }

        #endregion

        #region Report

        public GetReportListResponse GetReportList(GetReportListRequest request)
        {
            return _bus.Request<GetReportListRequest, GetReportListResponse>(request);
        }

        #endregion
    }
}