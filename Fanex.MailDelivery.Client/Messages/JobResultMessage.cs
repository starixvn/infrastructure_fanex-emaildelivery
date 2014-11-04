namespace Fanex.MailDelivery.Client.Messages
{
    using Fanex.MailDelivery.Client.Enumerations;
    using Fanex.MailDelivery.Client.Models;

    /// <summary>
    /// Job Status Message.
    /// </summary>
    public class JobResultMessage
    {
        public Schedule Schedule { get; set; }

        public Mail Mail { get; set; }

        public JobStatus JobStatus { get; set; }

        public ResultMessage ResultMessage { get; set; }

        public int RunningTime { get; set; }
    }
}