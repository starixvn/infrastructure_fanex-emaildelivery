using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.MailDelivery.Client.Models;
using Fanex.MailDelivery.Criteria;
using Fanex.MailDelivery.Client.Enumerations;

namespace Fanex.MailDelivery.Repositories
{
    public interface IMailRepository
    {
        Schedule GetScheduleByID(GetScheduleByIDCriteria criteria);

        int AddMailTask(AddMailTaskCriteria criteria);

        void UpdateMailTaskStatus(UpdateMailTaskStatusCriteria criteria);

        void UpdateRetryCounter(UpdateTaskRetryCounterCriteria criteria);

        void AddMailLog(AddMailLogCriteria criteria);

        MailingList GetMailingList(GetMailWithEmailAddressByMailID criteria);

        MailType GetMailingListType(GetMailingListTypeCriteria criteria);
    }
}
