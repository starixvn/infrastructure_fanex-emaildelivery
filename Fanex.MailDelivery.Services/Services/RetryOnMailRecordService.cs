using Fanex.MailDeliver.Model;
using Fanex.MailDelivery.Services.Repositories;
using System.Timers;

namespace Fanex.MailDelivery.Services.Services.MailService
{
    public interface RetryOnMailRecordService
    {
        void Retry(int mailId);
    }

    public class RetryOnMailRecordServiceImp : RetryOnMailRecordService
    {
        private Timer timer;
        private int retryCounter;
        private readonly int sendMailRetryLimit;
        private readonly IMailRepository mailRepository;

        public RetryOnMailRecordServiceImp(IMailRepository mailRepository, int sendMailRetryLimit)
        {
            this.mailRepository = mailRepository;
            this.sendMailRetryLimit = sendMailRetryLimit;
        }

        public void Retry(int mailId)
        {
            retryCounter = 0;
            timer = new Timer();
            timer.Interval = 1000 * 5;
            timer.Elapsed += (sender, e) =>
            {
                Execute(mailId);
            };

            timer.Start();
        }

        private void Execute(int mailId)
        {
            var mailRecord = mailRepository.GetMailRecordById(mailId);
            var isNeedToRetry = IsNeedToRetry(mailRecord);

            try
            {
                if (retryCounter < sendMailRetryLimit && isNeedToRetry)
                {
                    ResendMail(mailId);
                }
            }
            finally
            {
                if (retryCounter >= sendMailRetryLimit || !isNeedToRetry)
                {
                    timer.Stop();
                    timer.Dispose();
                }

                retryCounter++;
            }
        }

        private bool IsNeedToRetry(MailRecord mailRecord)
        {
            return mailRecord.SentSuccess == 0 && mailRecord.SentFail > 0;
        }

        private void ResendMail(int mailId)
        {
            using (var client = new MailDeliver.MailClient.API.MailDeliver())
            {
                client.ReSendMail(mailId);
            }
        }
    }
}