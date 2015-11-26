using EasyNetQ;
using Fanex.MailDelivery.Admin.Configurations;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Logging;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Messaging;
using Fanex.MailDelivery.Services.Messaging.MailManage;

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

        public void PublishRetryJobMessage(Schedule schedule)
        {
            string retryJobPublishId = string.Format(MailDeliveryConstants.RetryJobRoutingKeyFormat, schedule.SiteId);
            _bus.Publish<Schedule>(schedule, retryJobPublishId);
        }

        public void PublishScheduleChangeMessage(Schedule schedule)
        {
            string updateSchedulePublishId = string.Format(MailDeliveryConstants.UpdateScheduleRoutingKeyFormat, schedule.SiteId);
            _bus.Publish<Schedule>(schedule, updateSchedulePublishId);
        }

        public void PublishAbortJobMessage(int taskID)
        {
            JobResultMessage jobResultMessage = new JobResultMessage() { JobStatus = Core.Enumerations.JobStatus.Abort, Schedule = new Schedule() { TaskId = taskID } };
            _bus.Publish<JobResultMessage>(jobResultMessage, MailDeliveryConstants.AbortJobRoutingKeyFormat);
        }

        public GetMailListResponse RequestGetMailMessage(GetMailListRequest getMailListRequest)
        {
            return _bus.Request<GetMailListRequest, GetMailListResponse>(getMailListRequest);
        }

        public ResultMessage CreateFunctionMail(CreateFunctionMailRequest createFunctionMailRequest)
        {
            throw new System.NotImplementedException();
        }

        public ResultMessage CreateAutoMailMessage(CreateAutoMailRequest createAutoMailRequest)
        {
            throw new System.NotImplementedException();
        }

        //public ResultMessage RequestCreateMailMessage(CreateMailMessage message)
        //{
        //    return _bus.Request<CreateMailMessage, ResultMessage>(message);
        //}

        //public ResultMessage RequestUpdateMailMessage(UpdateMailMessage message)
        //{
        //    return _bus.Request<UpdateMailMessage, ResultMessage>(message);
        //}

        //public List<RecipientModel> RequestGetRecipientMessage(string email)
        //{
        //    RecipientListMessage message = new RecipientListMessage();
        //    message.Emails = email;
        //    return _bus.Request<RecipientListMessage, List<RecipientModel>>(message);
        //}

        //public ResultMessage RequestCreateRecipientMessage(CreateRecipientMessage message)
        //{
        //    return _bus.Request<CreateRecipientMessage, ResultMessage>(message);
        //}

        //public ResultMessage RequestUpdateRecipientMessage(UpdateRecipientMessage message)
        //{
        //    return _bus.Request<UpdateRecipientMessage, ResultMessage>(message);
        //}

        //public RecipientModel RequestGetRecipientByIdMessage(GetRecipientByIdMessage message)
        //{
        //    return _bus.Request<GetRecipientByIdMessage, RecipientModel>(message);
        //}
    }
}