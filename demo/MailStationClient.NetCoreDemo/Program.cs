using Fanex.Logging;
using Fanex.Logging.Sentry;
using Fanex.MailDelivery.Client;
using Fanex.MailDelivery.Client.Configurations;
using Microsoft.Extensions.Configuration;
using Sentry;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MailStationClient.NetCoreDemo
{
    class Program
    {
        static async Task Main(string[] args)
        {
            RegisterLogging();

            await MailClientHandler.StartAsync();

            Console.ReadLine();
        }

        private static void RegisterLogging()
        {
            LogManager
                .SetDefaultLogCategory("")
                .Use(new List<ILogEngine>
                {
                     new SentryLogging(new SentryEngineOptions {
                        Dsn = new Dsn("https://3714717bde1f487795ca0bdeb6c6da9f@sentry.nexdev.net/98"),
                        Environment = "DEV"
                     })
                });
        }
    }
}
