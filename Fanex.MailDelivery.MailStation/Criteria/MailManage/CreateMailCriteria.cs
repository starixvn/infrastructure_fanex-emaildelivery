// --------------------------------------------------------------------------
// <copyright file="CreateMailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    using System;
    using System.Diagnostics.CodeAnalysis;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class CreateMailCriteria : CriteriaBase
    {
        public CreateMailCriteria()
        {
            this.IsActive = true;
        }

        public CreateMailCriteria(MailModel model)
            : this()
        {
            if (model != null)
            {
                this.MailName = model.MailName;
                this.MailGroupId = model.MailGroupId;
                this.SiteId = model.SiteId;
                this.TriggerType = model.TriggerType;
                this.RepeatType = model.RepeatType;
                this.CronExpression = model.CronExpression;
                this.MailFrom = model.MailFrom;
                this.MailTo = model.MailTo;
                this.MailCc = model.MailCc;
                this.MailBcc = model.MailBcc;
                this.IsActive = model.IsActive;
                this.StartOn = model.StartOn;
                this.EndOn = model.EndOn;
                this.Attempt = model.Attempt;
                this.Interval = model.Interval;
                this.Description = model.Description;
            }
            else 
            {
                this.TriggerType = 1;
            }
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
