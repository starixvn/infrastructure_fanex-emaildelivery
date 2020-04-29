using Fanex.MailDelivery.Services.Criteria;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Criteria
{
    public class GetMailWithEmailAddressByMailIDCriteriaTests
    {
        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            // Arrange
            var expected = "Mail_MailManagement_GetMailWithEmailAddressByMailID";

            // Act
            var spName = new GetMailWithEmailAddressByMailId().GetSettingKey();

            // Assert
            Assert.Equal(expected, spName);
        }
    }
}
