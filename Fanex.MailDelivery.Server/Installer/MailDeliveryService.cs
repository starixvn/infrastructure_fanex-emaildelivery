using Fanex.MailDelivery.Server.Handlers;
using Fanex.MailDelivery.Services.Handlers.Impl;
using Fanex.MailDelivery.Services.Handlers.Interface;
using Fanex.MailDelivery.Services.MailService;

namespace Fanex.MailDelivery.Server.Installer
{
    using System.Diagnostics.CodeAnalysis;
    using System.ServiceProcess;
    using Data.Configuration;
    using System;

    /// <summary>
    /// Mail Centralized Service.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Reviewed.")]
    partial class MailDeliveryService : ServiceBase
    {
        private IMessageBroker _messageBroker;
        private JobHandler _jobHandler;

        private string serviceInfo = "Service {0} {1} at {2}.";

        public MailDeliveryService()
            : this(null)
        {
        }

        public MailDeliveryService(IMessageBroker messageBroker)
        {
            InitializeComponent();
            _messageBroker = messageBroker ?? new MailServiceMessageBroker();
            _jobHandler = new JobHandler();
            DbSettingProviderManager.Current.Start();
        }

        protected override void OnStart(string[] args)
        {
            MailService.LogInfo(string.Format(serviceInfo, ServiceName, "starts", DateTime.Now.ToString()));
            _messageBroker.Start();
            _jobHandler.Start();
        }

        protected override void OnStop()
        {
            MailService.LogInfo(string.Format(serviceInfo, this.ServiceName, "stops", DateTime.Now.ToString()));
            _messageBroker.Dispose();
            _jobHandler.Stop();
        }
    }
}