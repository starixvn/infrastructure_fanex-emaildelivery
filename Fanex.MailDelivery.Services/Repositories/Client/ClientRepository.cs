using System;
using System.Linq;
using Fanex.Data;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Criteria.Criteria;
using Fanex.MailDelivery.Services.Criteria.Job;

namespace Fanex.MailDelivery.Services.Repositories.Client
{
    public class ClientRepository : IClientRepository
    {
        public Schedule GetScheduleByMailId(GetScheduleByMailIdCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<Schedule>().FirstOrDefault();
            }
        }

        public void UpdateClientActiveTime(UpdateClientActiveTimeCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }

        public void UpdateServiceCheckIn(ServiceMonitoringCheckInCriteria criteria)
        {
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(criteria.GetSpParams());
            }
        }
    }
}