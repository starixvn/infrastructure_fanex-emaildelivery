using Fanex.MailDelivery.Client;
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
                await MailClientHandler.StartAsync();

                var mail = new Mail
                {
                    Subject = "Mail Station Client - Function mail demo",
                    Title = "Function mail demo",
                    Body = "Function mail Body",
                    MailingListId = MailClientHandler.MailStationSetting.GetFunctionId("FunctionMail")
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
