using Fanex.MailDelivery.Client;
using Fanex.MailDelivery.Client.Configurations;
using Fanex.MailDelivery.Core.Models;

namespace MailStationClient.FunctionMailDemo
{
    class Program
    {
        protected Program()
        {
        }

        static void Main(string[] args)
        {
            try
            {
                MailClientHandler.Start();

                var mail = new Mail
                {
                    Subject = "Mail Station Client - Function mail demo",
                    Title = "Function mail demo",
                    Body = "Function mail Body",
                    MailingListId = AppSettings.GetFunctionId("FunctionMail")
                };

                if (mail.MailingListId > 0)
                {
                    MailClientHandler.SendFunctionMail(mail);
                }
            }
            finally
            {
                MailClientHandler.Stop();
            }
        }
    }
}
