// -----------------------------------------------------------------------
// <copyright file="ClientStatusModel.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System;

namespace Fanex.MailDelivery.Services.Models
{
    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ClientStatusModel
    {
        public int MailId { get; set; }

        public string MailName { get; set; }

        public string CronExpression { get; set; }

        public DateTime? LastActive { get; set; }

        public bool IsOnline { get; set; }
    }
}
