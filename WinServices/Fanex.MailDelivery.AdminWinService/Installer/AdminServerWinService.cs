using System;
using System.Diagnostics.CodeAnalysis;
using System.ServiceProcess;
using Fanex.Data.Configuration;
using Fanex.MailDelivery.AdminWinService.Handlers;

namespace Fanex.MailDelivery.AdminWinService.Installer
{
    /// <summary>
    /// Mail Centralized Service.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Reviewed.")]
    partial class AdminServerWinService : ServiceBase
    {
        private IAdminServerMessageBroker _messageBroker;
        private JobHandler _jobHandler;

        private string serviceInfo = "Service {0} {1} at {2}.";

        public AdminServerWinService()
            : this(null)
        {
        }

        public AdminServerWinService(IAdminServerMessageBroker messageBroker)
        {
            InitializeComponent();
            _messageBroker = messageBroker ?? new AdminServerMessageBroker();
            _jobHandler = new JobHandler();
            DbSettingProviderManager.Current.Start();
        }

        protected override void OnStart(string[] args)
        {
            MailService.ServiceBase.LogInfo(string.Format(serviceInfo, ServiceName, "starts", DateTime.Now.ToString()));
            _messageBroker.Start();
            _jobHandler.Start();
        }

        protected override void OnStop()
        {
            MailService.ServiceBase.LogInfo(string.Format(serviceInfo, this.ServiceName, "stops", DateTime.Now.ToString()));
            _messageBroker.Dispose();
            _jobHandler.Stop();
        }
    }
}