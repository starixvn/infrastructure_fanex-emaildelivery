namespace Fanex.MailDelivery.Criteria
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class UpdateTaskRetryCounterCriteria : CriteriaBase
    {
        public int TaskId { get; set; }

        public int RetryCount { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "Mail_Core_UpdateRetry";
        }
    }
}
