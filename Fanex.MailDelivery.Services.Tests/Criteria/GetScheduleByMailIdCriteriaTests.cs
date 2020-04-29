using Fanex.MailDelivery.Services.Criteria.Job;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Criteria
{
    public class GetScheduleByMailIdCriteriaTests
    {
        private readonly GetScheduleByMailIdCriteria criteria;

        public GetScheduleByMailIdCriteriaTests()
        {
            criteria = new GetScheduleByMailIdCriteria();
        }

        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            // Arrange
            var expected = "Mail_MailManagement_GetScheduleByMailId";

            // Act
            var spName = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, spName);
        }
    }
}
