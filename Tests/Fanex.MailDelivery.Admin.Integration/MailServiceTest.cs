using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Services.Mail;
using Fanex.MailDelivery.AdminWinService.Handlers;
using Fanex.MailDelivery.Core.Models;
using NUnit.Framework;

namespace Fanex.MailDelivery.Admin.UserTest
{
    [TestFixture]
    public class MailServiceTest
    {
        [SetUp]
        public void Initial()
        {
            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
        }

        [Test]
        public void GetMailListTest()
        {
            GetMailListRequest getMailListRequest = new GetMailListRequest();

            getMailListRequest.SiteId = 12;

            GetMailListResponse mails;

            var mailService = new MailService();

            mails = mailService.GetMailList(getMailListRequest);
        }

        [Test]
        public void GetMailByIdTest()
        {
            GetMailByIdRequest getMailByIdRequest = new GetMailByIdRequest();

            getMailByIdRequest.Id = 127;

            GetMailByIdResponse mails;

            IMailService mailService = new MailService();
            mails = mailService.GetMailById(getMailByIdRequest);
        }

      
    }

  
}
