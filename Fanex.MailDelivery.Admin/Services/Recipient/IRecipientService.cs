using System;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;

namespace Fanex.MailDelivery.Admin.Services.Recipient
{
    public interface IRecipientService : IDisposable
    {
        GetRecipientListResponse GetRecipientList(GetRecipientListRequest request);

        ResultResponse CreateRecipient(CreateRecipientRequest request);

        ResultResponse UpdateRecipient(UpdateRecipientRequest request);

        ResultResponse DeleteRecipient(DeleteRecipientRequest request);
    }
}