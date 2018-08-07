using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net;
using Fanex.MailDeliver.Business.Implement;
using Fanex.MailDeliver.Core.Infrastructure.Logging;
using Fanex.MailDeliver.Model;
using Fanex.MailStation.Admin.Service.Cache;
using Fanex.MailStation.Admin.Service.Resources;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Configurations;
using Fanex.MailDelivery.Services.Criteria.Criteria;
using Fanex.MailDelivery.Services.Repositories;
using Fanex.MailDelivery.Services.Services.MailService;
using Quartz;
using RazorEngine;
using RazorEngine.Templating;
using System.Linq;

namespace Fanex.MailStation.Admin.Service.Handlers
{
    /// <summary>
    /// For every 5 mins, load all schedules into cache and identify online status.
    /// Send mail to admin if there is any change on online status (On/Off).
    /// </summary>
    public class CheckClientOnlineStatusJob : IJob
    {
        public const string CheckClientOnlineStatusJobName = "CheckClientOnlineStatusJobName";
        public const string CheckClientOnlineStatusTriggerName = "CheckClientOnlineStatusTriggerName";
        private const string keyScheduleMailListCache = "Fanex.MailStation.Admin.Service.Handlers";

        [SuppressMessage("StyleCop.CSharp.StylecopPlus.Rules", "SP2101:MethodMustNotContainMoreLinesThan", Justification = "Reviewed. Suppression is OK here.")]
        public void Execute(IJobExecutionContext context)
        {
            var repository = new MailRepository();
            try
            {
                var scheduleMailListFromDB = repository.GetAllScheduleMail(new GetAllScheduleMailCriteria());
                var cacheScheduleMailList = Caching.Get<List<ClientStatusModel>>(keyScheduleMailListCache);
                if (cacheScheduleMailList != null && cacheScheduleMailList.Count != 0)
                {
                    var listOffline = new List<ClientStatusModel>();
                    var listReconnect = new List<ClientStatusModel>();
                    var newClients = new List<ClientStatusModel>();
                    foreach (var scheduleMail in scheduleMailListFromDB)
                    {
                        var cacheScheduleMail = cacheScheduleMailList.FirstOrDefault(m => m.MailId == scheduleMail.MailId);
                        if (cacheScheduleMail == null)
                        {
                            // There is new added Mail.                            
                            newClients.Add(cacheScheduleMail);
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
                        mailRecord.Body = GenBody(newClients, listReconnect, listOffline, ips);
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
                Caching.Set(keyScheduleMailListCache, scheduleMailListFromDB);
            }
            catch (Exception ex)
            {
                LogExtension.LogError(ex);
            }
        }

        public string GenBody(List<ClientStatusModel> newClient, List<ClientStatusModel> listReconnect, List<ClientStatusModel> listOffline, List<string> ips)
        {
            var template = File.ReadAllText(@"Templates\EmailTemplate.cshtml");
            var tuple = new Tuple<List<ClientStatusModel>, List<ClientStatusModel>, List<ClientStatusModel>, List<string>>(newClient, listReconnect, listOffline, ips);
            return Engine.Razor.RunCompile(template, "template", model: tuple);
        }
    }
}