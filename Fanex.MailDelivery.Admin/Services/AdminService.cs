#region

using System;
using EasyNetQ;
using Fanex.MailDelivery.Admin.Configurations;
using Fanex.MailDelivery.Admin.Logging;

#endregion

namespace Fanex.MailDelivery.Admin.Services
{
    public class AdminService : IDisposable
    {
        protected readonly IBus Bus;

        public AdminService()
        {
            Bus = RabbitHutch.CreateBus(
                AppSettings.MailAdminConfig.RabbitMQConnectionString,
                serviceRegister => { serviceRegister.Register<IEasyNetQLogger>(_ => new MailAdminLogger()); });
        }

        public void Dispose()
        {
            Bus.Dispose();
        }
    }
}