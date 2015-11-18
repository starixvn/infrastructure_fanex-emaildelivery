using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;

namespace Fanex.MailDelivery.Admin.Services.Mail
{
    public class MailService : AdminService, IMailService
    {
        public GetMailListResponse GetMailList(GetMailListRequest request)
        {
            return Bus.Request<GetMailListRequest, GetMailListResponse>(request);
        }

        public GetMailByIdResponse GetMailById(GetMailByIdRequest request)
        {
            return Bus.Request<GetMailByIdRequest, GetMailByIdResponse>(request);
        }

        public ResultResponse CreateFunctionMail(CreateFunctionMailRequest request)
        {
            return Bus.Request<CreateFunctionMailRequest, ResultResponse>(request);
        }

        public ResultResponse CreateAutoMail(CreateAutoMailRequest request)
        {
            return Bus.Request<CreateAutoMailRequest, ResultResponse>(request);
        }

        public ResultResponse UpdateFunctionMail(UpdateFunctionMailRequest request)
        {
            return Bus.Request<UpdateFunctionMailRequest, ResultResponse>(request);
        }

        public ResultResponse UpdateAutoMail(UpdateAutoMailRequest request)
        {
            return Bus.Request<UpdateAutoMailRequest, ResultResponse>(request);
        }

        public ResultResponse DeleteMail(DeleteMailRequest request)
        {
            return Bus.Request<DeleteMailRequest, ResultResponse>(request);
        }
    }
}