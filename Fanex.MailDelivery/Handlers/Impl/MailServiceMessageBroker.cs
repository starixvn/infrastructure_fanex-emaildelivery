namespace Fanex.MailDelivery.Handlers.Impl
{
    using System;
    using System.Collections.Generic;
    using EasyNetQ;
    using Fanex.MailDelivery.Client;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Configurations;
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
            SubscribeMailingListRequest();
            RespondAddMailTaskForRunningJob();
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

        /// <summary>
        /// Subscribes the function mailing list request.
        /// </summary>
        public void SubscribeMailingListRequest()
        {
            _bus.Respond<MailingListRequest, MailingList>(
                mailingListRequest =>
                {
                    return _mailService.GetMailingList(mailingListRequest.MailingListId);
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

        public void Dispose()
        {
            _bus.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}