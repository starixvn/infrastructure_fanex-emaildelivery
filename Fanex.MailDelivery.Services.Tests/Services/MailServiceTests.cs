using Fanex.MailDeliver.Business.Interface;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Criteria;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Repositories;
using Fanex.MailDelivery.Services.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Services
{
    public class MailServiceTests
    {
        private readonly MailService mailService;
        private readonly IMailRepository mockMailRepository;
        private readonly IMailDeliverBusiness mockMailDeliverBusiness;
        private readonly LogService mockLogService;

        public MailServiceTests()
        {
            mockLogService = Substitute.For<LogService>();
            mockMailRepository = Substitute.For<IMailRepository>();
            mockMailDeliverBusiness = Substitute.For<IMailDeliverBusiness>();
            mailService = new MailServiceImpl(1, 0, mockLogService, mockMailRepository, mockMailDeliverBusiness);
        }

        [Fact]
        public void GetScheduleList_Any_CallGetScheduleByMailIdOfRepository()
        {
            // Arrange
            int mailId = 123;

            // Act
            mailService.GetScheduleList(new GetScheduleListRequest { MailIds = new List<int> { mailId } });

            // Assert
            mockMailRepository.Received(1).GetScheduleByMailId(Arg.Compat.Is<GetScheduleByMailIdCriteria>(x => x.MailId == mailId));
        }

        [Fact]
        public void GetScheduleList_TwoMailIds_ReturnsTwoSchedules()
        {
            // Arrange
            int mailId1 = 1;
            int mailId2 = 2;
            var schedule1 = new Schedule { Id = mailId1 };
            var schedule2 = new Schedule { Id = mailId2 };
            mockMailRepository.GetScheduleByMailId(Arg.Compat.Is<GetScheduleByMailIdCriteria>(x => x.MailId == mailId1))
                .Returns(schedule1);
            mockMailRepository.GetScheduleByMailId(Arg.Compat.Is<GetScheduleByMailIdCriteria>(x => x.MailId == mailId2))
                .Returns(schedule2);

            // Act
            var schedules = mailService.GetScheduleList(new GetScheduleListRequest { MailIds = new List<int> { mailId1, mailId2 } });

            // Assert
            Assert.Collection(
                schedules.Schedules,
                (x) => Assert.Equal(mailId1, x.Id),
                (x) => Assert.Equal(mailId2, x.Id));
        }

        [Fact]
        public void GetScheduleList_HasException_CallLogErrorOfLogService()
        {
            // Arrange
            int mailId = 123;
            mockMailRepository.GetScheduleByMailId(Arg.Compat.Any<GetScheduleByMailIdCriteria>()).Returns(x => { throw new Exception(); });

            // Act
            mailService.GetScheduleList(new GetScheduleListRequest { MailIds = new List<int> { mailId } });

            // Assert
            mockLogService.Received(1).LogError(Arg.Compat.Any<Exception>(), mailId);
        }

        [Fact]
        public void UpdateClientActiveTime_Any_CallUpdateClientActiveTimeOfRepository()
        {
            // Arrange
            var mailId = "123";

            // Act
            mailService.UpdateClientActiveTime(new UpdateActiveTimeRequest { MailIds = mailId });

            // Assert
            mockMailRepository.Received(1).UpdateClientActiveTime(Arg.Compat.Is<UpdateClientActiveTimeCriteria>(x => x.MailIds == mailId));
        }

        [Fact]
        public void UpdateClientActiveTime_HasException_CallLogErrorOfLogService()
        {
            // Arrange
            var mailId = "123";
            mockMailRepository
                .When(x => x.UpdateClientActiveTime(Arg.Compat.Any<UpdateClientActiveTimeCriteria>()))
                .Do((x) => { throw new Exception(); });

            // Act
            mailService.UpdateClientActiveTime(new UpdateActiveTimeRequest { MailIds = mailId });

            // Assert
            mockLogService.Received(1).LogError(Arg.Compat.Any<Exception>());
        }

        [Fact]
        public async Task UpdateJobStatus_HasTaskLog_CallAddMailLog()
        {
            // Arrange
            var jobResult = GenerateJobMessage();
            jobResult.JobStatus = JobStatus.SuccessWithoutSendmail;

            // Act
            await mailService.UpdateJobStatus(jobResult);

            // Assert
            mockMailRepository.Received(1).AddMailLog(Arg.Compat.Any<AddMailLogCriteria>());
        }

        [Fact]
        public async Task UpdateJobStatus_RunningJob_CallUpdateMailTaskStatusOfRepository()
        {
            // Arrange
            var jobResult = GenerateJobMessage();
            jobResult.JobStatus = JobStatus.Running;

            // Act
            await mailService.UpdateJobStatus(jobResult);

            // Assert
            mockMailRepository.Received(1).UpdateMailTaskStatus(
                Arg.Compat.Is<UpdateMailTaskStatusCriteria>(x => x.Status == (int)JobStatus.Running && x.TaskId == jobResult.Schedule.TaskId));
        }

        [Fact]
        public async Task UpdateJobStatus_RunningJob_CallUpdateRetryCounterOfRepository()
        {
            // Arrange
            var jobResult = GenerateJobMessage();
            jobResult.JobStatus = JobStatus.Running;

            // Act
            await mailService.UpdateJobStatus(jobResult);

            // Assert
            mockMailRepository.Received(1).UpdateRetryCounter(
                Arg.Compat.Is<UpdateTaskRetryCounterCriteria>(x => x.TaskId == jobResult.Schedule.TaskId && x.RetryCount == jobResult.Schedule.CurrentRetryTimes));
        }

        [Fact]
        public async Task UpdateJobStatus_DataNotReadyJob_CallUpdateMailTaskStatusOfRepository()
        {
            // Arrange
            JobMessage jobResult = GenerateJobMessage();
            jobResult.JobStatus = JobStatus.DataIsNotReady;

            // Act
            await mailService.UpdateJobStatus(jobResult);

            // Assert
            mockMailRepository.Received(1).UpdateMailTaskStatus(
                Arg.Compat.Is<UpdateMailTaskStatusCriteria>(x => x.Status == (int)JobStatus.DataIsNotReady && x.TaskId == jobResult.Schedule.TaskId));
        }

        [Fact]
        public async Task UpdateJobStatus_AbortedJob_CallUpdateMailTaskStatusOfRepository()
        {
            // Arrange
            JobMessage jobResult = GenerateJobMessage();
            jobResult.JobStatus = JobStatus.Abort;

            // Act
            await mailService.UpdateJobStatus(jobResult);

            // Assert
            mockMailRepository.Received(1).UpdateMailTaskStatus(
                Arg.Compat.Is<UpdateMailTaskStatusCriteria>(x => x.Status == (int)JobStatus.Abort && x.TaskId == jobResult.Schedule.TaskId));
        }

        [Fact]
        public async Task UpdateJobStatus_SucceededJob_CallInsertMailRecordOfMailDeliver()
        {
            // Arrange
            JobMessage jobResult = GenerateJobMessage();
            jobResult.JobStatus = JobStatus.Success;
            mockMailRepository.GetMailTaskById(Arg.Compat.Any<GetMailTaskByIdCriteria>()).Returns(new MailTaskModel { Status = (int)JobStatus.Success });
            mockMailRepository.GetScheduleByMailId(Arg.Compat.Any<GetScheduleByMailIdCriteria>())
                .Returns(new Schedule
                {
                    Active = true,
                    TriggerType = (int)MailType.Schedule,
                    TriggerTypeEnum = MailType.Schedule,
                    Id = 1,
                    Name = "Auto Mail",
                    SiteId = 1,
                    TaskId = 1,
                    RetryInterval = 0,
                    MaxRetryTimes = 1,
                    RepeatExpression = string.Empty
                });
            mockMailRepository.GetMailingList(Arg.Compat.Any<GetMailWithEmailAddressByMailId>())
                .Returns(new MailingList
                {
                    TOs = new List<string> { },
                    BCCs = new List<string> { },
                    CCs = new List<string> { }
                });

            // Act
            await mailService.UpdateJobStatus(jobResult);

            // Assert
            mockMailDeliverBusiness.Received(1).InsertMailRecord(Arg.Compat.Any<MailDeliver.Model.MailRecord>());
        }

        [Fact]
        public async Task UpdateJobStatus_SucceededJobAndInsertMailRecordFailed_CallUpdateMailTaskStatusOfRepository()
        {
            // Arrange
            JobMessage jobResult = GenerateJobMessage();
            jobResult.JobStatus = JobStatus.Success;
            mockMailRepository.GetMailTaskById(Arg.Compat.Any<GetMailTaskByIdCriteria>()).Returns(new MailTaskModel { Status = (int)JobStatus.Success });
            mockMailRepository.GetScheduleByMailId(Arg.Compat.Any<GetScheduleByMailIdCriteria>())
                .Returns(new Schedule
                {
                    Active = true,
                    TriggerType = (int)MailType.Schedule,
                    TriggerTypeEnum = MailType.Schedule,
                    Id = 1,
                    Name = "Auto Mail",
                    SiteId = 1,
                    TaskId = 1,
                    RetryInterval = 0,
                    MaxRetryTimes = 1,
                    RepeatExpression = string.Empty
                });
            mockMailRepository.GetMailingList(Arg.Compat.Any<GetMailWithEmailAddressByMailId>())
                .Returns(new MailingList
                {
                    TOs = new List<string> { },
                    BCCs = new List<string> { },
                    CCs = new List<string> { }
                });

            // Act
            await mailService.UpdateJobStatus(jobResult);

            // Assert
            mockMailRepository.Received(1).UpdateMailTaskStatus(
                Arg.Compat.Is<UpdateMailTaskStatusCriteria>(x => x.Status == (int)JobStatus.JobFailed && x.TaskId == jobResult.Schedule.TaskId));
        }

        [Fact]
        public async Task UpdateJobStatus_SucceededJobAndInsertMailRecordFailed_CallAddMailLogOfRepository()
        {
            // Arrange
            JobMessage jobResult = GenerateJobMessage();
            jobResult.JobStatus = JobStatus.Success;
            mockMailRepository.GetMailTaskById(Arg.Compat.Any<GetMailTaskByIdCriteria>()).Returns(new MailTaskModel { Status = (int)JobStatus.Success });
            mockMailRepository.GetScheduleByMailId(Arg.Compat.Any<GetScheduleByMailIdCriteria>())
                .Returns(new Schedule
                {
                    Active = true,
                    TriggerType = (int)MailType.Schedule,
                    TriggerTypeEnum = MailType.Schedule,
                    Id = 1,
                    Name = "Auto Mail",
                    SiteId = 1,
                    TaskId = 1,
                    RetryInterval = 0,
                    MaxRetryTimes = 1,
                    RepeatExpression = string.Empty
                });
            mockMailRepository.GetMailingList(Arg.Compat.Any<GetMailWithEmailAddressByMailId>())
                .Returns(new MailingList
                {
                    TOs = new List<string> { },
                    BCCs = new List<string> { },
                    CCs = new List<string> { }
                });

            // Act
            await mailService.UpdateJobStatus(jobResult);

            // Assert
            mockMailRepository.Received(1).AddMailLog(
                Arg.Compat.Is<AddMailLogCriteria>(x => x.Status == (int)JobStatus.JobFailed && x.TaskId == jobResult.Schedule.TaskId));
        }

        [Fact]
        public async Task UpdateJobStatus_SucceededJobAndFunctionMail_CallAddMailTaskOfRepository()
        {
            // Arrange
            JobMessage jobResult = GenerateJobMessage();
            jobResult.JobStatus = JobStatus.Success;
            mockMailRepository.GetMailTaskById(Arg.Compat.Any<GetMailTaskByIdCriteria>()).Returns(new MailTaskModel { Status = (int)JobStatus.Success });
            mockMailRepository.GetScheduleByMailId(Arg.Compat.Any<GetScheduleByMailIdCriteria>())
                .Returns(new Schedule
                {
                    Active = true,
                    TriggerType = (int)MailType.Function,
                    TriggerTypeEnum = MailType.Function,
                    Id = 1,
                    Name = "Function Mail",
                    SiteId = 1,
                    TaskId = 1,
                    RetryInterval = 0,
                    MaxRetryTimes = 1,
                    RepeatExpression = string.Empty
                });
            mockMailRepository.GetMailingList(Arg.Compat.Any<GetMailWithEmailAddressByMailId>())
                .Returns(new MailingList
                {
                    TOs = new List<string> { },
                    BCCs = new List<string> { },
                    CCs = new List<string> { }
                });
            mockMailRepository.AddMailTask(Arg.Compat.Any<AddMailTaskCriteria>()).Returns(1);

            // Act
            await mailService.UpdateJobStatus(jobResult);

            // Assert
            mockMailRepository.Received(1).AddMailTask(
                Arg.Compat.Is<AddMailTaskCriteria>(
                    x => x.Status == (int)JobStatus.Running
                    && x.SiteId == jobResult.Mail.SiteId
                    && x.MailId == jobResult.Mail.MailingListId));
        }

        [Fact]
        public async Task UpdateJobStatus_SucceededJobAndAbortedByUser_LogMessageAndNotSendMail()
        {
            // Arrange
            JobMessage jobResult = GenerateJobMessage();
            jobResult.JobStatus = JobStatus.Success;
            mockMailRepository.GetMailTaskById(Arg.Compat.Any<GetMailTaskByIdCriteria>()).Returns(new MailTaskModel { Status = (int)JobStatus.Abort });

            // Act
            await mailService.UpdateJobStatus(jobResult);

            // Assert
            mockLogService.Received(1).LogInfo(Arg.Compat.Any<string>());
        }

        [Fact]
        public void UpdateFailedForLongRunningMailTask_Any_CallUpdateFailedForLongRunningMailTaskOfRepository()
        {
            // Act
            mailService.UpdateFailedForLongRunningMailTask();

            // Assert
            mockMailRepository.Received(1).UpdateFailedForLongRunningMailTask();
        }

        [Fact]
        public void UpdateFailedForLongRunningMailTask_ThrowException_CallLogErrorOfLogService()
        {
            // Arrange
            mockMailRepository.When((x) => x.UpdateFailedForLongRunningMailTask()).Throw(new InvalidOperationException());

            // Act
            mailService.UpdateFailedForLongRunningMailTask();

            // Assert
            mockLogService.Received(1).LogError(Arg.Compat.Any<InvalidOperationException>());
        }

        [Fact]
        public void AddMailTaskForRunningJob_Any_CallAddMailTaskOfRepository()
        {
            // Act
            mailService.AddMailTaskForRunningJob(new JobMessage { Schedule = new Schedule() });

            // Assert
            mockMailRepository.Received(1).AddMailTask(Arg.Compat.Any<AddMailTaskCriteria>());
        }

        [Fact]
        public void AddMailTaskForRunningJob_ThrowException_CallLogErrorOfLogService()
        {
            // Arrange
            mockMailRepository.When((x) => x.AddMailTask(Arg.Compat.Any<AddMailTaskCriteria>())).Throw(new InvalidOperationException());

            // Act
            mailService.AddMailTaskForRunningJob(new JobMessage { Schedule = new Schedule() });

            // Assert
            mockLogService.Received(1).LogError(Arg.Compat.Any<InvalidOperationException>(), Arg.Compat.Any<Schedule>());
        }

        [Fact]
        public void InsertTaskLog_Any_CallAddMailLogOfRepository()
        {
            // Act
            mailService.InsertTaskLog(new TaskLog { TaskId = 1 });

            // Assert
            mockMailRepository.Received(1).AddMailLog(Arg.Compat.Any<AddMailLogCriteria>());
        }

        [Fact]
        public void InsertTaskLog_ThrowException_CallLogErrorOfLogService()
        {
            // Arrange
            mockMailRepository.When((x) => x.AddMailLog(Arg.Compat.Any<AddMailLogCriteria>())).Throw(new InvalidOperationException());

            // Act
            mailService.InsertTaskLog(new TaskLog { TaskId = 1 });

            // Assert
            mockLogService.Received(1).LogError(Arg.Compat.Any<InvalidOperationException>());
        }

        [Fact]
        public void GetMailTaskById_Any_CallGetMailTaskByIdOfRepository()
        {
            // Act
            mailService.GetMailTaskById(new GetMailTaskByIdRequest());

            // Assert
            mockMailRepository.Received(1).GetMailTaskById(Arg.Compat.Any<GetMailTaskByIdCriteria>());
        }

        [Fact]
        public void GetMailTaskById_ThrowException_CallLogErrorOfLogService()
        {
            // Arrange
            mockMailRepository.When((x) => x.GetMailTaskById(Arg.Compat.Any<GetMailTaskByIdCriteria>())).Throw(new InvalidOperationException());

            // Act
            mailService.GetMailTaskById(new GetMailTaskByIdRequest());

            // Assert
            mockLogService.Received(1).LogError(Arg.Compat.Any<InvalidOperationException>());
        }

        #region Helper
        private static JobMessage GenerateJobMessage()
        {
            return new JobMessage
            {
                Mail = new Mail
                {
                    Id = 1,
                    MailingListId = 1,
                    SiteId = 1,
                    Subject = "Subject",
                    Body = "Body",
                    ApplicationSendMailId = 1,
                    Title = "Title",
                    Attachments = new List<MailAttachment> {
                        new MailAttachment {
                            Id=1,
                            MailId=1,
                            FileName="Test.txt",
                            FileContent = new byte[] { 1,2,3 },
                            FileExtension= FileExtension.TEXT
                        }
                    },
                    AlternativeViews = new List<MailLinkedResource>
                    {
                        new MailLinkedResource
                        {
                            Id=1,
                            MailId=1,
                            ContentId="1",
                            FileName="Test.txt",
                            FileContent = new byte[] { 1,2,3 },
                            FileExtension= FileExtension.TEXT
                        }
                    }
                },
                TaskLog = new TaskLog { LogTime = DateTime.Now, Message = "This is an error" },
                Schedule = new Schedule
                {
                    Id = 1,
                    Active = true,
                    SiteId = 1,
                    TaskId = 123
                }
            };
        }
        #endregion
    }
}
