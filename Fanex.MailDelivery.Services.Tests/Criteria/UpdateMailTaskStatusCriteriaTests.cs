using Fanex.MailDelivery.Services.Criteria;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Criteria
{
    public class UpdateMailTaskStatusCriteriaTests
    {
        private readonly UpdateMailTaskStatusCriteria criteria;

        public UpdateMailTaskStatusCriteriaTests()
        {
            criteria = new UpdateMailTaskStatusCriteria();
        }

        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            // Arrange
            var expected = "Mail_Core_UpdateMailStatus";

            // Act
            var spName = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, spName);
        }
    }
}
