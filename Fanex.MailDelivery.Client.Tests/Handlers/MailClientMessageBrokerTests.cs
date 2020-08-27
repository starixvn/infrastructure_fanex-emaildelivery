using EasyNetQ;
using EasyNetQ.FluentConfiguration;
using Fanex.Caching;
using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.MailDelivery.Client.Tests.Handlers
{
    public class MailClientMessageBrokerTests
    {
        private readonly MailClientMessageBroker broker;
        private readonly IBus mockBus;
        private readonly IJobHandler mockJobHandler;
        private readonly ICacheService cacheService;

        public MailClientMessageBrokerTests()
        {
            mockJobHandler = Substitute.For<IJobHandler>();
            cacheService = Substitute.For<ICacheService>();
            mockBus = Substitute.For<IBus>();
            mockBus.IsConnected.Returns(true);

            broker = new MailClientMessageBroker(mockJobHandler, mockBus, cacheService);
        }

        #region StartAsync
        [Fact]
        public async Task StartAsync_Any_CallInitializeSchedulerAsyncOfJobHandler()
        {
            // Arrange

            // Act
            await broker.StartAsync();

            // Assert
            await mockJobHandler.Received(1).InitializeSchedulerAsync();
        }

        [Fact]
        public async Task StartAsync_Any_CallSubscribeScheduleChangeAsync()
        {
            // Arrange
            var subscriptionId = "RoutingKey.UpdateSchedule_97";

            // Act
            await broker.StartAsync();

            // Assert
            mockBus.Received(1).SubscribeAsync<Schedule>(
                   Arg.Compat.Is<string>(x => x.StartsWith(subscriptionId)),
                   Arg.Compat.Any<Func<Schedule, Task>>(),
                   Arg.Compat.Any<Action<ISubscriptionConfiguration>>());
        }

        [Fact]
        public async Task StartAsync_Any_CallSubscribeRunNowJobAsync()
        {
            // Arrange
            var subscriptionId = "RoutingKey.RunNowJob_97";

            // Act
            await broker.StartAsync();

            // Assert
            mockBus.Received(1).SubscribeAsync<Schedule>(
                   Arg.Compat.Is<string>(x => x.StartsWith(subscriptionId)),
                   Arg.Compat.Any<Func<Schedule, Task>>(),
                   Arg.Compat.Any<Action<ISubscriptionConfiguration>>());
        }

        [Fact]
        public async Task StartAsync_Any_CallSubscribeRetryJobAsync()
        {
            // Arrange
            var subscriptionId = "RoutingKey.RetryJob_97";

            // Act
            await broker.StartAsync();

            // Assert
            mockBus.Received(1).SubscribeAsync<Schedule>(
                   Arg.Compat.Is<string>(x => x.StartsWith(subscriptionId)),
                   Arg.Compat.Any<Func<Schedule, Task>>(),
                   Arg.Compat.Any<Action<ISubscriptionConfiguration>>());
        }

        [Fact]
        public async Task StartAsync_Any_CallSubscribeRunAfterUMJobAsync()
        {
            // Arrange
            var subscriptionId = "RoutingKey.RunAfterUMJob_97";

            // Act
            await broker.StartAsync();

            // Assert
            mockBus.Received(1).SubscribeAsync<Schedule>(
                   Arg.Compat.Is<string>(x => x.StartsWith(subscriptionId)),
                   Arg.Compat.Any<Func<Schedule, Task>>(),
                   Arg.Compat.Any<Action<ISubscriptionConfiguration>>());
        }
        #endregion

        [Fact]
        public async Task StopAsync_Any_CallDisposeOfBus()
        {
            // Arrange

            // Act
            await broker.StopAsync();

            // Assert
            mockBus.Received(1).Dispose();
        }

        [Fact]
        public async Task StopAsync_Any_CallStopAsyncOfJobHandler()
        {
            // Arrange

            // Act
            await broker.StopAsync();

            // Assert
            await mockJobHandler.Received(1).StopAsync();
        }

        [Fact]
        public async Task SendFunctionMailAsync_Any_CallPublishAsyncOfBus()
        {
            // Arrange
            var mail = new Mail
            {
                Id = 1,
                Subject = "Subject",
                Body = "Body"
            };

            // Act
            await broker.SendFunctionMailAsync(mail);

            // Assert
            await mockBus.Received(1).PublishAsync(mail, MailDeliveryConstants.SendMailRoutingKey);
        }

        [Fact]
        public async Task SendFunctionMailAsync_HasException_RetryPublishAsyncOfBus()
        {
            // Arrange
            var mail = new Mail
            {
                Id = 1,
                Subject = "Subject",
                Body = "Body"
            };
            mockBus.When(x => x.PublishAsync(mail, MailDeliveryConstants.SendMailRoutingKey)).Throw(new InvalidOperationException());

            // Act
            await broker.SendFunctionMailAsync(mail);

            // Assert
            await mockBus.Received(2).PublishAsync(mail, MailDeliveryConstants.SendMailRoutingKey);
        }

        [Fact]
        public async Task PublishClientNotifyMessageAsync_Any_CallPublishAsyncOfBus()
        {
            // Arrange          

            // Act
            await broker.PublishClientNotifyMessageAsync();

            // Assert
            await mockBus.Received(1).PublishAsync(Arg.Compat.Is<UpdateActiveTimeRequest>(x => x.MailIds == "1"), MailDeliveryConstants.ClientNotificationTopic);
        }

        [Fact]
        public async Task ReportJobAsync_Any_CallPublishAsyncOfBus()
        {
            // Arrange          
            var jobMessage = new JobMessage
            {
                Mail = new Mail
                {
                    Id = 1,
                    Subject = "Subject",
                    Body = "Body"
                }
            };

            // Act
            await broker.ReportJobAsync(jobMessage);

            // Assert
            await mockBus.Received(1).PublishAsync(jobMessage, MailDeliveryConstants.JobResultRoutingKey);
        }

        [Fact]
        public async Task ReportJobAsync_HasException_RetryPublishAsync()
        {
            // Arrange          
            JobMessage jobMessage = FakeJobMessage();
            mockBus.When(x => x.PublishAsync(jobMessage, MailDeliveryConstants.JobResultRoutingKey)).Throw(new InvalidOperationException());

            // Act
            await broker.ReportJobAsync(jobMessage);

            // Assert
            await mockBus.Received(2).PublishAsync(jobMessage, MailDeliveryConstants.JobResultRoutingKey);
        }

        [Fact]
        public async Task PublishRunAfterUMJobAsync_Any_CallPublishAsyncOfBus()
        {
            // Arrange          
            var schedule = FakeSchedule();
            var subscriptionId = "RoutingKey.RunAfterUMJob_97";

            // Act
            await broker.PublishRunAfterUMJobAsync(schedule);

            // Assert
            await mockBus.Received(1).PublishAsync(schedule, subscriptionId);
        }

        [Fact]
        public async Task PublishRunAfterUMJobAsync_HasException_RetryPublishAsync()
        {
            // Arrange          
            var schedule = FakeSchedule();
            var subscriptionId = "RoutingKey.RunAfterUMJob_97";
            mockBus.When(x => x.PublishAsync(schedule, subscriptionId)).Throw(new InvalidOperationException());

            // Act
            await broker.PublishRunAfterUMJobAsync(schedule);

            // Assert
            await mockBus.Received(2).PublishAsync(schedule, subscriptionId);
        }

        [Fact]
        public async Task PublishTaskLogAsync_Any_CallPublishAsyncOfBus()
        {
            // Arrange          
            var taskLog = new TaskLog
            {
                TaskId = 1,
                Status = (int)JobStatus.Success
            };

            // Act
            await broker.PublishTaskLogAsync(taskLog);

            // Assert
            await mockBus.Received(1).PublishAsync(taskLog, MailDeliveryConstants.TaskLogRoutingKey);
        }

        [Fact]
        public async Task PublishTaskLogAsync_HasException_RetryPublishAsync()
        {
            // Arrange          
            var taskLog = new TaskLog
            {
                TaskId = 1,
                Status = (int)JobStatus.Success
            };
            mockBus.When(x => x.PublishAsync(taskLog, MailDeliveryConstants.TaskLogRoutingKey)).Throw(new InvalidOperationException());

            // Act
            await broker.PublishTaskLogAsync(taskLog);

            // Assert
            await mockBus.Received(2).PublishAsync(taskLog, MailDeliveryConstants.TaskLogRoutingKey);
        }

        [Fact]
        public async Task GetSchedulesAsync_Any_CallRequestAsyncOfBus()
        {
            // Arrange
            mockBus.RequestAsync<GetScheduleListRequest, GetScheduleListResponse>(Arg.Compat.Any<GetScheduleListRequest>()).Returns(new GetScheduleListResponse());

            // Act
            await broker.GetSchedulesAsync();

            // Assert
            await mockBus.Received(1).RequestAsync<GetScheduleListRequest, GetScheduleListResponse>(
                   Arg.Compat.Is<GetScheduleListRequest>(x => x.MailIds.Contains(1)));
        }

        [Fact]
        public async Task GetSchedulesAsync_HasException_RetryCallRequestAsyncOfBus()
        {
            // Arrange
            mockBus.When(x => x.RequestAsync<GetScheduleListRequest, GetScheduleListResponse>(Arg.Compat.Any<GetScheduleListRequest>())).Throw(new InvalidOperationException());

            // Act
            await broker.GetSchedulesAsync();

            // Assert
            await mockBus.Received(2).RequestAsync<GetScheduleListRequest, GetScheduleListResponse>(
                   Arg.Compat.Is<GetScheduleListRequest>(x => x.MailIds.Contains(1)));
        }

        [Fact]
        public async Task GetScheduleAsync_Any_CallRequestAsyncOfBus()
        {
            // Arrange
            mockBus.RequestAsync<GetScheduleListRequest, GetScheduleListResponse>(Arg.Compat.Any<GetScheduleListRequest>())
                .Returns(new GetScheduleListResponse { Schedules = new List<Schedule>() });

            // Act
            await broker.GetScheduleAsync(1);

            // Assert
            await mockBus.Received(1).RequestAsync<GetScheduleListRequest, GetScheduleListResponse>(
                   Arg.Compat.Is<GetScheduleListRequest>(x => x.MailIds.Contains(1)));
        }

        [Fact]
        public async Task RequestAddMailTaskForRunningJobAsync_Any_CallRequestAsyncOfBus()
        {
            // Arrange
            var jobMessage = FakeJobMessage();

            // Act
            await broker.RequestAddMailTaskForRunningJobAsync(jobMessage);

            // Assert
            await mockBus.Received(1).RequestAsync<JobMessage, JobMessage>(jobMessage);
        }

        [Fact]
        public async Task RequestAddMailTaskForRunningJobAsync_HasException_RetryCallRequestAsyncOfBus()
        {
            // Arrange
            var jobMessage = FakeJobMessage();
            mockBus.When(x => x.RequestAsync<JobMessage, JobMessage>(jobMessage)).Throw(new InvalidOperationException());

            // Act
            await broker.RequestAddMailTaskForRunningJobAsync(jobMessage);

            // Assert
            await mockBus.Received(2).RequestAsync<JobMessage, JobMessage>(jobMessage);
        }

        [Fact]
        public async Task RequestMailTaskByIdAsync_Any_CallRequestAsyncOfBus()
        {
            // Arrange
            var request = new GetMailTaskByIdRequest
            {
                TaskId = 1
            };

            // Act
            await broker.RequestMailTaskByIdAsync(request);

            // Assert
            await mockBus.Received(1).RequestAsync<GetMailTaskByIdRequest, MailTaskModel>(request);
        }

        [Fact]
        public async Task RequestMailTaskByIdAsync_HasException_RetryCallRequestAsyncOfBus()
        {
            // Arrange
            var request = new GetMailTaskByIdRequest
            {
                TaskId = 1
            };
            mockBus.When(x => x.RequestAsync<GetMailTaskByIdRequest, MailTaskModel>(request)).Throw(new InvalidOperationException());

            // Act
            await broker.RequestMailTaskByIdAsync(request);

            // Assert
            await mockBus.Received(2).RequestAsync<GetMailTaskByIdRequest, MailTaskModel>(request);
        }

        [Fact]
        public async Task GetUMInfoAsync_Any_CallRequestAsyncOfBus()
        {
            // Arrange

            // Act
            await broker.GetUMInfoAsync();

            // Assert
            await mockBus.Received(1).RequestAsync<UMInfoModel, UMInfoModel>(Arg.Compat.Any<UMInfoModel>());
        }

        [Fact]
        public async Task GetUMInfoAsync_HasException_RetryCallRequestAsyncOfBus()
        {
            // Arrange
            mockBus.When(x => x.RequestAsync<UMInfoModel, UMInfoModel>(Arg.Compat.Any<UMInfoModel>())).Throw(new InvalidOperationException());

            // Act
            await broker.GetUMInfoAsync();

            // Assert
            await mockBus.Received(2).RequestAsync<UMInfoModel, UMInfoModel>(Arg.Compat.Any<UMInfoModel>());
        }

        [Fact]
        public async Task SubscribeScheduleChangeAsync_HasException_RetryCallSubscribeAsyncOfBus()
        {
            // Arrange
            var subscriptionId = "RoutingKey.UpdateSchedule_97";
            mockBus.When(x => x.SubscribeAsync<Schedule>(
                       Arg.Compat.Any<string>(),
                       Arg.Compat.Any<Func<Schedule, Task>>(),
                       Arg.Compat.Any<Action<ISubscriptionConfiguration>>()))
                   .Throw(new InvalidOperationException());

            // Act
            await broker.SubscribeScheduleChangeAsync();

            // Assert
            mockBus.Received(2).SubscribeAsync<Schedule>(
                   Arg.Compat.Is<string>(x => x.StartsWith(subscriptionId)),
                   Arg.Compat.Any<Func<Schedule, Task>>(),
                   Arg.Compat.Any<Action<ISubscriptionConfiguration>>());
        }

        [Fact]
        public async Task SubscribeRunNowJobAsync_HasException_RetryCallSubscribeRunNowJobAsync()
        {
            // Arrange
            var subscriptionId = "RoutingKey.RunNowJob_97";
            mockBus.When(x => x.SubscribeAsync<Schedule>(
                           Arg.Compat.Any<string>(),
                           Arg.Compat.Any<Func<Schedule, Task>>(),
                           Arg.Compat.Any<Action<ISubscriptionConfiguration>>()))
                   .Throw(new InvalidOperationException());

            // Act
            await broker.SubscribeRunNowJobAsync();

            // Assert
            mockBus.Received(2).SubscribeAsync<Schedule>(
                   Arg.Compat.Is<string>(x => x.StartsWith(subscriptionId)),
                   Arg.Compat.Any<Func<Schedule, Task>>(),
                   Arg.Compat.Any<Action<ISubscriptionConfiguration>>());
        }

        [Fact]
        public async Task SubscribeRetryJobAsync_HasException_RetryCallSubscribeRetryJobAsync()
        {
            // Arrange
            var subscriptionId = "RoutingKey.RetryJob_97";
            mockBus.When(x => x.SubscribeAsync<Schedule>(
                      Arg.Compat.Any<string>(),
                      Arg.Compat.Any<Func<Schedule, Task>>(),
                      Arg.Compat.Any<Action<ISubscriptionConfiguration>>()))
                  .Throw(new InvalidOperationException());

            // Act
            await broker.SubscribeRetryJobAsync();

            // Assert
            mockBus.Received(2).SubscribeAsync<Schedule>(
                   Arg.Compat.Is<string>(x => x.StartsWith(subscriptionId)),
                   Arg.Compat.Any<Func<Schedule, Task>>(),
                   Arg.Compat.Any<Action<ISubscriptionConfiguration>>());
        }

        [Fact]
        public async Task SubscribeRunAfterUMJobAsync_HasException_RetryCallSubscribeRunAfterUMJobAsync()
        {
            // Arrange
            var subscriptionId = "RoutingKey.RunAfterUMJob_97";
            mockBus.When(x => x.SubscribeAsync<Schedule>(
                       Arg.Compat.Any<string>(),
                       Arg.Compat.Any<Func<Schedule, Task>>(),
                       Arg.Compat.Any<Action<ISubscriptionConfiguration>>()))
                   .Throw(new InvalidOperationException());

            // Act
            await broker.SubscribeRunAfterUMJobAsync();

            // Assert
            mockBus.Received(2).SubscribeAsync<Schedule>(
                   Arg.Compat.Is<string>(x => x.StartsWith(subscriptionId)),
                   Arg.Compat.Any<Func<Schedule, Task>>(),
                   Arg.Compat.Any<Action<ISubscriptionConfiguration>>());
        }

        #region helper
        private static Schedule FakeSchedule()
        {
            return new Schedule
            {
                Id = 1,
                SiteId = 97,
                RepeatExpression = "* * 4 * * ?",
                StartTime = DateTime.Now.AddDays(-1),
                Active = true
            };
        }

        private static JobMessage FakeJobMessage()
        {
            return new JobMessage
            {
                Mail = new Mail
                {
                    Id = 1,
                    Subject = "Subject",
                    Body = "Body"
                }
            };
        }
        #endregion
    }
}
