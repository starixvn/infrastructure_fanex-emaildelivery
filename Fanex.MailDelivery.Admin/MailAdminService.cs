using EasyNetQ;
using Fanex.MailDelivery.Admin.Configurations;
using Fanex.MailDelivery.Admin.Handlers;
using Fanex.MailDelivery.Core.Logging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Messaging.MailManage;

namespace Fanex.MailDelivery.Admin
{
    /// <summary>
    /// Mail Admin Handler.
    /// </summary>
    public static class MailAdminService
    {
        private static IBus _bus;

        private static IMailAdminMessageBroker _messageBroker;

        internal static IBus Bus
        {
            get
            {
                if (_bus == null)
                {
                    lock (_bus)
                    {
                        if (_bus == null)
                        {
                            _bus = RabbitHutch.CreateBus(AppSettings.MailAdminConfig.RabbitMQConnectionString, serviceRegister =>
                                {
                                    serviceRegister.Register<IEasyNetQLogger>(_ => new MailAdminLogger());
                                });
                        }
                    }
                }

                return _bus;
            }
        }

        public static void Start(IMailAdminMessageBroker messageBroker = null)
        {
            _messageBroker = messageBroker ?? new MailAdminMessageBroker();
            _messageBroker.Start();
        }

        public static void Stop()
        {
            _messageBroker.Stop();
        }

        public static void PublishRetryJobMessage(Schedule schedule)
        {
            _messageBroker.PublishRetryJobMessage(schedule);
        }

        public static void PublishScheduleChangeMessage(Schedule schedule)
        {
            _messageBroker.PublishScheduleChangeMessage(schedule);
        }

        public static void PublishAbortJobMessage(int taskID)
        {
            _messageBroker.PublishAbortJobMessage(taskID);
        }


        public static GetMailListResponse GetMailList(GetMailListRequest getMailListRequest)
        {
            return _messageBroker.RequestGetMailMessage(getMailListRequest);
        }

        //public static ResultMessage CreateMailMessage(CreateMailMessage message)
        //{
        //    return _messageBroker.RequestCreateMailMessage(message);
        //}

        //public static ResultMessage RequestUpdateMailMessage(UpdateMailMessage message)
        //{
        //    return _messageBroker.RequestUpdateMailMessage(message);
        //}


        //public static List<RecipientModel> RequestGetRecipientMessage(string email)
        //{
        //    return _messageBroker.RequestGetRecipientMessage(email);
        //}

        //public static ResultMessage RequestCreateRecipientMessage(CreateRecipientMessage message)
        //{
        //    return _messageBroker.RequestCreateRecipientMessage(message);
        //}

        //public static ResultMessage RequestUpdateRecipientMessage(UpdateRecipientMessage message)
        //{
        //    return _messageBroker.RequestUpdateRecipientMessage(message);
        //}

        //public static RecipientModel RequestGetRecipientByIdMessage(GetRecipientByIdMessage message)
        //{
        //    return _messageBroker.RequestGetRecipientByIdMessage(message);
        //}
    }
}