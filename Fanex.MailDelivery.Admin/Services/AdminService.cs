using System;
using EasyNetQ;
using Fanex.MailDelivery.Admin.Configurations;
using Fanex.MailDelivery.Admin.Logging;

namespace Fanex.MailDelivery.Admin.Services
{
    public class AdminService
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
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                // Free managed resources
            }

            // Free native resources if there are any.
            Bus.Dispose();
        }
    }
}