using System.Collections.Generic;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Criteria.Criteria;
using Fanex.MailDelivery.Services.Criteria.Group;
using Fanex.MailDelivery.Services.Criteria.Mail;
using Fanex.MailDelivery.Services.Criteria.Recipient;
using Fanex.MailDelivery.Services.Criteria.Report;
using Fanex.MailDelivery.Services.Models;


namespace Fanex.MailDelivery.Services.Repositories
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
        ResultModel CreateMail(CreateMailCriteria criteria);
        ResultModel UpdateMail(UpdateMailCriteria criteria);
        ResultModel DeleteMail(DeleteMailCriteria criteria);

        List<RecipientModel> GetRecipientList(GetRecipientListCriteria criteria);
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
    }
}
