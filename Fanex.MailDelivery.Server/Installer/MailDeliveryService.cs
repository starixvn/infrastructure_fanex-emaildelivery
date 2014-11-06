namespace Fanex.MailDelivery.Server.Installer
{
    using System.Diagnostics.CodeAnalysis;
    using System.ServiceProcess;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Handlers.Impl;
    using Fanex.MailDelivery.Handlers.Interface;

    /// <summary>
    /// Mail Centralized Service.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Reviewed.")]
    partial class MailDeliveryService : ServiceBase
    {
        private IMessageBroker _messageBroker;
        private JobHandler _jobHandler;

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
            _messageBroker.Start();
            _jobHandler.Start();
        }

        protected override void OnStop()
        {
            _messageBroker.Dispose();
            _jobHandler.Stop();
        }
    }
}