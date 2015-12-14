using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceProcess;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.ClientWinService.MessageBrokers;
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
            LogService.LogInfo(string.Format(serviceInfo, ServiceName, "starts", DateTime.Now.ToString()));
            _clientServerMessageBroker.Start();
        }

        protected override void OnStop()
        {
            LogService.LogInfo(string.Format(serviceInfo, this.ServiceName, "stops", DateTime.Now.ToString()));
            _clientServerMessageBroker.Dispose();
        }
    }
}