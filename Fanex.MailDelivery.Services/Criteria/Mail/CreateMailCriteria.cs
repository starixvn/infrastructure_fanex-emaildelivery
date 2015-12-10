using System;
using System.Diagnostics.CodeAnalysis;
using Fanex.Data.Repository;

namespace Fanex.MailDelivery.Services.Criteria.Mail
{
    public class CreateMailCriteria : CriteriaBase
    {
        public CreateMailCriteria()
        {
            this.IsActive = true;
        }

        public string MailName { get; set; }

        public int MailGroupId { get; set; }

        public int SiteId { get; set; }

        public int TriggerType { get; set; }

        public int RepeatType { get; set; }

        public string CronExpression { get; set; }

        public string MailFrom { get; set; }

        public string MailTo { get; set; }

        [SuppressMessage("Microsoft.Naming", "CA1709:IdentifiersShouldBeCasedCorrectly", Justification = "It's ok here.")]
        public string MailCc { get; set; }

        public string MailBcc { get; set; }

        public bool IsActive { get; set; }

        public DateTime? StartOn { get; set; }

        public DateTime? EndOn { get; set; }

        public int Attempt { get; set; }

        public int Interval { get; set; }

        public string Description { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_CreateMail";
        }
    }
}
