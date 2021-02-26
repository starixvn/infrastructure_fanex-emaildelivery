using Fanex.MailDelivery.Client;
using Fanex.MailDelivery.Client.Configurations;
using System.ServiceProcess;

namespace MailStationClient.AutoJobDemo
{
    public partial class AutoJobService : ServiceBase
    {
        public AutoJobService()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            MailClientHandler.Start(new MailStationSetting());
        }

        protected override void OnStop()
        {
            MailClientHandler.Stop();
        }
    }
}
