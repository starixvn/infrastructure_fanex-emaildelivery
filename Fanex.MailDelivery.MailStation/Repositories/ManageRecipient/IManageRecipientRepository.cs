// --------------------------------------------------------------------------
// <copyright file="IManageRecipientRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Repositories.ManageRecipient
{
    using System.Collections.Generic;
    using Fanex.MailDelivery.MailStation.Criteria.ManageRecipient;
    using Fanex.MailDelivery.MailStation.Models;

    public interface IManageRecipientRepository
    {
        List<RecipientModel> RecipientList(RecipientListCriteria criteria);

        RecipientModel RecipientById(RecipientByIdCriteria criteria);

        ResultModel EditRecipient(UpdateRecipientCriteria criteria);

        ResultModel InsertRecipient(InsertRecipientCriteria criteria);

        RecipientModel CheckRecipient(CheckRecipientInfoCriteria criteria);
    }
}
