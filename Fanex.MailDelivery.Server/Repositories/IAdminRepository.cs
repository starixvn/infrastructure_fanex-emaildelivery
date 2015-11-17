using System.Collections.Generic;
using Fanex.MailDelivery.Admin.Models;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Server.Criteria.Group;
using Fanex.MailDelivery.Server.Criteria.Report;
using Fanex.MailDelivery.Services.Criteria;
using Fanex.MailDelivery.Services.Messaging;
using Fanex.MailDelivery.Services.Models;
using CreateMailCriteria = Fanex.MailDelivery.Server.Criteria.Mail.CreateMailCriteria;
using CreateRecipientCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.CreateRecipientCriteria;
using DeleteMailCriteria = Fanex.MailDelivery.Server.Criteria.Mail.DeleteMailCriteria;
using DeleteRecipientCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.DeleteRecipientCriteria;
using GetMailListCriteria = Fanex.MailDelivery.Server.Criteria.Mail.GetMailListCriteria;
using RecipientByIdCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.RecipientByIdCriteria;
using RecipientListCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.RecipientListCriteria;
using UpdateMailCriteria = Fanex.MailDelivery.Server.Criteria.Mail.UpdateMailCriteria;
using UpdateRecipientCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.UpdateRecipientCriteria;

namespace Fanex.MailDelivery.Server.Repositories
{
    public interface IAdminRepository
    {
        Schedule GetScheduleById(GetScheduleByIdCriteria criteria);

        int AddMailTask(AddMailTaskCriteria criteria);

        void UpdateMailTaskStatus(UpdateMailTaskStatusCriteria criteria);

        void UpdateRetryCounter(UpdateTaskRetryCounterCriteria criteria);

        void AddMailLog(AddMailLogCriteria criteria);

        MailingList GetMailingList(GetMailWithEmailAddressByMailId criteria);

        void UpdateMailTask(UpdateMailTaskMSiteMailIdCriteria criteria);

        MailTask GetMailTask(GetMailTaskByIdCriteria criteria);

        void UpdateClientActiveTime(UpdateClientActiveTimeCriteria criteria);

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
