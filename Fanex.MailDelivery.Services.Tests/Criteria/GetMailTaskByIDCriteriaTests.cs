using Fanex.MailDelivery.Services.Criteria;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Criteria
{
    public class GetMailTaskByIDCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            // Arrange
            var expected = "Mail_Core_GetMailTask";

            // Act
            var spName = new GetMailTaskByIdCriteria().GetSettingKey();

            // Assert
            Assert.Equal(expected, spName);
        }
    }
}
