using System;
using System.Reflection;
using System.Security;
using System.Security.Permissions;
using System.Security.Policy;
using System.ServiceProcess;
using Fanex.MailStation.Admin.Service.Installer;

namespace Fanex.MailStation.Admin.Service
{
    public class Program
    {
        public static void Main(string[] args)
        {
            ServiceBase[] servicesToRun = new ServiceBase[]
            {
                new AdminServerWinService()
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

                Console.WriteLine("Mail service has been started.");
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