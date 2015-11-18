using System.Collections.Generic;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Models;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Server.Criteria.Group;
using Fanex.MailDelivery.Server.Criteria.Mail;
using Fanex.MailDelivery.Server.Criteria.Report;
using Fanex.MailDelivery.Services.Criteria;
using CreateMailCriteria = Fanex.MailDelivery.Server.Criteria.Mail.CreateMailCriteria;
using CreateRecipientCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.CreateRecipientCriteria;
using DeleteMailCriteria = Fanex.MailDelivery.Server.Criteria.Mail.DeleteMailCriteria;
using DeleteRecipientCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.DeleteRecipientCriteria;
using GetMailListCriteria = Fanex.MailDelivery.Server.Criteria.Mail.GetMailListCriteria;
using MailTask = Fanex.MailDelivery.Admin.Models.MailTask;
using GetRecipientByIdCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.GetRecipientByIdCriteria;
using GetRecipientListCriteria = Fanex.MailDelivery.Server.Criteria.Recipient.GetRecipientListCriteria;
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

        RecipientModel RecipientById(GetRecipientByIdCriteria criteria);

        List<MailModel> GetMailList(GetMailListCriteria criteria);
        MailModel GetMailById(GetMailByIdCriteria criteria);
        ResultResponse CreateMail(CreateMailCriteria criteria);
        ResultResponse UpdateMail(UpdateMailCriteria criteria);
        ResultResponse DeleteMail(DeleteMailCriteria criteria);

        List<RecipientModel> GetRecipientList(GetRecipientListCriteria criteria);
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
