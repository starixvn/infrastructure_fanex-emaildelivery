using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.Job;
using Fanex.MailDelivery.Admin.Messaging.ManageGroup;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Admin.Messaging.ManageReport;

namespace Fanex.MailDelivery.AdminWinService.Services
{
    public interface IAdminServerService
    {
        GetMailListResponse GetMailList(GetMailListRequest request);

        GetMailByIdResponse GetMailById(GetMailByIdRequest request);

        ResultResponse CreateFunctionMail(CreateFunctionMailRequest request);

        ResultResponse CreateAutoMail(CreateAutoMailRequest request);

        ResultResponse UpdateFunctionMail(UpdateFunctionMailRequest request);

        ResultResponse UpdateAutoMail(UpdateAutoMailRequest request);

        ResultResponse DeleteMail(DeleteMailRequest request);

        GetRecipientListResponse GetRecipientList(GetRecipientListRequest request);

        GetRecipientByIdResponse GetRecipientById(GetRecipientByIdRequest request);

        ResultResponse CreateRecipient(CreateRecipientRequest request);

        ResultResponse UpdateRecipient(UpdateRecipientRequest request);

        ResultResponse DeleteRecipient(DeleteRecipientRequest request);

        GetGroupListResponse GetGroupList(GetGroupListRequest request);

        GetMailGroupByIdResponse GetGroupById(GetMailGroupByIdRequest request);

        ResultResponse CreateGroup(CreateGroupRequest request);

        ResultResponse UpdateGroup(UpdateGroupRequest request);

        ResultResponse DeleteGroup(DeleteGroupRequest request);

        GetReportListResponse GetReportList(GetReportListRequest request);

        GetScheduleByMailIdResponse GetScheduleByMailId(GetScheduleByMailIdRequest request);
    }
}
