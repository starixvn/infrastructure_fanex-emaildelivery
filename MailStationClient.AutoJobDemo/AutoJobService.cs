using Fanex.MailDelivery.Client;
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
            MailClientHandler.Start();
        }

        protected override void OnStop()
        {
            MailClientHandler.Stop();
        }
    }
}
