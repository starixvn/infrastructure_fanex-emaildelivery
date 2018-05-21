using System;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;

namespace Fanex.MailDelivery.Admin.Services.Recipient
{
    public class RecipientService : AdminService, IRecipientService
    {
        ~RecipientService()
        {
            Dispose(false);
        }

        public GetRecipientListResponse GetRecipientList(GetRecipientListRequest request)
        {
            return Bus.Request<GetRecipientListRequest, GetRecipientListResponse>(request);
        }

        public GetRecipientByIdResponse GetRecipientById(GetRecipientByIdRequest request)
        {
            return Bus.Request<GetRecipientByIdRequest, GetRecipientByIdResponse>(request);
        }

        public ResultResponse CreateRecipient(CreateRecipientRequest request)
        {
            return Bus.Request<CreateRecipientRequest, ResultResponse>(request);
        }

        public ResultResponse UpdateRecipient(UpdateRecipientRequest request)
        {
            return Bus.Request<UpdateRecipientRequest, ResultResponse>(request);
        }

        public ResultResponse DeleteRecipient(DeleteRecipientRequest request)
        {
            return Bus.Request<DeleteRecipientRequest, ResultResponse>(request);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            // Cleanup
        }
    }
}