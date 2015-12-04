#region

using System;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;

#endregion

namespace Fanex.MailDelivery.Admin.Services.Recipient
{
    public interface IRecipientService : IDisposable
    {
        GetRecipientListResponse GetRecipientList(GetRecipientListRequest request);

        GetRecipientByIdResponse GetRecipientById(GetRecipientByIdRequest request);

        ResultResponse CreateRecipient(CreateRecipientRequest request);

        ResultResponse UpdateRecipient(UpdateRecipientRequest request);

        ResultResponse DeleteRecipient(DeleteRecipientRequest request);
    }
}