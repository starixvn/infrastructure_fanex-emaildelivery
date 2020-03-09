using Fanex.MailDeliver.Model;

namespace Fanex.MailDelivery.Services.Models
{
    public class RetryMailRecord
    {
        public RetryMailRecord(MailRecord mailRecord, int retryCount)
        {
            MailRecord = mailRecord;
            RetryCount = retryCount;
            MaxRetryCount = retryCount;
        }

        public MailRecord MailRecord { get; private set; }

        public int RetryCount { get; set; }

        public int MaxRetryCount { get; private set; }

        public int ResendTime => MaxRetryCount - RetryCount;
    }
}
