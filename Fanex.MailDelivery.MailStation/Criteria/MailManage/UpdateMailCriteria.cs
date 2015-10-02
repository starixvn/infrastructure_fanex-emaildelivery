// --------------------------------------------------------------------------
// <copyright file="UpdateMailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    using Fanex.MailDelivery.MailStation.Models;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class UpdateMailCriteria : CreateMailCriteria
    {
        public UpdateMailCriteria()
            : base()
        {
        }

        public UpdateMailCriteria(MailModel model)
            : base(model)
        {
            if (model != null)
            {
                this.MailId = model.MailId;
            }
        }

        public int MailId { get; set; }

        public override string GetSettingKey()
        {
            return "MailStation_EditMail";
        }
    }
}
