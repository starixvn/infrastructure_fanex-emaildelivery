using System;
using System.Collections.Generic;

namespace Fanex.MailDelivery.Admin.Messaging.ManageMail
{
    public class GetMailListResponse
    {
        public List<GetMailListResponseDto> GetMailListResponseDtos { get; set; }
    }

    public class GetMailListResponseDto
    {
        public int MailId { get; set; }

        public string MailName { get; set; }


        public int MailGroupId { get; set; }

        public string MailGroupName { get; set; }


        public int SiteId { get; set; }

        public string SiteName { get; set; }


        public int TriggerType { get; set; }


        public string MailTo { get; set; }

        public string MailCc { get; set; }

        public string MailBcc { get; set; }


        public DateTime? LastSent { get; set; }

        public bool IsActive { get; set; }
    }
}
