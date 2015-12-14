using System;
using System.Reflection;
using System.ServiceProcess;
using Fanex.MailDelivery.ClientWinService.Installer;

namespace Fanex.MailDelivery.ClientWinService
{
    public static class Program
    {
        /// <summary>
        /// Mains the specified arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        public static void Main(string[] args)
        {
            ServiceBase[] servicesToRun = new ServiceBase[]
            {
                new ClientServerWinService()
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

                Console.WriteLine("Client Server Service has been started.");
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
