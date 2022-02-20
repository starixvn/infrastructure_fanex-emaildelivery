using Fanex.MailDelivery.Client;
using Fanex.MailDelivery.Core.Models;
using System;
using System.Configuration;
using System.Threading.Tasks;

namespace MailStationClient.FunctionMailDemo
{
    internal class Program
    {
        protected Program()
        {
        }

        private static async Task Main(string[] args)
        {
            try
            {
                await MailClientHandler.StartAsync();

                var mail = new Mail
                {
                    Subject = "Mail Station Client - Function mail demo",
                    Title = "Function mail demo",
                    Body = "Function mail Body",
                    MailingListId = 3
                };

                if (mail.MailingListId > 0)
                {
                    await MailClientHandler.SendFunctionMailAsync(mail);
                }
            }
            catch (Exception ex)
            {
                var s = ex.Message;
            }
            finally
            {
                await MailClientHandler.StopAsync();
            }
        }
    }
}