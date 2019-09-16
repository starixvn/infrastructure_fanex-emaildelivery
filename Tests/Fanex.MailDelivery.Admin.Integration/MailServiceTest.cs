using System;
using System.Collections.Generic;
using System.IO;
using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Services.Mail;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailStation.Admin.Service.Handlers;
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

        [Test]
        public void TestRazor()
        {
            var service = new CheckClientOnlineStatusJob();
            var list = new List<ClientStatusModel>
            {
                new ClientStatusModel { MailName = "test1" },
                new ClientStatusModel { MailName = "test2" },
            };
        }
    }
}