// -----------------------------------------------------------------------
// <copyright file="UpdateMailCriteria.cs" company="Microsoft">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Server.Criteria.Mail
{
    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class UpdateMailCriteria : CreateMailCriteria
    {
        public UpdateMailCriteria()
            : base()
        {
        }

        public int MailId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_EditMail";
        }
    }
}
