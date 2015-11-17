using System;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Services.Messaging;

namespace Fanex.MailDelivery.Admin.Services.Recipient
{
    public interface IRecipientService : IDisposable
    {
        GetRecipientListResponse GetRecipientList(GetRecipientListRequest request);

        ResultResponse CreateRecipient(CreateRecipientRequest request);

        ResultResponse DeleteRecipient(DeleteRecipientRequest request);
    }
}