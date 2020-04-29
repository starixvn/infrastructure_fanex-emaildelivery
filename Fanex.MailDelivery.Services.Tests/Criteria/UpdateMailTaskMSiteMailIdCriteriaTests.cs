using Fanex.MailDelivery.Services.Criteria;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Criteria
{
    public class UpdateMailTaskMSiteMailIdCriteriaTests
    {
        private readonly UpdateMailTaskMSiteMailIdCriteria criteria;

        public UpdateMailTaskMSiteMailIdCriteriaTests()
        {
            criteria = new UpdateMailTaskMSiteMailIdCriteria();
        }

        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            // Arrange
            var expected = "Mail_Core_UpdateMailTaskMSiteMailID";

            // Act
            var spName = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, spName);
        }
    }
}
