using Fanex.MailDelivery.MailStationService.Jobs;
using Fanex.MailDelivery.Services.Services;
using NSubstitute;
using Quartz;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.MailDelivery.MailStationService.Tests.Jobs
{
    public class UpdateFailedForLongRunningTaskJobTests
    {
        private readonly MailService mockMailService;

        public UpdateFailedForLongRunningTaskJobTests()
        {
            mockMailService = Substitute.For<MailService>();
        }

        [Fact]
        public async Task Execute_Any_CallUpdateFailedForLongRunningMailTaskOfService()
        {
            // Arrange
            var job = new UpdateFailedForLongRunningTaskJob(mockMailService);
            var context = Substitute.For<IJobExecutionContext>();

            // Act
            await job.Execute(context);

            // Assert
            mockMailService.Received(1).UpdateFailedForLongRunningMailTask();
        }
    }
}
