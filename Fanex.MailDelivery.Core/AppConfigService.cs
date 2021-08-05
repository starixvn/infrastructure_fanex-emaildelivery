using Fanex.MailDelivery.Core.Models;
using SimpleInjector;

namespace Fanex.MailDelivery.Core
{
    public static class AppConfigService
    {
        public static ConfigSettingModel AppConfig { get; set; }

        public static Container Container { get; set; }
    }
}