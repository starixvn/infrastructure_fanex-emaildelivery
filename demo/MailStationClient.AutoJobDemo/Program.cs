using Fanex.Logging;
using Fanex.Logging.Sentry.RavenClient;
using SharpRaven;
using System.Collections.Generic;
using System.Configuration;
using System.ServiceProcess;

namespace MailStationClient.AutoJobDemo
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            var category = ConfigurationManager.AppSettings.Get("LogCategory");
            LogManager
                .SetDefaultLogCategory(category)
                .Use(new List<ILogEngine>
                {
                     new SentryLogging(new SentryEngineOptions {
                        Dsn = new Dsn(ConfigurationManager.AppSettings.Get("SentryDsn")),
                        Environment = category
                     })
                });

            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]
            {
                new AutoJobService()
            };
            ServiceBase.Run(ServicesToRun);
        }
    }
}
