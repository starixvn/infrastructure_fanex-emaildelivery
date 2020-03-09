using System;
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
