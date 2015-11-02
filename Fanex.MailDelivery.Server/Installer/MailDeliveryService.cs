namespace Fanex.MailDelivery.Server.Installer
{
    using System.Diagnostics.CodeAnalysis;
    using System.ServiceProcess;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Handlers.Impl;
    using Fanex.MailDelivery.Handlers.Interface;
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
            Fanex.MailDelivery.Services.ServiceBase.LogInfo(string.Format(serviceInfo, this.ServiceName, "starts", DateTime.Now.ToString()));
            _messageBroker.Start();
            _jobHandler.Start();
        }

        protected override void OnStop()
        {
            Fanex.MailDelivery.Services.ServiceBase.LogInfo(string.Format(serviceInfo, this.ServiceName, "stops", DateTime.Now.ToString()));
            _messageBroker.Dispose();
            _jobHandler.Stop();
        }
    }
}