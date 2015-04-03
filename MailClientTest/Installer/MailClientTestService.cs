using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Client.Configurations;

namespace MailClientTest.Installer
{
    partial class MailClientTestService : ServiceBase
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
