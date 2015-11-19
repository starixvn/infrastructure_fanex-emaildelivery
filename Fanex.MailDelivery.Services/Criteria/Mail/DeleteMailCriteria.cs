// --------------------------------------------------------------------------
// <copyright file="DeleteMailCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

using Fanex.Data.Repository;

namespace Fanex.MailDelivery.Services.Criteria.Mail
{
    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class DeleteMailCriteria : CriteriaBase
    {
        public int MailId { get; set; }

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
