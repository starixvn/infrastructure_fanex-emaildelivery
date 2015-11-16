// -----------------------------------------------------------------------
// <copyright file="MailLinkedResource.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

using Fanex.MailDelivery.Core.Enumerations;
using Fanex.MailDelivery.Core.Models;

namespace Fanex.MailDelivery.Client.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class MailLinkedResource : MailAttachment
    {
        public MailLinkedResource()
        { 
        }

        public MailLinkedResource(string filePath, FileExtension fileExtension, string contentId) 
            : base(filePath, fileExtension)
        {
            ContentId = contentId;
        }

        public MailLinkedResource(string fileName, byte[] fileContent, FileExtension fileExtension, string contentId)
            : base(fileName, fileContent, fileExtension)
        {
            ContentId = contentId;
        }

        public string ContentId { get; set; }
    }
}
