using System.Collections.Generic;
using System.Linq;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.AdminWinService.Cache
{
    public static class CacheExtension
    {
        public static List<ClientStatusModel> ScheduleMailList { get; set; }

        public static ClientStatusModel Find(int mailId)
        {
            return ScheduleMailList.FirstOrDefault(m => m.MailId == mailId);
        }
    }
}
