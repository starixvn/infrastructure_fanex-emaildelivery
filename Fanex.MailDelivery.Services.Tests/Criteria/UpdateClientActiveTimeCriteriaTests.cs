using Fanex.MailDelivery.Services.Criteria;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Criteria
{
    public class UpdateClientActiveTimeCriteriaTests
    {
        private readonly UpdateClientActiveTimeCriteria criteria;

        public UpdateClientActiveTimeCriteriaTests()
        {
            criteria = new UpdateClientActiveTimeCriteria();
        }

        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            // Arrange
            var expected = "Mail_Core_ClientActiveTimeUpdate";

            // Act
            var spName = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, spName);
        }
    }
}
