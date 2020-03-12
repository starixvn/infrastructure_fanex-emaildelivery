using Fanex.Data.Configuration;
using Fanex.MailDelivery.MailStationService.MessageBrokers;
using Fanex.MailDelivery.Services.Services;
using System.ServiceProcess;

namespace Fanex.MailDelivery.MailStationService
{
    partial class MainService : ServiceBase
    {
        private MessageBroker messageBroker;
        private readonly LogService logService;

        public MainService()
            : this(null, null)
        {
        }

        public MainService(MessageBroker messageBroker, LogService logService)
        {
            InitializeComponent();

            this.logService = logService ?? new LogServiceImpl();
            this.messageBroker = messageBroker ?? new MessageBrokerImpl(this.logService);
            DbSettingProviderManager.Current.Start();
        }

        protected override void OnStart(string[] args)
        {
            messageBroker.Start();
        }

        protected override void OnStop()
        {
            messageBroker?.Dispose();
        }
    }
}