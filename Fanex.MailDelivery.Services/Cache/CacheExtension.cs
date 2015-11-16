// -----------------------------------------------------------------------
// <copyright file="CacheExtension.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;
using System.Linq;
using Fanex.MailDelivery.Services.Models;

namespace Fanex.MailDelivery.Services.Cache
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public static class CacheExtension
    {
        public static List<ClientStatusModel> ScheduleMailList { get; set; }

        public static ClientStatusModel Find(int mailId)
        {
            return ScheduleMailList.Where(m => m.MailId == mailId).FirstOrDefault();
        }
    }
}
