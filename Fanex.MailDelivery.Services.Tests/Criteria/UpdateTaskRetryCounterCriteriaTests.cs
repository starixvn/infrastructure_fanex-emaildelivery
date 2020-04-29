using Fanex.MailDelivery.Services.Criteria;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Criteria
{
    public class UpdateTaskRetryCounterCriteriaTests
    {
        private readonly UpdateTaskRetryCounterCriteria criteria;

        public UpdateTaskRetryCounterCriteriaTests()
        {
            criteria = new UpdateTaskRetryCounterCriteria();
        }

        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            // Arrange
            var expected = "Mail_Core_UpdateRetry";

            // Act
            var spName = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, spName);
        }
    }
}
