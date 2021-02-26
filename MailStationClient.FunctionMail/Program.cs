using Fanex.MailDelivery.Client;
using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.Core.Models;
using System.Threading.Tasks;

namespace MailStationClient.FunctionMailDemo
{
    class Program
    {
        protected Program()
        {
        }

        static async Task Main(string[] args)
        {
            try
            {
                var mailStationSetting = new MailStationSetting();
                await MailClientHandler.StartAsync(mailStationSetting);

                var mail = new Mail
                {
                    Subject = "Mail Station Client - Function mail demo",
                    Title = "Function mail demo",
                    Body = "Function mail Body",
                    MailingListId = mailStationSetting.GetFunctionId("FunctionMail")
                };

                if (mail.MailingListId > 0)
                {
                    await MailClientHandler.SendFunctionMailAsync(mail);
                }
            }
            finally
            {
                await MailClientHandler.StopAsync();
            }
        }
    }
}
