using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Admin.Messaging.Schedule;
using Fanex.MailDelivery.Services.Messaging;

namespace Fanex.MailDelivery.Admin.Handlers
{
    /// <summary>
    /// Mail Admin Message Broker interface.
    /// </summary>
    public interface IMailAdminMessageBroker
    {
        void Start();

        void Stop();

        void RetryJobMessage(ScheduleRequest schedule);

        void PublishScheduleChangeMessage(ScheduleRequest schedule);

        void PublishAbortJobMessage(int taskId);


        GetMailListResponse GetMailList(GetMailListRequest request);

        ResultResponse CreateFunctionMail(CreateFunctionMailRequest request);
        ResultResponse CreateAutoMail(CreateAutoMailRequest request);

        ResultResponse UpdateFunctionMail(UpdateFunctionMailRequest request);
        ResultResponse UpdateAutoMail(UpdateAutoMailRequest request);

        ResultResponse DeleteMail(DeleteMailRequest request);


        GetRecipientListResponse GetRecipientList(GetRecipientListRequest request);
        ResultResponse CreateRecipient(CreateRecipientRequest request);
        ResultResponse DeleteRecipient(DeleteRecipientRequest request);


        GetGroupListResponse GetGroupList(GetGroupListRequest request);
        ResultResponse CreateGroup(CreateGroupRequest request);
        ResultResponse UpdateGroup(UpdateGroupRequest request);
        ResultResponse DeleteGroup(DeleteGroupRequest request);

    }
}
