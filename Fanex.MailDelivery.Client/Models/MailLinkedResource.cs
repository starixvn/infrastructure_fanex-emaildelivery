// -----------------------------------------------------------------------
// <copyright file="MailLinkedResource.cs" company="">
// TODO: Update copyright text.
// </copyright>
// -----------------------------------------------------------------------

namespace Fanex.MailDelivery.Client.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Fanex.MailDelivery.Client.Enumerations;

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
