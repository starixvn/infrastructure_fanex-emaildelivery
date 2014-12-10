namespace Fanex.MailDelivery.MailStation.Models
{
    using System;

    public class MailModel
    {
        public int MailId;

        public string MailName;

        public int MailGroupID;

        public string MailGroupName;

        public int SiteId;

        public string SiteName;

        public int TriggerType;

        public string TriggerTypeName
        {
            get 
            {
                switch (this.TriggerType) { 
                    case 0:
                        return "Function";
                    case 1:
                        return "Schedule";
                    default:
                        return "Unknow";
                }
            }
        }

        public string CronExpression;

        public string MailFrom;

        public string MailTo;

        public string MailCc;

        public string MailBcc;

        public bool IsActive;

        public DateTime StartOn;

        public DateTime EndOn;

        public int Attempt;

        public int Interval;

        public DateTime LastSent;

        public string Description;
    }
}
