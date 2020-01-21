using Fanex.Data.Configuration;
using Fanex.MailDelivery.MailStationService.MessageBrokers;
using Fanex.MailDelivery.Services.Services.Logging;
using System;
using System.ServiceProcess;

namespace Fanex.MailDelivery.MailStationService
{
    partial class MainService : ServiceBase
    {
        private IMessageBroker messageBroker;
        private int startCounter = 0;
        private readonly LogService logService;

        public MainService()
            : this(null, null)
        {
        }

        public MainService(IMessageBroker messageBroker, LogService logService)
        {
            InitializeComponent();

            this.logService = logService ?? new LogServiceImpl();
            this.messageBroker = messageBroker ?? new MessageBroker();
            DbSettingProviderManager.Current.Start();
        }

        protected override void OnStart(string[] args)
        {
            StartService();
        }

        protected override void OnStop()
        {
            messageBroker?.Dispose();
        }

        private void StartService()
        {
            startCounter++;

            if (startCounter <= AppSettings.StartRetryLimit)
            {
                try
                {
                    messageBroker.Start();

                    startCounter = 0;
                }
                catch (Exception exception)
                {
                    logService.LogError(exception);

                    StartService();
                }
            }
            else
            {
                Environment.Exit(-99);
            }
        }
    }
}