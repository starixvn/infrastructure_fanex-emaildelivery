using ONEbook.UM.Configurations;

namespace Fanex.MailDelivery.Core.Models
{
    public class ConfigSettingModel
    {
        public UMClientConfiguration UMConfig { get; set; }

        public string AppMailId { get; set; }

        public string AppMailAddress { get; set; }
    }
}