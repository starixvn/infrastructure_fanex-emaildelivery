﻿// --------------------------------------------------------------------------
// <copyright file="ManageRecipientService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Fanex.Data;
using Fanex.MailDelivery.MailStation.Criteria.ManageRecipient;
using Fanex.MailDelivery.MailStation.Models;
using Fanex.NAP.Core.Criteria.Misc;
using Fanex.NAP.Core.Domain;
using Fanex.NAP.Core.Service.Impl;
using Fanex.NAP.Core.Service.Interface;
using Fanex.NAP.Framework.Command;

namespace Fanex.MailDelivery.MailStation.Services
{
    /// <summary>
    /// Mail Recipient Service.
    /// </summary>
    public class ManageRecipientService : MailStationBaseService
    {
        private IUserService _userService;

        public ManageRecipientService()
        {
            this._userService = this._userService ?? new UserService();
        }

        /// <summary>
        /// Gets the recipient list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The recipient list.</returns>
        public List<RecipientModel> RecipientList(RecipientListCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<RecipientModel>().ToList();
            }
        }

        /// <summary>
        /// Gets the recipient by identity.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The recipient by identity.</returns>
        public RecipientModel RecipientById(RecipientByIdCriteria criteria)
        {
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.AssignParamValues(criteria.GetSpParams());
                return db.Query<RecipientModel>().FirstOrDefault();
            }
        }

        /// <summary>
        /// Edits the recipient.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Editing result.</returns>
        public CommandResult EditRecipient(UpdateRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Inserts the recipient.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Inserting result.</returns>
        public CommandResult InsertRecipient(CreateRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Deletes the recipient.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Command result.</returns>
        public CommandResult DeleteRecipient(DeleteRecipientCriteria criteria)
        {
            Hashtable hash = criteria.GetSpParams();
            hash = (new ResultCriteria()).AddSpParams(hash);
            using (var db = new ObjectDb(criteria.GetSettingKey()))
            {
                db.ExecuteNonQuery(hash);
                return GetCommandResult(hash);
            }
        }

        /// <summary>
        /// Checks the recipient information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The recipient information.</returns>
        public IUser CheckRecipient(CheckRecipientInfoCriteria criteria)
        {
            return _userService.GetByUsername(criteria.Username);
        }
    }
}