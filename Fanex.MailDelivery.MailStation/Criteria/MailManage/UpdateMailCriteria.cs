// --------------------------------------------------------------------------
// <copyright file="UpdateMailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Core.Models.Mails;

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
