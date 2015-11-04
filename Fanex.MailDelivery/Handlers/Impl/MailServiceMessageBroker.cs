namespace Fanex.MailDelivery.Handlers.Impl
{
    using System;
    using System.Collections.Generic;
    using EasyNetQ;
    using Fanex.MailDelivery.Client;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Messages.MailManage;
    using Fanex.MailDelivery.Client.Messages.Recipient;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Configurations;
    using Fanex.MailDelivery.Criteria.Mail;
    using Fanex.MailDelivery.Criteria.Recipient;
    using Fanex.MailDelivery.Handlers.Interface;
    using Fanex.MailDelivery.Services;

    /// <summary>
    /// Mail Service Message Broker.
    /// </summary>
    public class MailServiceMessageBroker : IMessageBroker
    {
        private IBus _bus;
        private IMailService _mailService;

        public MailServiceMessageBroker()
            : this(null)
        {
        }

        public MailServiceMessageBroker(IMailService mailService)
        {
            _mailService = mailService ?? new MailService();
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            //// Standard practice is to create a single IBus instance for the lifetime of your application. Dispose it when your application closes.
            //// https://github.com/mikehadlow/EasyNetQ/wiki/Connecting-to-RabbitMQ
            _bus = RabbitHutch.CreateBus(
                AppSettings.MailServiceSection.RabbitMQConnectionString,
                serviceRegister =>
                {
                    serviceRegister.Register<IEasyNetQLogger>(_ => new RabbitMQLogger());
                });
            SubscribeScheduleRequest();
            SubscribeMail();
            SubscribeJobResult();
            SubscribeAbortJob();
            RespondAddMailTaskForRunningJob();
            SubscribeClientNotificationMessage();
            RespondGetListRecipient();
            RespondCreateRecipient();
            RespondUpdateRecipient();
            RespondGetListMail();
            RespondCreateMail();
            RespondUpdateMail();
            RespondGetRecipientById();
        }

        public void SubscribeAbortJob()
        {
            _bus.Subscribe<JobResultMessage>(
                MailDeliveryConstants.AbortJobRoutingKeyFormat,
                    jobStatus =>
                    {
                        _mailService.UpdateJobStatus(jobStatus);
                    },
                    config => config.WithTopic(MailDeliveryConstants.AbortJobRoutingKeyFormat));
        }

        /// <summary>
        /// Subscribes the mail.
        /// </summary>
        public void SubscribeMail()
        {
            _bus.Subscribe<Mail>(
                MailDeliveryConstants.SendMailRoutingKey,
                    mail =>
                    {
                        _mailService.SendMail(mail);
                    },
                    config => config.WithTopic(MailDeliveryConstants.SendMailRoutingKey));
        }

        /// <summary>
        /// Subscribes the job result.
        /// </summary>
        public void SubscribeJobResult()
        {
            _bus.Subscribe<JobResultMessage>(
                MailDeliveryConstants.JobResultRoutingKey,
                    jobStatus =>
                    {
                        _mailService.UpdateJobStatus(jobStatus);
                    },
                    config => config.WithTopic(MailDeliveryConstants.JobResultRoutingKey));
        }

        /// <summary>
        /// Subscribes the job result.
        /// </summary>
        public void SubscribeClientNotificationMessage()
        {
            _bus.Subscribe<ClientInfomationMessage>(
                MailDeliveryConstants.ClientNotificationTopic,
                    clientInfoMessage =>
                    {
                        _mailService.UpdateClientActiveTime(clientInfoMessage);
                    },
                    config => config.WithTopic(MailDeliveryConstants.ClientNotificationTopic));
        }

        /// <summary>
        /// Subscribes the schedule request.
        /// </summary>
        public void SubscribeScheduleRequest()
        {
            _bus.Respond<ScheduleRequestMessage, List<Schedule>>(
                scheduleRequest =>
                {
                    var jobLog = "Subscribe schedule request.";
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(jobLog);
                    Console.ForegroundColor = ConsoleColor.White;
                    List<Schedule> schedules = _mailService.GetSchedules(scheduleRequest.Ids);

                    return schedules;
                });
        }

        public void RespondAddMailTaskForRunningJob()
        {
            _bus.Respond<JobResultMessage, JobResultMessage>(
                jobResult =>
                {
                    _mailService.AddMailTaskForRunningJob(jobResult);
                    return jobResult;
                });
        }

        public void RespondGetListRecipient()
        {
            _bus.Respond<RecipientListMessage, List<RecipientModel>>(
                requestMessage =>
                {
                    return _mailService.GetListRecipient(new RecipientListCriteria()
                    {
                        Emails = requestMessage.Emails,
                        Status = requestMessage.Status,
                        UserName = requestMessage.UserName
                    });
                });
        }

        public void RespondCreateRecipient()
        {
            _bus.Respond<CreateRecipientMessage, ResultMessage>(
                requestMessage =>
                {
                    return _mailService.CreateRecipient(new CreateRecipientCriteria()
                    {
                        Email1 = requestMessage.Email1,
                        Email2 = requestMessage.Email2,
                        Email3 = requestMessage.Email3,
                        IsNAPUser = requestMessage.IsNAPUser,
                        NAPUserID = requestMessage.NAPUserID,
                        Note = requestMessage.Note,
                        Status = requestMessage.Status,
                        UserName = requestMessage.UserName
                    });
                });
        }

        public void RespondUpdateRecipient()
        {
            _bus.Respond<UpdateRecipientMessage, ResultMessage>(
                requestMessage =>
                {
                    return _mailService.UpdateRecipient(new UpdateRecipientCriteria()
                    {
                        Email1 = requestMessage.Email1,
                        Email2 = requestMessage.Email2,
                        Email3 = requestMessage.Email3,
                        Note = requestMessage.Note,
                        Status = requestMessage.Status,
                        Username = requestMessage.Username
                    });
                });
        }

        public void RespondGetListMail()
        {
            _bus.Respond<GetMailListMessage, List<MailModel>>(
                requestMessage =>
                {
                    return _mailService.GetListMail(new GetMailListCriteria() 
                    { 
                        Emails = requestMessage.Emails,
                        IsActive = requestMessage.IsActive,
                        MailGroupId = requestMessage.MailGroupId,
                        MailName = requestMessage.MailName,
                        SiteId = requestMessage.SiteId,
                        TriggerType = requestMessage.TriggerType
                    });
                });
        }

        public void RespondCreateMail()
        {
            _bus.Respond<CreateMailMessage, ResultMessage>(
                    requestMessage =>
                    {
                        return _mailService.CreateMail(new CreateMailCriteria() 
                        {
                            ////Attempt = requestMessage.Attempt,
                            ////CronExpression = requestMessage.CronExpression,
                            Description = requestMessage.Description,
                            ////EndOn = requestMessage.EndOn,
                            ////Interval = requestMessage.Interval,
                            IsActive = requestMessage.IsActive,
                            MailBcc = requestMessage.MailBcc,
                            MailCc = requestMessage.MailCc,
                            MailFrom = requestMessage.MailFrom,
                            MailGroupId = requestMessage.MailGroupId,
                            MailName = requestMessage.MailName,
                            MailTo = requestMessage.MailTo,
                            ////RepeatType = requestMessage.RepeatType,
                            SiteId = requestMessage.SiteId,
                            ////StartOn = requestMessage.StartOn,
                            TriggerType = 0 // Function Mail
                        });
                    });
        }

        public void RespondUpdateMail()
        {
            _bus.Respond<UpdateMailMessage, ResultMessage>(
                requestMessage =>
                {
                    return _mailService.UpdateMail(new UpdateMailCriteria() 
                    {
                        ////Attempt = requestMessage.Attempt,
                        ////CronExpression = requestMessage.CronExpression,
                        Description = requestMessage.Description,
                        ////EndOn = requestMessage.EndOn,
                        ////Interval = requestMessage.Interval,
                        IsActive = requestMessage.IsActive,
                        MailBcc = requestMessage.MailBcc,
                        MailCc = requestMessage.MailCc,
                        MailFrom = requestMessage.MailFrom,
                        MailGroupId = requestMessage.MailGroupId,
                        MailId = requestMessage.MailId,
                        MailName = requestMessage.MailName,
                        MailTo = requestMessage.MailTo,
                        ////RepeatType = requestMessage.RepeatType,
                        SiteId = requestMessage.SiteId,
                        ////StartOn = requestMessage.StartOn,
                        TriggerType = 0 // Function Mail
                    });
                });
        }

        public void RespondGetRecipientById()
        {
            _bus.Respond<GetRecipientByIdMessage, RecipientModel>(
                requestMessage =>
                {
                    return _mailService.RecipientById(new RecipientByIdCriteria() {
                        RecipientId = requestMessage.RecipientId
                    });
                });
        }

        public void Dispose()
        {
            _bus.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}