// --------------------------------------------------------------------------
// <copyright file="GetAllCategoryCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.MailStation.Criteria.MailManage
{
    using Fanex.NAP.Framework.Data;
    using System;
    using Fanex.MailDelivery.MailStation.Models;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class DeleteMailCriteria : CriteriaBase
    {
        public int MailID { get; set; }

        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_DeleteMail";
        }
    }
}
