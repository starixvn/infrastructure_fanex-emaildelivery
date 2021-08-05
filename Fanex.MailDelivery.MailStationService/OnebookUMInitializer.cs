using Fanex.Caching;
using Fanex.MailDelivery.Core;
using ONEbook.UM;
using ONEbook.UM.Helpers;
using ONEbook.UM.Services;
using ONEbook.UM.Transport;
using SimpleInjector;

namespace Fanex.MailDelivery.MailStationService
{
    public static class OnebookUMInitializer
    {
        public static void InitializeUMClientManager()
        {
            var umConfig = AppConfigService.AppConfig.UMConfig;

            UMClientManager
              .UseConfig(umConfig)
              .UseCacheService(new UMCacheService(new CacheService()))
              .UseRestClient(new UMRestClient(umConfig));
        }

        public static void RegisterSimpleInjector(Container container)
        {
            var umConfig = AppConfigService.AppConfig.UMConfig;
            container.RegisterInstance(umConfig);
            container.RegisterSingleton<IHttpContextHelper, HttpContextHelper>();
            container.RegisterInstance<ICacheService>(new CacheService());
            container.RegisterSingleton<IUMCacheService, UMCacheService>();
            container.RegisterSingleton<IPHelper, IPHelperImpl>();
            container.RegisterSingleton<IUMRestClient, UMRestClient>();
            container.RegisterSingleton<IMaintenanceService, MaintenanceService>();
        }
    }

    public class HttpContextHelper : IHttpContextHelper
    {
        public string GetSession(string key)
        {
            return string.Empty;
        }

        public string GetValueFromQueryString(string queryKey)
        {
            return string.Empty;
        }

        public void SetSession(string key, string value)
        {
        }
    }
}