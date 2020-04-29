using Fanex.Logging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Services;
using NSubstitute;
using System;
using System.Collections.Generic;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Services
{
    public class LogServiceTests
    {
        private readonly MailDelivery.Services.Services.LogService logService;
        private readonly ILogger mockLogger;
        private const string Message = "This is an error message";

        public LogServiceTests()
        {
            mockLogger = Substitute.For<ILogger>();
            logService = new LogServiceImpl(mockLogger);
        }

        [Fact]
        public void LogInfo_Any_CallInfoOfLogger()
        {
            // Act
            logService.LogInfo(Message);

            // Assert
            mockLogger.Received(1).Info(Message);
        }

        [Fact]
        public void LogError_Any_CallErrorOfLogger()
        {
            // Act
            logService.LogError(Message);

            // Assert
            mockLogger.Received(1).Error(
                Arg.Compat.Is<string>(x => x.Contains(Message)));
        }

        [Fact]
        public void LogError_HasException_CallErrorOfLogger()
        {
            // Act
            logService.LogError(Message, new InvalidOperationException());

            // Assert
            mockLogger.Received(1).Error(
                Arg.Compat.Is<string>(x => x.Contains(Message)));
        }

        [Fact]
        public void LogError_OnlyException_CallErrorOfLogger()
        {
            // Act
            logService.LogError(new InvalidOperationException(Message));

            // Assert
            mockLogger.Received(1).Error(
                Arg.Compat.Is<string>(x => x.Contains(Message)));
        }

        [Fact]
        public void LogError_ExceptionAndFunctionMail_CallErrorOfLogger()
        {
            // Arrange
            var schedule = new Schedule
            {
                Id = 1,
                Name = "Mail Test",
                TriggerType = (int)MailType.Function,
                TriggerTypeEnum = MailType.Function
            };
            var customMessage = "Function Mail #1 (Mail Test)";

            // Act
            logService.LogError(new InvalidOperationException(Message), schedule);

            // Assert
            mockLogger.Received(1).Error(
                Arg.Compat.Is<string>(x => x.Contains(customMessage)));
        }

        [Fact]
        public void LogError_ExceptionAndScheduledMail_CallErrorOfLogger()
        {
            // Arrange
            var schedule = new Schedule
            {
                Id = 1,
                Name = "Mail Test",
                TriggerType = (int)MailType.Schedule,
                TriggerTypeEnum = MailType.Schedule
            };
            var customMessage = "Schedule Mail #1 (Mail Test)";

            // Act
            logService.LogError(new InvalidOperationException(Message), schedule);

            // Assert
            mockLogger.Received(1).Error(
                Arg.Compat.Is<string>(x => x.Contains(customMessage)));
        }

        [Fact]
        public void LogError_ExceptionAndMailId_CallErrorOfLogger()
        {
            // Arrange
            int mailId = 1;
            var customMessage = "Schedule Mail #1";

            // Act
            logService.LogError(new InvalidOperationException(Message), mailId);

            // Assert
            mockLogger.Received(1).Error(
                Arg.Compat.Is<string>(x => x.Contains(customMessage)));
        }

        [Fact]
        public void LogError_ExceptionAndScheduleList_CallErrorOfLogger()
        {
            // Arrange
            var scheduleIds = new List<int> { 1, 2, 3 };
            var customMessage = "Mail #1, 2, 3";

            // Act
            logService.LogError(new InvalidOperationException(Message), scheduleIds);

            // Assert
            mockLogger.Received(1).Error(
                Arg.Compat.Is<string>(x => x.Contains(customMessage)));
        }

        [Fact]
        public void LogError_ExceptionAndNoScheduleButHasMailId_CallErrorOfLogger()
        {
            // Arrange
            Schedule schedule = null;
            int mailId = 1;
            var customMessage = "Schedule Mail #1";

            // Act
            logService.LogError(new InvalidOperationException(Message), schedule, mailId);

            // Assert
            mockLogger.Received(1).Error(
                Arg.Compat.Is<string>(x => x.Contains(customMessage)));
        }

        [Fact]
        public void LogError_ExceptionAndScheduleMailId_CallErrorOfLogger()
        {
            // Arrange
            var schedule = new Schedule
            {
                Id = 1,
                Name = "Mail Test",
                TriggerType = (int)MailType.Schedule,
                TriggerTypeEnum = MailType.Schedule
            };
            int mailId = 1;
            var customMessage = "Schedule Mail #1 (Mail Test)";

            // Act
            logService.LogError(new InvalidOperationException(Message), schedule, mailId);

            // Assert
            mockLogger.Received(1).Error(
                Arg.Compat.Is<string>(x => x.Contains(customMessage)));
        }
    }
}
