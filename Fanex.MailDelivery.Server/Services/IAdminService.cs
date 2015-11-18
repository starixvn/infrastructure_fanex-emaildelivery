using System.Collections.Generic;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Models;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Server.Criteria.Group;
using Fanex.MailDelivery.Server.Criteria.Mail;
using Fanex.MailDelivery.Server.Criteria.Recipient;
using Fanex.MailDelivery.Server.Criteria.Report;

namespace Fanex.MailDelivery.Server.Services
{
    /// <summary>
    /// Mail Service interface.
    /// </summary>
    public interface IAdminService
    {
        RecipientModel RecipientById(GetRecipientByIdCriteria criteria);

        List<MailModel> GetListMail(GetMailListCriteria criteria);
        MailModel GetMailById(GetMailByIdCriteria criteria);
        ResultResponse CreateMail(CreateMailCriteria criteria);
        ResultResponse UpdateMail(UpdateMailCriteria criteria);
        ResultResponse DeleteMail(DeleteMailCriteria criteria);

        List<RecipientModel> GetListRecipient(GetRecipientListCriteria criteria);
        RecipientModel GetRecipientById(GetRecipientByIdCriteria criteria);
        ResultResponse CreateRecipient(CreateRecipientCriteria criteria);
        ResultResponse UpdateRecipient(UpdateRecipientCriteria criteria);
        ResultResponse DeleteRecipient(DeleteRecipientCriteria criteria);

        IList<GroupModel> GetGroupList(GetGroupListCriteria criteria);
        GroupModel GetGroupById(GetGroupByIdCriteria criteria);
        ResultResponse CreateGroup(CreateGroupCriteria criteria);
        ResultResponse UpdateGroup(UpdateGroupCriteria criteria);
        ResultResponse DeleteGroup(DeleteGroupCriteria criteria);

        IList<MailTaskModel> GetReportList(GetReportListCriteria criteria);
    }
}