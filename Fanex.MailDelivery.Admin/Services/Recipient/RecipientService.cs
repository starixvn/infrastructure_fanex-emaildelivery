using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Services.Messaging;

namespace Fanex.MailDelivery.Admin.Services.Recipient
{
    public class RecipientService : AdminService, IRecipientService
    {
        public GetRecipientListResponse GetRecipientList(GetRecipientListRequest request)
        {
            return Bus.Request<GetRecipientListRequest, GetRecipientListResponse>(request);
        }

        public ResultResponse CreateRecipient(CreateRecipientRequest request)
        {
            return Bus.Request<CreateRecipientRequest, ResultResponse>(request);
        }

        public ResultResponse DeleteRecipient(DeleteRecipientRequest request)
        {
            return Bus.Request<DeleteRecipientRequest, ResultResponse>(request);
        }
    }
}