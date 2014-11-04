namespace Fanex.MailDelivery.Server.Installer
{
    using System.ServiceProcess;
    using Fanex.Data.Configuration;
    using Fanex.MailDelivery.Handlers.Impl;
    using Fanex.MailDelivery.Handlers.Interface;

    partial class MailCentralizedService : ServiceBase
    {
        private IMessageBroker _messageBroker;
        private JobHandler _jobHandler;

        public MailCentralizedService()
            : this(null)
        {
        }

        public MailCentralizedService(IMessageBroker messageBroker)
        {
            InitializeComponent();
            _messageBroker = messageBroker ?? new RabbitMQBroker();
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