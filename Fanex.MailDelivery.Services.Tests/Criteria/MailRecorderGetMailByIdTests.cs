using Fanex.MailDelivery.Services.Criteria;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Criteria
{
    public class MailRecorderGetMailByIdTests
    {
        private readonly MailRecorderGetMailById criteria;

        public MailRecorderGetMailByIdTests()
        {
            criteria = new MailRecorderGetMailById();
        }

        [Fact]
        public void GetSettingKey_Any_ReturnRightSPName()
        {
            // Arrange
            var expected = "GetMail";

            // Act
            var spName = criteria.GetSettingKey();

            // Assert
            Assert.Equal(expected, spName);
        }
    }
}
