using Fanex.MailDeliver.Business.Interface;
using Fanex.MailDelivery.Core.Messaging;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Repositories;
using Fanex.MailDelivery.Services.Services;
using NSubstitute;
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
    }
}
