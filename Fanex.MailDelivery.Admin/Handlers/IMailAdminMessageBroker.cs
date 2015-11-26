using System.Collections.Generic;
using Fanex.MailDelivery.Core.Messaging.MailManage;
using Fanex.MailDelivery.Core.Messaging.Recipient;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Messaging;
using Fanex.MailDelivery.Services.Messaging.MailManage;

namespace Fanex.MailDelivery.Admin.Handlers
{
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


        GetMailListResponse RequestGetMailMessage(GetMailListRequest getMailListRequest);

        ResultMessage CreateFunctionMail(CreateFunctionMailRequest createFunctionMailRequest);

        ResultMessage CreateAutoMailMessage(CreateAutoMailRequest createAutoMailRequest);

        //ResultMessage RequestCreateMailMessage(CreateMailMessage message);

        //ResultMessage RequestUpdateMailMessage(UpdateMailMessage message);


        //List<RecipientModel> RequestGetRecipientMessage(string email);

        //ResultMessage RequestCreateRecipientMessage(CreateRecipientMessage message);

        //ResultMessage RequestUpdateRecipientMessage(UpdateRecipientMessage message);

        //RecipientModel RequestGetRecipientByIdMessage(GetRecipientByIdMessage message);
    }
}
