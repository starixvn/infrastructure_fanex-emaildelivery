using Fanex.Data.Configuration;
using Fanex.MailDelivery.MailStationService.MessageBrokers;
using Fanex.MailDelivery.Services.Services.Logging;
using System;
using System.ServiceProcess;

namespace Fanex.MailDelivery.MailStationService
{
    partial class MainService : ServiceBase
    {
        private IClientServerMessageBroker messageBroker;
        private int startCounter = 0;

        public MainService()
            : this(null)
        {
        }

        public MainService(IClientServerMessageBroker messageBroker)
        {
            InitializeComponent();

            this.messageBroker = messageBroker ?? new ClientServerMessageBroker();
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

            if (startCounter <= AppSettings.StartLimitTime)
            {
                try
                {
                    messageBroker.Start();

                    startCounter = 0;
                }
                catch (Exception exception)
                {
                    LogService.LogError(exception);

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