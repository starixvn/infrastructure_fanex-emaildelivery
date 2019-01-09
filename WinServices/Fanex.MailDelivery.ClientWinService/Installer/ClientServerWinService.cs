using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceProcess;
using System.Threading;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.ClientWinService.MessageBrokers;
using Fanex.MailDelivery.Services.Configurations;
using Fanex.MailDelivery.Services.Services.Logging;

namespace Fanex.MailDelivery.ClientWinService.Installer
{
    /// <summary>
    /// Mail Centralized Service.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Reviewed.")]
    partial class ClientServerWinService : ServiceBase
    {
        private IClientServerMessageBroker _clientServerMessageBroker;
        private int counter; 
        private int maxRetryStartUpService = AppSettings.MailServiceSection.MaxRetryStartUpService;
        private int maxIntervalStartUpServiceInSecond = AppSettings.MailServiceSection.MaxIntervalStartUpServiceInSecond;

        private string serviceInfo = "Service {0} {1} at {2}.";

        public ClientServerWinService()
            : this(null)
        {
        }

        public ClientServerWinService(IClientServerMessageBroker messageBroker)
        {
            InitializeComponent();
            _clientServerMessageBroker = messageBroker ?? new ClientServerMessageBroker();
            DbSettingProviderManager.Current.Start();
        }

        protected override void OnStart(string[] args)
        {
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
        }

        protected override void OnStop()
        {
            LogService.LogInfo(string.Format(serviceInfo, this.ServiceName, "stops", DateTime.Now.ToString()));
            if (_clientServerMessageBroker != null)
            {
                _clientServerMessageBroker.Dispose();
            }
        }

        private bool Execute()
        {
            var result = false;
            try
            {
                counter++;
                LogService.LogInfo(string.Format(serviceInfo, ServiceName, "starts", DateTime.Now.ToString()));
                _clientServerMessageBroker.Start();
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