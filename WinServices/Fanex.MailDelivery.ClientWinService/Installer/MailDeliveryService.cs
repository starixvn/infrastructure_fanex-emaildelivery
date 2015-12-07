using System.Diagnostics.CodeAnalysis;
using System.ServiceProcess;

namespace Fanex.MailDelivery.ClientWinService.Installer
{
    /// <summary>
    /// Mail Centralized Service.
    /// </summary>
    [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1400:AccessModifierMustBeDeclared", Justification = "Reviewed.")]
    partial class ClientWinService : ServiceBase
    {

        private string serviceInfo = "Service {0} {1} at {2}.";

        public ClientWinService()
        {
        }

        protected override void OnStart(string[] args)
        {
         
        }

        protected override void OnStop()
        {
          
        }
    }
}