using EasyNetQ;
using Fanex.MailDelivery.Admin.Configurations;

namespace Fanex.MailDelivery.Admin.Services
{
    public class AdminService
    {
        protected static readonly IBus Bus = RabbitHutch.CreateBus(
                AppSettings.MailAdminConfig.RabbitMQConnectionString,
                serviceRegister => {
                    //serviceRegister.Register<IEasyNetQLogger>(_ => new MailAdminLogger());
                });

        public AdminService()
        {
        }
    }
}