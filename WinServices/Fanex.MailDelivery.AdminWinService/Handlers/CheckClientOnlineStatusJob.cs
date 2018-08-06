using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using Fanex.MailDeliver.Business.Implement;
using Fanex.MailDeliver.Core.Infrastructure.Logging;
using Fanex.MailDeliver.Model;
using Fanex.MailDelivery.AdminWinService.Cache;
using Fanex.MailDelivery.AdminWinService.Resources;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Configurations;
using Fanex.MailDelivery.Services.Criteria.Criteria;
using Fanex.MailDelivery.Services.Repositories;
using Fanex.MailDelivery.Services.Services.MailService;
using Quartz;
using RazorEngine;
using RazorEngine.Templating;

namespace Fanex.MailDelivery.AdminWinService.Handlers
{
    /// <summary>
    /// For every 5 mins, load all schedules into cache and identify online status.
    /// Send mail to admin if there is any change on online status (On/Off).
    /// </summary>
    public class CheckClientOnlineStatusJob : IJob
    {
        public const string CheckClientOnlineStatusJobName = "CheckClientOnlineStatusJobName";
        public const string CheckClientOnlineStatusTriggerName = "CheckClientOnlineStatusTriggerName";

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void Execute(IJobExecutionContext context)
        {
            var repository = new MailRepository();
            try
            {
                List<ClientStatusModel> scheduleMailList = new List<ClientStatusModel>();
                scheduleMailList = repository.GetAllScheduleMail(new GetAllScheduleMailCriteria());
                if (CacheExtension.ScheduleMailList != null && CacheExtension.ScheduleMailList.Count != 0)
                {
                    List<ClientStatusModel> listOffline = new List<ClientStatusModel>();
                    List<ClientStatusModel> listReconnect = new List<ClientStatusModel>();
                    foreach (var scheduleMail in scheduleMailList)
                    {
                        var cacheScheduleMail = CacheExtension.Find(scheduleMail.MailId);
                        if (cacheScheduleMail == null)
                        {
                            // There is new added Mail.
                            CacheExtension.ScheduleMailList.Add(scheduleMail);
                        }
                        else if (cacheScheduleMail.IsOnline != scheduleMail.IsOnline)
                        {
                            if (scheduleMail.IsOnline)
                            {
                                // This client is come back to live.
                                listReconnect.Add(scheduleMail);
                            }
                            else
                            {
                                // This client is died.
                                listOffline.Add(scheduleMail);
                            }
                        }
                    }

                    if (listOffline.Count > 0 || listReconnect.Count > 0)
                    {
                        var hostName = Dns.GetHostName();
                        var addressList = Dns.GetHostEntry(hostName).AddressList;
                        var ips = new List<string>();
                        ips.Add(hostName);
                        foreach (var ip in addressList)
                        {
                            ips.Add(ip.ToString());
                        }

                        var subject = $"{string.Format(MailStationResource.NotifyMailSubject, DateTime.Now.ToString("yyyyMMdd"))} - HostName: {hostName}";
                        MailRecord mailRecord = new MailRecord();
                        mailRecord.MailTo = AppSettings.MailServiceSection.AdminMail;
                        mailRecord.Subject = subject;
                        mailRecord.Title = "Auto Generated Report Administrator";
                        mailRecord.AppID = AppSettings.MailServiceSection.AppSendMailToAdminId;
                        mailRecord.Config = new MailConfig() { AppID = AppSettings.MailServiceSection.AppSendMailToAdminId };
                        string reconnectServiceMessage = string.Format(MailStationResource.NotifyMailBodyReconnectServices, GenerateListServiceNameString(listReconnect));
                        string offlineServiceMessage = $"{string.Format(MailStationResource.NotifyMailBodyDisconnectServices, GenerateListServiceNameString(listOffline))} <br/> - From Server: {string.Join("<br/> - ", ips)}";
                        mailRecord.Body = GenBody(listReconnect, listOffline, ips);// $"{string.Format(MailStationResource.NotifyMailBody, offlineServiceMessage, reconnectServiceMessage)}";
                        MailService mailService = new MailService();
                        var mailDeliverBusiness = new MailDeliverBusiness();
                        int mailId = (int)mailDeliverBusiness.InsertMailRecord(mailRecord);
                        mailRecord.MailID = mailId;
                        mailRecord.IsReady = true;
                        if (mailId > 0)
                        {
                            mailService.SendMail(mailRecord);
                        }
                    }
                }

                // If there is no previous cache.
                CacheExtension.ScheduleMailList = scheduleMailList;
            }
            catch (Exception ex)
            {
                LogExtension.LogError(ex);
            }
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

        public string GenBody(List<ClientStatusModel> listReconnect, List<ClientStatusModel> listOffline, List<string> ips)
        {
            var template = File.ReadAllText(@"Templates\EmailTemplate.cshtml");
            var tuple = new Tuple<List<ClientStatusModel>, List<ClientStatusModel>, List<string>>(listReconnect, listOffline, ips);
            return Engine.Razor.RunCompile(template, "template", model: tuple);
        }
    }
}