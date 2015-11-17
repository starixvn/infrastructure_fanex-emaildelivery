using System.Collections.Generic;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Models;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Server.Criteria.Group;
using Fanex.MailDelivery.Server.Criteria.Mail;
using Fanex.MailDelivery.Server.Criteria.Recipient;
using Fanex.MailDelivery.Server.Criteria.Report;
using Fanex.MailDelivery.Services.Messaging;

namespace Fanex.MailDelivery.Server.Services
{
    /// <summary>
    /// Mail Service interface.
    /// </summary>
    public interface IAdminService
    {
        RecipientModel RecipientById(RecipientByIdCriteria criteria);


        List<MailModel> GetListMail(GetMailListCriteria criteria);
        ResultResponse CreateMail(CreateMailCriteria criteria);
        ResultResponse UpdateMail(UpdateMailCriteria criteria);
        ResultResponse DeleteMail(DeleteMailCriteria criteria);

        List<RecipientModel> GetListRecipient(RecipientListCriteria criteria);
        ResultResponse CreateRecipient(CreateRecipientCriteria criteria);
        ResultResponse UpdateRecipient(UpdateRecipientCriteria criteria);
        ResultResponse DeleteRecipient(DeleteRecipientCriteria criteria);

        IList<GroupModel> GetGroupList(GetGroupListCriteria criteria);
        ResultResponse CreateGroup(CreateGroupCriteria criteria);
        ResultResponse UpdateGroup(UpdateGroupCriteria criteria);
        ResultResponse DeleteGroup(DeleteGroupCriteria criteria);

        IList<MailTaskModel> GetReportList(GetReportListCriteria criteria);
    }
}