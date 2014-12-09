// --------------------------------------------------------------------------
// <copyright file="ManageRecipientService.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Services.ManageRecipient
{
    using System;
    using System.Collections.Generic;
    using Fanex.MailDelivery.MailStation.Constants;
    using Fanex.MailDelivery.MailStation.Criteria.ManageRecipient;
    using Fanex.MailDelivery.MailStation.Models;
    using Fanex.MailDelivery.MailStation.Repositories.ManageRecipient;
    using Fanex.MailDelivery.MailStation.Resources;
    using Fanex.NAP.Core.Domain;
    using Fanex.NAP.Core.Service.Impl;
    using Fanex.NAP.Core.Service.Interface;

    /// <summary>
    /// Mail Recipient Service.
    /// </summary>
    public class ManageRecipientService : IManageRecipientService
    {
        private IManageRecipientRepository _manageRecipientRepository;

        private IUserService _userService;

        public ManageRecipientService()
            : this(null, null)
        {
        }

        public ManageRecipientService(
            IManageRecipientRepository manageRecipientRepository,
            IUserService userService)
        {
            _manageRecipientRepository = manageRecipientRepository ?? new ManageRecipientRepository();
            _userService = userService ?? new UserService();
        }

        /// <summary>
        /// Gets the recipient list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The recipient list.</returns>
        public List<RecipientModel> RecipientList(RecipientListCriteria criteria)
        {
            try
            {
                return _manageRecipientRepository.RecipientList(criteria);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                throw;
            }
        }

        /// <summary>
        /// Gets the recipient by identity.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The recipient by identity.</returns>
        public RecipientModel RecipientById(RecipientByIdCriteria criteria)
        {
            try
            {
                return _manageRecipientRepository.RecipientById(criteria);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                throw;
            }
        }

        /// <summary>
        /// Edits the recipient.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Editing result.</returns>
        public ResultModel EditRecipient(UpdateRecipientCriteria criteria)
        {
            try
            {
                var editResult = _manageRecipientRepository.EditRecipient(criteria);
                if (editResult.Code == ErrorCodeConstants.NoneError)
                {
                    editResult.Message = Labels.EditRecipient_Success;
                }

                return editResult;
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                throw;
            }
        }

        /// <summary>
        /// Inserts the recipient.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Inserting result.</returns>
        public ResultModel InsertRecipient(InsertRecipientCriteria criteria)
        {
            try
            {
                var insertResult = _manageRecipientRepository.InsertRecipient(criteria);
                if (insertResult.Code == ErrorCodeConstants.NoneError)
                {
                    insertResult.Message = Labels.InsertRecipient_Success;
                }

                return insertResult;
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                throw;
            }
        }

        /// <summary>
        /// Checks the recipient information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The recipient information.</returns>
        public IUser CheckRecipient(CheckRecipientInfoCriteria criteria)
        {
            try
            {
                return _userService.GetByUsername(criteria.Username);
            }
            catch (Exception ex)
            {
                LoggingHandler.Log(ex);
                throw;
            }
        }
    }
}
