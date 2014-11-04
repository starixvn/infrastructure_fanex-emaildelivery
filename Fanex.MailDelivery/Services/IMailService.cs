// -----------------------------------------------------------------------
// <copyright file="IMailService.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Services
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.NAP.Framework.Command;
using Fanex.MailDelivery.Client.Enumerations;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public interface IMailService
    {
        List<Schedule> GetSchedules(List<int> scheduleId);

        CommandResult UpdateJobStatus(Schedule schedule, JobStatus jobStatus);

        CommandResult SendMail(Mail mail);
    }
}
