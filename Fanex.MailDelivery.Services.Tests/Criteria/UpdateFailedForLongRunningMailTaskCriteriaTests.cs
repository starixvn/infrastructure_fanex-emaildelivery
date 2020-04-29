using Fanex.MailDelivery.Services.Criteria;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Criteria
{
    public class UpdateFailedForLongRunningMailTaskCriteriaTests
    {
        private readonly UpdateFailedForLongRunningMailTaskCriteria criteria;

        public UpdateFailedForLongRunningMailTaskCriteriaTests()
        {
            criteria = new UpdateFailedForLongRunningMailTaskCriteria();
        }

        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            // Arrange
            var expected = "Mail_Core_UpdateFailedForLongRunningTasks";

            // Act
            var spName = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, spName);
        }
    }
}
