using EasyNetQ;
using EasyNetQ.FluentConfiguration;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.MailStationService.MessageBrokers;
using Fanex.MailDelivery.Services.Services;
using NSubstitute;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.MailDelivery.MailStationService.Tests.MessageBrokers
{
    public class MessageBrokerTests
    {
        private readonly IBus mockBus;
        private readonly MailService mockMailService;
        private readonly LogService mockLogService;
        private readonly UMService mockUMService;
        private readonly MessageBroker messageBroker;

        public MessageBrokerTests()
        {
            mockMailService = Substitute.For<MailService>();
            mockLogService = Substitute.For<LogService>();
            mockUMService = Substitute.For<UMService>();
            mockBus = Substitute.For<IBus>();
            mockBus.IsConnected.Returns(true);
            messageBroker = new MessageBrokerImpl(mockMailService, mockLogService, mockUMService, mockBus);
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

    }
}
