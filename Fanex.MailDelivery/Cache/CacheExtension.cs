// -----------------------------------------------------------------------
// <copyright file="CacheExtension.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Cache
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
using Fanex.MailDelivery.Models;

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
