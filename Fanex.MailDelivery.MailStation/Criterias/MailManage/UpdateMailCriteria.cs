// --------------------------------------------------------------------------
// <copyright file="UpdateMailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.MailStation.Criterias.MailManage
{
    using Fanex.NAP.Framework.Data;
    using System;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.MailDelivery.MailStation.Criterias.MailManage;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class UpdateMailCriteria : CreateMailCriteria
    {
        public int MailID { get; set; }

        public UpdateMailCriteria()
            : base()
        {
        }

        public UpdateMailCriteria(MailModel model)
            : base(model)
        {
            if (model != null)
            {
                this.MailID = model.MailId;
            }
        }

        public override string GetSettingKey()
        {
            return "MailStation_EditMail";
        }
    }
}
