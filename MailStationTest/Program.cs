using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using SharpRaven;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.ServiceProcess;

namespace MailStationClientTest
{
    public class Program
    {
        protected Program()
        {
        }

        public static void Main(string[] args)
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

            ServiceBase[] servicesToRun = new ServiceBase[]
            {
                new MailStationTestService()
            };

            if (Environment.UserInteractive)
            {
                Type type = typeof(ServiceBase);
                BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;

                MethodInfo method = type.GetMethod("OnStart", flags);
                foreach (ServiceBase service in servicesToRun)
                {
                    method.Invoke(service, new object[] { args });
                }

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
