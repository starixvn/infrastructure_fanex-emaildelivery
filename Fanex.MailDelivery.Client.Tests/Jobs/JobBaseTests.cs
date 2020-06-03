using Fanex.MailDelivery.Client.Handlers;
using Fanex.MailDelivery.Client.Jobs;
using Fanex.MailDelivery.Core;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using NSubstitute;
using Quartz;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.MailDelivery.Client.Tests.Jobs
{
    public class JobBaseTests
    {
        private readonly JobBase jobBase;
        private readonly IMailClientMessageBroker mockBroker;
        private readonly IJobExecutionContext mockContext;

        public JobBaseTests()
        {
            jobBase = new JobBaseImpl();
            mockBroker = Substitute.For<IMailClientMessageBroker>();
            mockContext = Substitute.For<IJobExecutionContext>();
            MailClientHandler.StartAsync(mockBroker)
                .GetAwaiter()
                .GetResult();
        }

        [Fact]
        public async Task Execute_Any_CallGetUMInfoAsync()
        {
            // Arrange
            var schedule = FakeSchedule();
            mockContext.MergedJobDataMap.Returns(new JobDataMap(new Dictionary<string, Schedule> { { MailDeliveryConstants.ScheduleJobDataKey, schedule } }));
            mockBroker.GetUMInfoAsync().Returns(new UMInfoModel { });

            // Act
            await jobBase.Execute(mockContext);

            // Assert
            await mockBroker.Received(1).GetUMInfoAsync();
        }

        [Fact]
        public async Task Execute_HasException_NotCallGetSchedules()
        {
            // Arrange
            var schedule = FakeSchedule();
            mockContext.MergedJobDataMap.Returns(new JobDataMap(new Dictionary<string, Schedule> { { MailDeliveryConstants.ScheduleJobDataKey, schedule } }));
            mockBroker.When(x => x.GetUMInfoAsync()).Throw(new InvalidOperationException());

            // Act
            await jobBase.Execute(mockContext);

            // Assert
            await mockBroker.DidNotReceive().GetScheduleAsync(Arg.Compat.Any<int>());
        }

        [Fact]
        public async Task Execute_InUM_CallPublishRunAfterUMJobAsync()
        {
            // Arrange
            var schedule = FakeSchedule();
            mockContext.MergedJobDataMap.Returns(new JobDataMap(new Dictionary<string, Schedule> { { MailDeliveryConstants.ScheduleJobDataKey, schedule } }));
            mockBroker.GetUMInfoAsync().Returns(new UMInfoModel { From = DateTime.Now.AddHours(1), IsUnderMaintenanceTime = true, To = DateTime.Now.AddHours(3) });

            // Act
            await jobBase.Execute(mockContext);

            // Assert
            await mockBroker.Received(1).PublishRunAfterUMJobAsync(Arg.Compat.Is<Schedule>(x => x.Id == schedule.Id));
        }

        [Fact]
        public async Task Execute_NotInUM_CallGetScheduleAsync()
        {
            // Arrange
            var schedule = FakeSchedule();
            mockContext.MergedJobDataMap.Returns(new JobDataMap(new Dictionary<string, Schedule> { { MailDeliveryConstants.ScheduleJobDataKey, schedule } }));

            // Act
            await jobBase.Execute(mockContext);

            // Assert
            await mockBroker.Received(1).GetScheduleAsync(Arg.Compat.Is<int>(x => x == schedule.Id));
        }

        [Fact]
        public async Task Execute_InactiveSchedule_CallPublishTaskLogAsync()
        {
            // Arrange
            var schedule = FakeSchedule();
            schedule.TaskId = 1;
            mockContext.MergedJobDataMap.Returns(new JobDataMap(new Dictionary<string, Schedule> { { MailDeliveryConstants.ScheduleJobDataKey, schedule } }));
            mockBroker.GetScheduleAsync(schedule.Id).Returns(new Schedule { Id = schedule.Id, Active = false });

            // Act
            await jobBase.Execute(mockContext);

            // Assert
            await mockBroker.Received(1).PublishTaskLogAsync(Arg.Compat.Is<TaskLog>(x => x.TaskId == schedule.TaskId));
        }

        [Fact]
        public async Task Execute_ActiveScheduleAndNoTaskId_CallRequestAddMailTaskForRunningJobAsync()
        {
            // Arrange
            var schedule = FakeSchedule();
            mockContext.MergedJobDataMap.Returns(new JobDataMap(new Dictionary<string, Schedule> { { MailDeliveryConstants.ScheduleJobDataKey, schedule } }));
            mockBroker.GetScheduleAsync(schedule.Id).Returns(new Schedule { Id = schedule.Id, Active = true });

            // Act
            await jobBase.Execute(mockContext);

            // Assert
            await mockBroker.Received(1).RequestAddMailTaskForRunningJobAsync(Arg.Compat.Is<JobMessage>(x => x.Schedule.Id == schedule.Id));
        }

        [Fact]
        public async Task Execute_ActiveScheduleAndHasTaskId_CallReportJobAsync()
        {
            // Arrange
            var schedule = FakeSchedule();
            schedule.TaskId = 1;
            mockContext.MergedJobDataMap.Returns(new JobDataMap(new Dictionary<string, Schedule> { { MailDeliveryConstants.ScheduleJobDataKey, schedule } }));
            mockBroker.GetScheduleAsync(schedule.Id).Returns(new Schedule { Id = schedule.Id, Active = true });
            mockBroker.RequestMailTaskByIdAsync(Arg.Compat.Is<GetMailTaskByIdRequest>(x => x.TaskId == schedule.TaskId)).Returns(new MailTaskModel { Status = (int)JobStatus.Running });

            // Act
            await jobBase.Execute(mockContext);

            // Assert
            await mockBroker.Received(2).ReportJobAsync(
                Arg.Compat.Is<JobMessage>(
                    x => x.Schedule.Id == schedule.Id
                    && x.Schedule.TaskId == schedule.TaskId));
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
        #endregion
    }

    public class JobBaseImpl : JobBase
    {
        protected override JobMessage Run(Schedule schedule, IJobExecutionContext jobContext)
        {
            return new JobMessage();
        }
    }
}
