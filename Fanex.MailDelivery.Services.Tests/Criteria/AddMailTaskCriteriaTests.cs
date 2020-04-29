using Fanex.MailDelivery.Services.Criteria;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Criteria
{
    public class AddMailTaskCriteriaTests
    {
        private readonly AddMailTaskCriteria criteria;

        public AddMailTaskCriteriaTests()
        {
            criteria = new AddMailTaskCriteria();
        }

        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            // Arrange
            var expected = "Mail_Core_InsertMailTask";

            // Act
            var spName = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, spName);
        }
    }
}
