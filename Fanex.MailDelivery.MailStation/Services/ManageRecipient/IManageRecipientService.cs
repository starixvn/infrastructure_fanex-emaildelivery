// --------------------------------------------------------------------------
// <copyright file="IManageRecipientService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Services.ManageRecipient
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.MailStation.Criteria.ManageRecipient;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.NAP.Core.Domain;

    public interface IManageRecipientService
    {
        List<RecipientModel> RecipientList(RecipientListCriteria criteria);

        RecipientModel RecipientById(RecipientByIdCriteria criteria);

        ResultModel EditRecipient(UpdateRecipientCriteria criteria);

        ResultModel InsertRecipient(InsertRecipientCriteria criteria);

        IUser CheckRecipient(CheckRecipientInfoCriteria criteria);
    }
}
