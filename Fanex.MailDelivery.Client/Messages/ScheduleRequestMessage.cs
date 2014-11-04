// -----------------------------------------------------------------------
// <copyright file="ScheduleRequestMessage.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Client.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class ScheduleRequestMessage
    {
        public ScheduleRequestMessage()
        {
            Ids = new List<int>();
        }

        public List<int> Ids { get; set; }
    }
}
