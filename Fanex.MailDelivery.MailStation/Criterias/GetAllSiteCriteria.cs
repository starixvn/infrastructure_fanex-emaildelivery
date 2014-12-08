// --------------------------------------------------------------------------
// <copyright file="GetAllCategoryCriteria.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------

namespace Fanex.MailDelivery.MailStation.Criterias
{
    using Fanex.NAP.Framework.Data;

    /// <summary>
    /// Get all log category criteria.
    /// </summary>
    public class GetAllSiteCriteria : CriteriaBase
    {
        public override bool IsValid()
        {
            return true;
        }

        public override string GetSettingKey()
        {
            return "MailStation_GetAllSites";
        }
    }
}
