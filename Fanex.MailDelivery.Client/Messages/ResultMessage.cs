namespace Fanex.MailDelivery.Client.Messages
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Result Response.
    /// </summary>
    public class ResultMessage
    {
        public int ErrCode { get; set; }

        public string ErrMsg { get; set; }
    }
}
