using Fanex.MailDelivery.Admin.Messaging.ManageMail;
using Fanex.MailDelivery.Admin.Services.Mail;
using NUnit.Framework;

namespace Fanex.MailDelivery.Admin.UserTest
{
    [TestFixture]
    public class MailServiceTest
    {
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

    /*
    [Test]
    public void CreateFunctionMailTest()
    {
        // GroupIds: Mail_Group_GetMailGroup
        // SiteId
        // MailTo: Mail_MailManagement_GetSuggesionEmailList
        CreateFunctionMailRequest request = new CreateFunctionMailRequest
        {
            GroupIds = 1,
            MailName = "CreateFunctionMailTest",
            SiteId = 12,

            MailFrom = "ddfgdg",
            MailTo = "17",

            Status = true,

            Description = "CreateFunctionMailTest Description"
        };

        ResultResponse response = MailAdminService.CreateFunctionMail(request);

    }

    [Test]
    public void CreateAutoMailTest()
    {
        // GroupIds: Mail_Group_GetMailGroup
        // SiteId
        // MailTo: Mail_MailManagement_GetSuggesionEmailList
        CreateAutoMailRequest request = new CreateAutoMailRequest
        {
            GroupIds = 1,
            MailName = "CreateAutoMailTest",
            SiteId = 12,

            MailFrom = "ddfgdg",
            MailTo = "17",

            Status = true,

            Description = "CreateFunctionMailTest Description",

            //RepeatType =  ,
            CronExpression = "0 46 5 12 * ?"
            //StartOn =  ,
            //EndOn =  ,
            //Attempt =  ,
            //Interval =  ,
        };

        ResultResponse response = MailAdminService.CreateAutoMail(request);
    }

    [Test]
    public void UpdateFunctionMailTest()
    {
        UpdateFunctionMailRequest request = new UpdateFunctionMailRequest
        {
            MailId = 122,

            GroupIds = 1,
            MailName = "CreateFunctionMailTest_Update1",
            SiteId = 12,

            MailFrom = "ddfgdg",
            MailTo = "17",

            Status = true,

            Description = "CreateFunctionMailTest Description",
        };

        ResultResponse response = MailAdminService.UpdateFunctionMail(request);
    }

    [Test]
    public void UpdateAutoMailTest()
    {
        UpdateAutoMailRequest request = new UpdateAutoMailRequest
        {
            MailId = 123,

            GroupIds = 1,
            MailName = "CreateAutoMailTest_Update2",
            SiteId = 12,

            MailFrom = "ddfgdg",
            MailTo = "17",

            Status = true,

            Description = "CreateFunctionMailTest Description",
        };

        ResultResponse response = MailAdminService.UpdateAutoMail(request);
    }

    [Test]
    public void DeleteMailTest()
    {
        DeleteMailRequest request = new DeleteMailRequest
        {
            MailId = 123
        };

        ResultResponse response = MailAdminService.DeleteMail(request);
    }


    [Test]
    public void GetRecipientListTest()
    {
        GetRecipientListRequest request = new GetRecipientListRequest
        {
            Emails = "LongTest2_To@gmail.com",
            Status = "-1"
        };

        GetRecipientListResponse response;

        using (IRecipientService recipientService = new RecipientService())
        {
            response = recipientService.GetRecipientList(request);
        }
    }

    [Test]
    public void CreateRecipienttTest()
    {
        CreateRecipientRequest request = new CreateRecipientRequest
        {
            UserName = "GeorgeTest",
            Email1 = "GeorgeTest@yahoo.com",
            Status = true,
            Note = "Note"
        };

        ResultResponse response = MailAdminService.CreateRecipient(request);
    }

    [Test]
    public void UpdateRecipienttTest()
    {
        //UpdateRecipientRequest request = new UpdateRecipientRequest
        //{
        //    RecipientId = 59
        //};

        //ResultResponse response = MailAdminService.DeleteRecipient(request);
    }

    [Test]
    public void DeleteRecipienttTest()
    {
        DeleteRecipientRequest request = new DeleteRecipientRequest
        {
            RecipientId = 59
        };

        ResultResponse response = MailAdminService.DeleteRecipient(request);
    }

    #region Group

    [Test]
    public void GetGroupListTest()
    {
        GetGroupListRequest request = new GetGroupListRequest
        {
        };

        GetGroupListResponse response;

        using (IGroupService groupService = new GroupService())
        {
            response = groupService.GetGroupList(request);
        }
    }

    #endregion
    */
}
