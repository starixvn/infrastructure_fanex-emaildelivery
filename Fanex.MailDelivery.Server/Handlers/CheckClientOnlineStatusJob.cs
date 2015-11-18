using System.Collections.Generic;
using Fanex.MailDelivery.Admin.Models;
using Fanex.MailDelivery.Core.Models;
using Quartz;

namespace Fanex.MailDelivery.Server.Handlers
{
    /// <summary>
    /// For every 5 mins, load all schedules into cache and identify online status. 
    /// Send mail to admin if there is any change on online status (On/Off).
    /// </summary>
    public class CheckClientOnlineStatusJob : IJob
    {
        public const string CheckClientOnlineStatusJobName = "CheckClientOnlineStatusJobName";
        public const string CheckClientOnlineStatusTriggerName = "CheckClientOnlineStatusTriggerName";

        public void Execute(IJobExecutionContext context)
        {
            //var repository = new MailRepository();
            //try
            //{
            //    List<ClientStatusModel> scheduleMailList = new List<ClientStatusModel>();
            //    scheduleMailList = repository.GetAllScheduleMail(new GetAllScheduleMailCriteria());
            //    if (CacheExtension.ScheduleMailList != null && CacheExtension.ScheduleMailList.Count != 0)
            //    {
            //        List<ClientStatusModel> listOffline = new List<ClientStatusModel>();
            //        List<ClientStatusModel> listReconnect = new List<ClientStatusModel>();
            //        foreach (var scheduleMail in scheduleMailList)
            //        {
            //            var cacheScheduleMail = CacheExtension.Find(scheduleMail.MailId);
            //            if (cacheScheduleMail == null)
            //            { 
            //                // There is new added Mail.
            //                CacheExtension.ScheduleMailList.Add(scheduleMail);
            //            }
            //            else if (cacheScheduleMail.IsOnline != scheduleMail.IsOnline)
            //            {
            //                if (scheduleMail.IsOnline)
            //                {
            //                    // This client is come back to live.
            //                    listReconnect.Add(scheduleMail);
            //                }
            //                else
            //                {
            //                    // This client is died.
            //                    listOffline.Add(scheduleMail);
            //                }
            //            }
            //        }

            //        if (listOffline.Count > 0 || listReconnect.Count > 0)
            //        {
            //            MailRecord mailRecord = new MailRecord();
            //            mailRecord.MailTo = AppSettings.MailServiceSection.AdminMail;
            //            mailRecord.Subject = string.Format(MailStationResource.NotifyMailSubject, DateTime.Now.ToString("yyyyMMdd"));
            //            mailRecord.Title = string.Format(MailStationResource.NotifyMailSubject, DateTime.Now.ToString("yyyyMMdd"));
            //            mailRecord.AppID = AppSettings.MailServiceSection.AppSendMailToAdminId;
            //            mailRecord.Config = new MailConfig() { AppID = AppSettings.MailServiceSection.AppSendMailToAdminId };
            //            string reconnectServiceMessage = string.Format(MailStationResource.NotifyMailBodyReconnectServices, GenerateListServiceNameString(listReconnect));
            //            string offlineServiceMessage = string.Format(MailStationResource.NotifyMailBodyDisconnectServices, GenerateListServiceNameString(listOffline));
            //            mailRecord.Body = string.Format(MailStationResource.NotifyMailBody, offlineServiceMessage, reconnectServiceMessage);
            //            MailService mailService = new MailService.MailService();
            //            var mailDeliverBusiness = new MailDeliverBusiness();
            //            int mailId = (int)mailDeliverBusiness.InsertMailRecord(mailRecord);
            //            mailRecord.MailID = mailId;
            //            mailRecord.IsReady = true;
            //            if (mailId > 0)
            //            {
            //                mailService.SendMail(mailRecord);
            //            }
            //        }
            //    }

            //    // If there is no previous cache.
            //    CacheExtension.ScheduleMailList = scheduleMailList;
            //}
            //catch (Exception ex)
            //{
            //    LogExtension.LogError(ex);
            //}
        }

        private string GenerateListServiceNameString(List<ClientStatusModel> listReconnect)
        {
            string reconnectServiceMessage = string.Empty;
            foreach (var reconnectService in listReconnect)
            {
                reconnectServiceMessage += "<br>&nbsp;&nbsp;&nbsp;- " + reconnectService.MailName;
            }

            return reconnectServiceMessage;
        }
    }
}
