namespace Fanex.MailDelivery.Handlers.Impl
{
    using System.Collections.Generic;
    using EasyNetQ;
    using Fanex.MailDelivery.Client;
    using Fanex.MailDelivery.Client.Messages;
    using Fanex.MailDelivery.Client.Messages.MailingList;
    using Fanex.MailDelivery.Client.Models;
    using Fanex.MailDelivery.Configurations;
    using Fanex.MailDelivery.Handlers.Interface;
    using Fanex.MailDelivery.Services;
    using System;

    /// <summary>
    /// TODO: Update summary.
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
            SubscribeFunctionMailingListRequest();
            SubscribeFunctionMailingListUpdate();
            SubscribeScheduleMailingListRequest();
            SubscribeScheduleMailingListUpdate();
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
                        _mailService.UpdateJobStatus(jobStatus.Schedule, jobStatus.JobStatus);
                        _mailService.SendMail(jobStatus.Mail);
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
                    List<Schedule> schedules = _mailService.GetSchedules(scheduleRequest.Ids);

                    return schedules;
                });
        }

        /// <summary>
        /// Subscribes the function mailing list request.
        /// </summary>
        public void SubscribeFunctionMailingListRequest()
        {
            _bus.Respond<FunctionMailingListRequest, MailingListMessage>(
                functionMailingListRequest =>
                {
                    return new MailingListMessage
                        {
                            TOs = new List<string>
                            {
                                "aaa",
                                "bbb"
                            }
                        };
                });
        }

        /// <summary>
        /// Subscribes the schedule mailing list request.
        /// </summary>
        public void SubscribeScheduleMailingListRequest()
        {
            _bus.Respond<ScheduleMailingListRequest, MailingListMessage>(
                scheduleMailingListRequest =>
                {
                    return new MailingListMessage
                    {
                        TOs = new List<string>
                            {
                                "ccc",
                                "ddd"
                            }
                    };
                });
        }

        /// <summary>
        /// Subscribes the function mailing list update.
        /// </summary>
        public void SubscribeFunctionMailingListUpdate()
        {
            _bus.Respond<UpdateFunctionMailingListMessage, ResultMessage>(
                 updateFunctionMailingListMessage =>
                 {
                     Console.WriteLine("Updated function mailing list:" + updateFunctionMailingListMessage.MailingList.TOs[0]);
                     return new ResultMessage
                         {
                             ErrCode = 0,
                             Message = "Updated successfully!"
                         };
                 });
        }

        /// <summary>
        /// Subscribes the schedule mailing list update.
        /// </summary>
        public void SubscribeScheduleMailingListUpdate()
        {
            _bus.Respond<UpdateScheduleMailingListMessage, ResultMessage>(
                 updateScheduleMailingListMessage =>
                 {
                     Console.WriteLine("Updated schedule mailing list:" + updateScheduleMailingListMessage.MailingList.TOs[0]);
                     return new ResultMessage
                     {
                         ErrCode = 0,
                         Message = "Updated successfully!"
                     };
                 });
        }

        public void Dispose()
        {
            _bus.Dispose();
        }
    }
}