using System.Collections.Generic;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Group;
using Fanex.MailDelivery.Core.Models.Mails;
using Fanex.MailDelivery.Core.Models.Recipient;
using Fanex.MailDelivery.Core.Models.Report;
using Fanex.MailDelivery.Services.Criteria.Group;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Criteria.Mail;
using Fanex.MailDelivery.Services.Criteria.Recipient;
using Fanex.MailDelivery.Services.Criteria.Report;
using Fanex.MailDelivery.Services.Models;

namespace Fanex.MailDelivery.Services.Services.Admin
{
    /// <summary>
    /// Mail Service interface.
    /// </summary>
    public interface IAdminDataService
    {
        IList<MailModel> GetListMail(GetMailListCriteria criteria);

        MailModel GetMailById(GetMailByIdCriteria criteria);

        ResultModel CreateMail(CreateMailCriteria criteria);

        ResultModel UpdateMail(UpdateMailCriteria criteria);

        ResultModel DeleteMail(DeleteMailCriteria criteria);

        List<RecipientModel> GetListRecipient(GetRecipientListCriteria criteria);

        RecipientModel GetRecipientById(GetRecipientByIdCriteria criteria);

        ResultModel CreateRecipient(CreateRecipientCriteria criteria);

        ResultModel UpdateRecipient(UpdateRecipientCriteria criteria);

        ResultModel DeleteRecipient(DeleteRecipientCriteria criteria);

        IList<GroupModel> GetGroupList(GetGroupListCriteria criteria);

        GroupModel GetGroupById(GetGroupByIdCriteria criteria);

        ResultModel CreateGroup(CreateGroupCriteria criteria);

        ResultModel UpdateGroup(UpdateGroupCriteria criteria);

        ResultModel DeleteGroup(DeleteGroupCriteria criteria);

        IList<MailTaskModel> GetReportList(GetReportListCriteria criteria);

        Schedule GetScheduleByMailId(GetScheduleByMailIdCriteria criteria);
    }
}