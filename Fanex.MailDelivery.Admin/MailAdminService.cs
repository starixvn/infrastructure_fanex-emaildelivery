using EasyNetQ;
using Fanex.MailDelivery.Admin.Configurations;
using Fanex.MailDelivery.Admin.Handlers;
using Fanex.MailDelivery.Admin.Logging;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.Job;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Admin.Messaging.ManageReport;
using Fanex.MailDelivery.Admin.Messaging.Schedule;

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

        public static void PublishRetryJobMessage(ScheduleRequest schedule)
        {
            _messageBroker.RetryJobMessage(schedule);
        }

        public static void PublishScheduleChangeMessage(ScheduleRequest schedule)
        {
            _messageBroker.PublishScheduleChangeMessage(schedule);
        }

        public static void AbortJob(AbortJobRequest request)
        {
            _messageBroker.AbortJob(request);
        }


        public static GetMailListResponse GetMailList(GetMailListRequest getMailListRequest)
        {
            return _messageBroker.GetMailList(getMailListRequest);
        }

        public static ResultResponse CreateFunctionMail(CreateFunctionMailRequest createFunctionMailRequest)
        {
            return _messageBroker.CreateFunctionMail(createFunctionMailRequest);
        }

        public static ResultResponse CreateAutoMail(CreateAutoMailRequest createAutoMailRequest)
        {
            return _messageBroker.CreateAutoMail(createAutoMailRequest);
        }

        public static ResultResponse UpdateFunctionMail(UpdateFunctionMailRequest updateFunctionMailRequest)
        {
            return _messageBroker.UpdateFunctionMail(updateFunctionMailRequest);
        }

        public static ResultResponse UpdateAutoMail(UpdateAutoMailRequest updateAutoMailRequest)
        {
            return _messageBroker.UpdateAutoMail(updateAutoMailRequest);
        }

        public static ResultResponse DeleteMail(DeleteMailRequest deleteMailRequest)
        {
            return _messageBroker.DeleteMail(deleteMailRequest);
        }


        public static GetRecipientListResponse GetRecipientList(GetRecipientListRequest getRecipientListRequest)
        {
            return _messageBroker.GetRecipientList(getRecipientListRequest);
        }

        public static ResultResponse CreateRecipient(CreateRecipientRequest createRecipientRequest)
        {
            return _messageBroker.CreateRecipient(createRecipientRequest);
        }

        public static ResultResponse DeleteRecipient(DeleteRecipientRequest deleteRecipientRequest)
        {
            return _messageBroker.DeleteRecipient(deleteRecipientRequest);
        }

        //#region Group

        //public static GetGroupListResponse GetGroupList(GetGroupListRequest getGroupListRequest)
        //{
        //    return _messageBroker.GetGroupList(getGroupListRequest);
        //}

        //public static ResultResponse CreateGroup(CreateGroupRequest request)
        //{
        //    return _messageBroker.CreateGroup(request);
        //}

        //public static ResultResponse UpdateGroup(UpdateGroupRequest request)
        //{
        //    return _messageBroker.UpdateGroup(request);
        //}

        //public static ResultResponse DeleteGroup(DeleteGroupRequest request)
        //{
        //    return _messageBroker.DeleteGroup(request);
        //}
        //#endregion

        #region Report

        public static GetReportListResponse GetReportList(GetReportListRequest request)
        {
            return _messageBroker.GetReportList(request);
        }

        #endregion
    }
}