// --------------------------------------------------------------------------
// <copyright file="TaskStatusConstants.cs" company="Nexcel Solutions Vietnam">
// Copyright (c) Nexcel Solutions Vietnam. All rights reserved.
// </copyright>
// --------------------------------------------------------------------------
// <history>
//   <change who="Louis.Nguyen" date="2012.12.07">Created</change>
// </history>

namespace Fanex.MailDelivery.MailStation.Constants
{
    using Fanex.MailDelivery.MailStation.Resources;

    /// <summary>
    /// Task Status Constants.
    /// </summary>
    public static class TaskStatusConstants
    {
        public const int All = -1;

        public const int Waiting = 0;

        public const int Running = 1;

        public const int Sent = 2;

        public const int DataNotReady = 3;

        public const int Failed = 4;

        /// <summary>
        /// Gets the status name.
        /// </summary>
        /// <param name="statusId">The status identifier.</param>
        /// <returns>The status name.</returns>
        public static string StatusName(int statusId)
        {
            switch (statusId)
            {
                case All:
                    return Labels.All;
                case Waiting:
                    return Labels.Waiting.ToUpper();
                case Running:
                    return Labels.Running.ToUpper();
                case Sent:
                    return Labels.Sent.ToUpper();
                case DataNotReady:
                    return Labels.Data_not_ready.ToUpper();
                case Failed:
                    return Labels.Failed.ToUpper();
                default:
                    return string.Empty;
            }
        }
    }
}
