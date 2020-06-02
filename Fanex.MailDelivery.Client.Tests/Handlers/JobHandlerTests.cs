using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Models;
using NSubstitute;
using Quartz;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.MailDelivery.Client.Tests.Handlers
{
    public class JobHandlerTests
    {
        private readonly JobHandler handler;
        private readonly ISchedulerFactory mockSchedulerFactory;
        private readonly IScheduler mockScheduler;

        public JobHandlerTests()
        {
            mockScheduler = Substitute.For<IScheduler>();
            mockSchedulerFactory = Substitute.For<ISchedulerFactory>();
            mockSchedulerFactory.GetScheduler().Returns(mockScheduler);

            handler = new JobHandler(mockSchedulerFactory);
        }

        [Fact]
        public async Task InitializeSchedulerAsync_Any_CallShutdown()
        {
            // Arrange

            // Act
            await handler.InitializeSchedulerAsync();

            // Assert
            await mockScheduler.Received(1).Shutdown();
        }

        [Fact]
        public async Task InitializeSchedulerAsync_Any_CallStart()
        {
            // Arrange

            // Act
            await handler.InitializeSchedulerAsync();

            // Assert
            await mockScheduler.Received(1).Start();
        }

        [Fact]
        public async Task StopAsync_Any_CallShutdown()
        {
            // Arrange
            await handler.InitializeSchedulerAsync();

            // Act
            await handler.StopAsync();

            // Assert
            await mockScheduler.Received(2).Shutdown();
        }

        [Fact]
        public async Task RetryAllProcessAsync_Any_CallScheduleJob()
        {
            // Arrange
            Schedule schedule = FakeSchedule();
            await handler.InitializeSchedulerAsync();

            // Act
            await handler.RetryAllProcessAsync(schedule);

            // Assert
            await mockScheduler.Received(1).ScheduleJob(Arg.Compat.Any<IJobDetail>(), Arg.Compat.Any<ITrigger>());
        }

        [Fact]
        public async Task RetryJobAsync_Any_CallScheduleJob()
        {
            // Arrange
            Schedule schedule = FakeSchedule();
            await handler.InitializeSchedulerAsync();

            // Act
            await handler.RetryJobAsync(schedule);

            // Assert
            await mockScheduler.Received(1).ScheduleJob(Arg.Compat.Any<IJobDetail>(), Arg.Compat.Any<ITrigger>());
        }

        #region UpdateScheduleAsync
        [Fact]
        public async Task UpdateScheduleAsync_OldTriggerAndActiveSchedule_CallRescheduleJob()
        {
            // Arrange
            Schedule schedule = FakeSchedule();
            ITrigger trigger = FakeTrigger(schedule);
            await handler.InitializeSchedulerAsync();
            mockScheduler.GetTrigger(Arg.Compat.Any<TriggerKey>()).Returns(trigger);

            // Act
            await handler.UpdateScheduleAsync(schedule);

            // Assert
            await mockScheduler.Received(1).RescheduleJob(Arg.Compat.Any<TriggerKey>(), Arg.Compat.Any<ITrigger>());
        }

        [Fact]
        public async Task UpdateScheduleAsync_OldTriggerAndInactiveSchedule_CallUnscheduleJob()
        {
            // Arrange
            Schedule schedule = FakeSchedule();
            schedule.Active = false;
            ITrigger trigger = FakeTrigger(schedule);
            await handler.InitializeSchedulerAsync();
            mockScheduler.GetTrigger(Arg.Compat.Any<TriggerKey>()).Returns(trigger);

            // Act
            await handler.UpdateScheduleAsync(schedule);

            // Assert
            await mockScheduler.Received(1).UnscheduleJob(Arg.Compat.Any<TriggerKey>());
        }

        [Fact]
        public async Task UpdateScheduleAsync_OldTriggerAndEndedSchedule_CallUnscheduleJob()
        {
            // Arrange
            Schedule schedule = FakeSchedule();
            schedule.EndTime = DateTime.Now.AddDays(-1);
            ITrigger trigger = FakeTrigger(schedule);
            await handler.InitializeSchedulerAsync();
            mockScheduler.GetTrigger(Arg.Compat.Any<TriggerKey>()).Returns(trigger);

            // Act
            await handler.UpdateScheduleAsync(schedule);

            // Assert
            await mockScheduler.Received(1).UnscheduleJob(Arg.Compat.Any<TriggerKey>());
        }

        [Fact]
        public async Task UpdateScheduleAsync_NotOldTriggerAndActiveSchedule_CallScheduleJob()
        {
            // Arrange
            Schedule schedule = FakeSchedule();
            await handler.InitializeSchedulerAsync();
            mockScheduler.GetTrigger(Arg.Compat.Any<TriggerKey>()).Returns((ITrigger)null);

            // Act
            await handler.UpdateScheduleAsync(schedule);

            // Assert
            await mockScheduler.Received(1).ScheduleJob(Arg.Compat.Any<IJobDetail>(), Arg.Compat.Any<ITrigger>());
        }
        #endregion

        #region helper
        private static Schedule FakeSchedule()
        {
            return new Schedule
            {
                Id = 1,
                RepeatExpression = "* * 4 * * ?",
                StartTime = DateTime.Now.AddDays(-1),
                Active = true
            };
        }

        private static ITrigger FakeTrigger(Schedule schedule)
        {
            var triggerBuilder = TriggerBuilder.Create();
            ITrigger trigger = triggerBuilder
                                .WithIdentity(schedule.Id.ToString(), MailDeliveryConstants.MailCentralizedTriggerGroup)
                                .WithCronSchedule(schedule.RepeatExpression)
                                .Build();

            return trigger;
        }
        #endregion
    }
}
