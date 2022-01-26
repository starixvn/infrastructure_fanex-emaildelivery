using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Models;
using Newtonsoft.Json;
using SimpleInjector;
using System;
using System.IO;
using MailServiceAppSettings = Fanex.MailDeliver.MailClient.AppSettings;

namespace Fanex.MailDelivery.MailStationService
{
    public static class RegisterService
    {
        public static void RegisterServices()
        {
            LoadConfig();
            DependencyInjection();
        }

        private static void LoadConfig()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);

            var text = File.ReadAllText("Configurations/appsettings.json");
            AppConfigService.AppConfig = JsonConvert.DeserializeObject<ConfigSettingModel>(text);
            MailServiceAppSettings.AppMailId = AppConfigService.AppConfig.AppMailId;
            MailServiceAppSettings.AppMailAddress = AppConfigService.AppConfig.AppMailAddress;
        }

        private static void DependencyInjection()
        {
            var container = new Container();
            AppConfigService.Container = container;
            OnebookUMInitializer.InitializeUMClientManager();
            OnebookUMInitializer.RegisterSimpleInjector(container);
        }
    }
}