namespace Fanex.MailDelivery.Client.Messages
{
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// Job Status ErrMsg.
    /// </summary>
    public class JobResultMessage
    {
        public Schedule Schedule { get; set; }

        public Mail Mail { get; set; }

        public JobStatus JobStatus { get; set; }

        public ResultMessage Result { get; set; }

        public int RunningTime { get; set; }
    }
}