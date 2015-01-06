// --------------------------------------------------------------------------
// <copyright file="MailLogModel.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2014.10.03">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Models
{
    /// <summary>
    /// Mail Log Model.
    /// </summary>
    public class MailLogModel
    {
        public int LogId { get; set; }

        public int TaskId { get; set; }

        public int Status { get; set; }

        public string ExceptionMessage { get; set; }

        public string LogTime { get; set; }

        public string StackTrace { get; set; }
    }
}
