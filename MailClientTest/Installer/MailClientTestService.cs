namespace MailClientTest.Installer
{
    using System.ServiceProcess;
    using Fanex.MailDelivery.Client.Handlers;

    /// <summary>
    /// Mail client service for testing.
    /// </summary>
    public partial class MailClientTestService : ServiceBase
    {
        public MailClientTestService()
        {
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
