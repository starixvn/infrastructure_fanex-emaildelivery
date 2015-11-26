// -----------------------------------------------------------------------
// <copyright file="ScheduleRequestMessage.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.Generic;

namespace Fanex.MailDelivery.Core.Messaging
{
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
