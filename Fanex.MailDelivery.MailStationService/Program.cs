using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using SharpRaven;
using System;
using System.Collections.Generic;
using System.Net;
using System.Reflection;
using System.ServiceProcess;

namespace Fanex.MailDelivery.MailStationService
{
    public static class Program
    {
        public static void Main(string[] args)
        {
            ServicePointManager.Expect100Continue = true;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls
                   | SecurityProtocolType.Tls11
                   | SecurityProtocolType.Tls12
                   | SecurityProtocolType.Ssl3;

            LogManager
                .SetDefaultLogCategory(AppSettings.LogCategory)
                .Use(new List<ILogEngine>
                {
                     new SentryLogging(new SentryEngineOptions {
                        Dsn = new Dsn(AppSettings.SentryDsn),
                        Environment = AppSettings.LogCategory
                     })
                });

            var servicesToRun = new ServiceBase[]
             {
                new MainService()
             };

            if (Environment.UserInteractive)
            {
                Type type = typeof(ServiceBase);
                const BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;
                MethodInfo method = type.GetMethod("OnStart", flags);

                foreach (ServiceBase service in servicesToRun)
                {
                    method.Invoke(service, new object[] { args });
                }

                Console.WriteLine("Mail Station Service has been started.");
                Console.WriteLine("Press any key to exit!");
                Console.ReadLine();

                foreach (ServiceBase service in servicesToRun)
                {
                    service.Stop();
                }
            }
            else
            {
                ServiceBase.Run(servicesToRun);
            }
        }
    }
}