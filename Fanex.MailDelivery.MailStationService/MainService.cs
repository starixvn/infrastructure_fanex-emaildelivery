using Fanex.Data.Configuration;
using Fanex.MailDelivery.MailStationService.MessageBrokers;
using Fanex.MailDelivery.Services.Services.Logging;
using System.ServiceProcess;

namespace Fanex.MailDelivery.MailStationService
{
    partial class MainService : ServiceBase
    {
        private IMessageBroker messageBroker;
        private readonly LogService logService;

        public MainService()
            : this(null, null)
        {
        }

        public MainService(IMessageBroker messageBroker, LogService logService)
        {
            InitializeComponent();

            this.logService = logService ?? new LogServiceImpl();
            this.messageBroker = messageBroker ?? new MessageBroker(this.logService);
            DbSettingProviderManager.Current.Start();
        }

        protected override async void OnStart(string[] args)
        {
            await messageBroker.StartAsync();
        }

        protected override void OnStop()
        {
            messageBroker?.Dispose();
        }
    }
}