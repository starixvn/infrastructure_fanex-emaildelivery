using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;

namespace Fanex.MailDelivery.Admin.Services.Mail
{
    public interface IMailService
    {
        GetMailListResponse GetMailList(GetMailListRequest request);

        ResultResponse CreateFunctionMail(CreateFunctionMailRequest request);
        ResultResponse CreateAutoMail(CreateAutoMailRequest request);

        ResultResponse UpdateFunctionMail(UpdateFunctionMailRequest request);
        ResultResponse UpdateAutoMail(UpdateAutoMailRequest request);

        ResultResponse DeleteMail(DeleteMailRequest request);
    }
}
