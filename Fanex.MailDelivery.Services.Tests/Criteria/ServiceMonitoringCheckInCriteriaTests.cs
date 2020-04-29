using Fanex.MailDelivery.Services.Criteria;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Criteria
{
    public class ServiceMonitoringCheckInCriteriaTests
    {
        private readonly ServiceMonitoringCheckInCriteria criteria;

        public ServiceMonitoringCheckInCriteriaTests()
        {
            criteria = new ServiceMonitoringCheckInCriteria();
        }

        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            // Arrange
            var expected = "ServiceMonitoring_CheckIn";

            // Act
            var spName = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, spName);
        }
    }
}
