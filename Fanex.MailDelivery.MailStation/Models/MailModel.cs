namespace Fanex.MailDelivery.MailStation.Models
{
    using System;

    public class MailModel
    {
        public int Id;

        public string MailName;

        public int MailGroupID;

        public string MailGroupName;

        public int SiteId;

        public string SiteName;

        public int TriggerType;

        public string TriggerTypeName;

        public string MailTo;

        public string MailCc;

        public string MailBcc;

        public DateTime LastSent;

        public bool IsActive;
    }
}
