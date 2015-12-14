using Fanex.MailDelivery.Client;
using Fanex.MailDelivery.Client.Configurations;

namespace MailClientTest
{
    using System;
    using System.Reflection;
    using System.ServiceProcess;
    using Fanex.MailDelivery.Client.Handlers;
    using MailClientTest.Installer;

    public class Program
    {
        public static void Main(string[] args)
        {
            MailClientHandler.Start();

            //Console.WriteLine("Mail Client Service Site: {0} started.", AppSettings.MailCientConfig.SiteId);
            //Type type = typeof(MailClientTestService);
            //MethodInfo method = type.GetMethod("SendFunctionMail", BindingFlags.Instance | BindingFlags.NonPublic);
            //method.Invoke(new MailClientTestService(), null);


            //ServiceBase[] servicesToRun = new ServiceBase[]
            //{
            //    new MailClientTestService()
            //};

            //if (Environment.UserInteractive)
            //{
            //    Type type = typeof(ServiceBase);
            //    BindingFlags flags = BindingFlags.Instance | BindingFlags.NonPublic;

            //    MethodInfo method = type.GetMethod("OnStart", flags);
            //    foreach (ServiceBase service in servicesToRun)
            //    {
            //        method.Invoke(service, new object[] { args });
            //    }

            //    foreach (ServiceBase service in servicesToRun)
            //    {
            //        service.Stop();
            //    }
            //}
            //else
            //{
            //    ServiceBase.Run(servicesToRun);
            //}


            Console.WriteLine("Wait!");
            Console.ReadLine();

            MailClientHandler.Stop();
        }
    }
}
