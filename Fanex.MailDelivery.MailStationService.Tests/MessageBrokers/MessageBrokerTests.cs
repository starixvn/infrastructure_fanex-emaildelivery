using EasyNetQ;
using EasyNetQ.FluentConfiguration;
using Fanex.Caching;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.MailStationService.MessageBrokers;
using Fanex.MailDelivery.Services.Services;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.MailDelivery.MailStationService.Tests.MessageBrokers
{
    public class MessageBrokerTests : IDisposable
    {
        private readonly IBus mockBus;
        private readonly MailService mockMailService;
        private readonly LogService mockLogService;
        private readonly UMService mockUMService;
        private readonly MessageBroker messageBroker;
        private readonly ICacheService mockCacheService;

        public MessageBrokerTests()
        {
            mockMailService = Substitute.For<MailService>();
            mockLogService = Substitute.For<LogService>();
            mockUMService = Substitute.For<UMService>();
            mockCacheService = Substitute.For<ICacheService>();
            mockBus = Substitute.For<IBus>();
            mockBus.IsConnected.Returns(true);
            messageBroker = new MessageBrokerImpl(mockMailService, mockLogService, mockUMService, mockBus, mockCacheService);
        }

        [Fact]
        public void Start_Any_CallSubscribeMail()
        {
            // Act
            messageBroker.Start();

            // Assert
            mockBus.Received(1).SubscribeAsync<Mail>(
                MailDeliveryConstants.SendMailRoutingKey,
                Arg.Compat.Any<Func<Mail, Task>>(),
                Arg.Compat.Any<Action<ISubscriptionConfiguration>>());
        }

        [Fact]
        public void Start_Any_CallSubscribeJobMessage()
        {
            // Act
            messageBroker.Start();

            // Assert
            mockBus.Received(1).SubscribeAsync<JobMessage>(
                MailDeliveryConstants.JobResultRoutingKey,
                Arg.Compat.Any<Func<JobMessage, Task>>(),
                Arg.Compat.Any<Action<ISubscriptionConfiguration>>());
        }

        [Fact]
        public void Start_Any_CallSubscribeTaskLog()
        {
            // Act
            messageBroker.Start();

            // Assert
            mockBus.Received(1).SubscribeAsync<TaskLog>(
                MailDeliveryConstants.TaskLogRoutingKey,
                Arg.Compat.Any<Func<TaskLog, Task>>(),
                Arg.Compat.Any<Action<ISubscriptionConfiguration>>());
        }

        [Fact]
        public void Start_Any_CallSubscribeClientNotificationMessage()
        {
            // Act
            messageBroker.Start();

            // Assert
            mockBus.Received(1).SubscribeAsync<UpdateActiveTimeRequest>(
                MailDeliveryConstants.ClientNotificationTopic,
                Arg.Compat.Any<Func<UpdateActiveTimeRequest, Task>>(),
                Arg.Compat.Any<Action<ISubscriptionConfiguration>>());
        }

        [Fact]
        public void Start_Any_CallResponseGetScheduleList()
        {
            // Act
            messageBroker.Start();

            // Assert
            mockBus.Received(1).RespondAsync<GetScheduleListRequest, GetScheduleListResponse>(
                Arg.Compat.Any<Func<GetScheduleListRequest, Task<GetScheduleListResponse>>>());
        }

        [Fact]
        public void Start_Any_CallRespondAddMailTaskForRunningJob()
        {
            // Act
            messageBroker.Start();

            // Assert
            mockBus.Received(1).RespondAsync<JobMessage, JobMessage>(
                Arg.Compat.Any<Func<JobMessage, Task<JobMessage>>>());
        }

        [Fact]
        public void Start_Any_CallRespondGetMailTaskForById()
        {
            // Act
            messageBroker.Start();

            // Assert
            mockBus.Received(1).RespondAsync<GetMailTaskByIdRequest, MailTaskModel>(
                Arg.Compat.Any<Func<GetMailTaskByIdRequest, Task<MailTaskModel>>>());
        }

        [Fact]
        public void Start_Any_CallRespondUMInfo()
        {
            // Act
            messageBroker.Start();

            // Assert
            mockBus.Received(1).RespondAsync<UMInfoModel, UMInfoModel>(
                Arg.Compat.Any<Func<UMInfoModel, Task<UMInfoModel>>>());
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    messageBroker.Dispose();
                }

                disposedValue = true;
            }
        }

        ~MessageBrokerTests()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
