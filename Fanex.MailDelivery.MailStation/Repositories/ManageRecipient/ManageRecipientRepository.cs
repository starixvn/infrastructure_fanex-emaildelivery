// --------------------------------------------------------------------------
// <copyright file="ManageRecipientRepository.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Repositories.ManageRecipient
{
    using System.Collections.Generic;
    using Fanex.Common;
    using Fanex.Data;
    using Fanex.MailDelivery.MailStation.Criteria.ManageRecipient;
    using Fanex.MailDelivery.MailStation.Models;

    /// <summary>
    /// Manage Recipient Repository.
    /// </summary>
    public class ManageRecipientRepository : IManageRecipientRepository
    {
        /// <summary>
        /// Gets the recipient list.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The recipient list.</returns>
        public List<RecipientModel> RecipientList(RecipientListCriteria criteria)
        {
            List<RecipientModel> result = new List<RecipientModel>();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    var recipientModel = new RecipientModel
                    {
                        Id = Converter.ToInt(reader["RecipientID"]),
                        Username = Converter.ToString2(reader["RecipientName"]),
                        IsNapUser = Converter.ToBool(reader["IsNAPUser"]),
                        IsActive = Converter.ToBool(reader["IsActive"]),

                        Email1 = Converter.ToString2(reader["Email1"]),
                        Email2 = Converter.ToString2(reader["Email2"]),
                        Email3 = Converter.ToString2(reader["Email3"]),

                        NumberRelevantEmail = Converter.ToInt(reader["MailRelevant"])
                    };

                    result.Add(recipientModel);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the recipient by identity.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The recipient by identity.</returns>
        public RecipientModel RecipientById(RecipientByIdCriteria criteria)
        {
            RecipientModel result = new RecipientModel();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    result = new RecipientModel
                    {
                        Id = Converter.ToInt(reader["RecipientID"]),
                        Username = Converter.ToString2(reader["RecipientName"]),
                        IsActive = Converter.ToBool(reader["IsActive"]),

                        Email1 = Converter.ToString2(reader["Email1"]),
                        Email2 = Converter.ToString2(reader["Email2"]),
                        Email3 = Converter.ToString2(reader["Email3"]),

                        // Note = Converter.ToString2(reader["Note"])
                    };
                }
            }

            return result;
        }

        /// <summary>
        /// Edits the recipient.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Editing result.</returns>
        public ResultModel EditRecipient(UpdateRecipientCriteria criteria)
        {
            ResultModel result = new ResultModel();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                objDb.ExecuteNonQuery();
                var outputParams = objDb.GetOuputParams();
                result = new ResultModel
                {
                    Code = Converter.ToInt(outputParams["ErrCode"]),
                    Message = Converter.ToString2(outputParams["ErrMessage"])
                };
            }

            return result;
        }

        /// <summary>
        /// Inserts the recipient.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>Inserting result.</returns>
        public ResultModel InsertRecipient(InsertRecipientCriteria criteria)
        {
            ResultModel result = new ResultModel();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                objDb.ExecuteNonQuery();
                var outputParams = objDb.GetOuputParams();
                result = new ResultModel
                {
                    Code = Converter.ToInt(outputParams["ErrCode"]),
                    Message = Converter.ToString2(outputParams["ErrMessage"])
                };
            }

            return result;
        }

        /// <summary>
        /// Checks the recipient information.
        /// </summary>
        /// <param name="criteria">The criteria.</param>
        /// <returns>The recipient information.</returns>
        public RecipientModel CheckRecipient(CheckRecipientInfoCriteria criteria)
        {
            RecipientModel result = new RecipientModel();
            using (var objDb = new ObjectDb(criteria.GetSettingKey()))
            {
                objDb.AssignParamValues(criteria.GetSpParams());
                var reader = objDb.ExecuteReader();
                while (reader.Read())
                {
                    result = new RecipientModel
                    {
                        Id = Converter.ToInt(reader["RecipientID"]),
                        Username = Converter.ToString2(reader["RecipientName"]),
                        IsActive = Converter.ToBool(reader["IsActive"]),

                        Email1 = Converter.ToString2(reader["Email1"]),
                        Email2 = Converter.ToString2(reader["Email2"]),
                        Email3 = Converter.ToString2(reader["Email3"]),

                        Note = Converter.ToString2(reader["Note"])
                    };
                }
            }

            return result;
        }
    }
}
