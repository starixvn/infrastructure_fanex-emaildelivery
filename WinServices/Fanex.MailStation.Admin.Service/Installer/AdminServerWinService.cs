using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.ServiceProcess;
using System.Threading;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.Services.Configurations;
using Fanex.MailDelivery.Services.Services.Logging;
using Fanex.MailStation.Admin.Service.Cache;
using Fanex.MailStation.Admin.Service.Handlers;
using Fanex.MailStation.Admin.Service.MessageBrokers;

namespace Fanex.MailStation.Admin.Service.Installer
{
    /// <summary>
    /// Mail Centralized Service.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Reviewed.")]
    partial class AdminServerWinService : ServiceBase
    {
        private AdminServerMessageBroker _messageBroker;
        private JobHandler _jobHandler;
        private int counter;
        private int maxRetryStartUpService = AppSettings.MailServiceSection.MaxRetryStartUpService;
        private int maxIntervalStartUpServiceInSecond = AppSettings.MailServiceSection.MaxIntervalStartUpServiceInSecond;

        private string serviceInfo = "Service {0} {1} at {2}.";

        public AdminServerWinService()
        {
            InitializeComponent();
            _messageBroker = new AdminServerMessageBroker();
            _jobHandler = new JobHandler();
            DbSettingProviderManager.Current.Start();
        }

        protected override void OnStart(string[] args)
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
            Caching.Register();
            LogService.LogInfo(string.Format(serviceInfo, ServiceName, "starts", DateTime.Now.ToString()));            

            while (true)
            {
                var result = Execute();
                if (result)
                {
                    break;
                }

                if (counter >= maxRetryStartUpService)
                {
                    throw new Exception("The service Fanex.MailStation.Service can't be started, maybe it's related to RabbitMQ or something else.");
                }

                Thread.Sleep(1000 * maxIntervalStartUpServiceInSecond);
            }

            _jobHandler.Start();
        }

        protected override void OnStop()
        {
            LogService.LogInfo(string.Format(serviceInfo, this.ServiceName, "stops", DateTime.Now.ToString()));
            _messageBroker.Dispose();
            _jobHandler.Stop();
        }

        private bool Execute()
        {
            var result = false;
            try
            {
                counter++;
                LogService.LogInfo(string.Format(serviceInfo, ServiceName, "starts", DateTime.Now.ToString()));
                _messageBroker.Start();
                result = true;
            }
            catch (Exception exception)
            {
                LogService.LogError(exception);
            }

            return result;
        }
    }
}