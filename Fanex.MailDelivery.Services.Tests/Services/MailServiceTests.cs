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
    }
}
