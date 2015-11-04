namespace Fanex.MailDelivery.Admin.Handlers.Interface
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Messages.MailManage;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Client.Messages.Recipient;

    /// <summary>
    /// Mail Admin Message Broker interface.
    /// </summary>
    public interface IMailAdminMessageBroker
    {
        void Start();

        void Stop();

        void PublishRetryJobMessage(Schedule schedule);

        void PublishScheduleChangeMessage(Schedule schedule);

        void PublishAbortJobMessage(int taskID);

        List<MailModel> RequestGetMailMessage(string mailName);

        ResultMessage RequestCreateMailMessage(CreateMailMessage message);

        ResultMessage RequestUpdateMailMessage(UpdateMailMessage message);

        List<RecipientModel> RequestGetRecipientMessage(string email);

        ResultMessage RequestCreateRecipientMessage(CreateRecipientMessage message);

        ResultMessage RequestUpdateRecipientMessage(UpdateRecipientMessage message);

        RecipientModel RequestGetRecipientByIdMessage(GetRecipientByIdMessage message);
    }
}
