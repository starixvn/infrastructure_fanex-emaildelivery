using Fanex.MailDeliver.Model;
using Fanex.MailDelivery.Services.Models;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Models
{
    public class RetryMailRecordTests
    {
        private readonly RetryMailRecord model;

        public RetryMailRecordTests()
        {
            model = new RetryMailRecord(new MailRecord(), 3);
        }

        [Theory]
        [InlineData(1)]
        [InlineData(2)]
        [InlineData(3)]
        public void CanRetry_RetryTimeLessThanOrEqualsMaxRetry_ReturnTrue(int retryTime)
        {
            // Arrange
            model.RetryTime = retryTime;

            // Assert
            Assert.True(model.CanRetry);
        }

        [Theory]
        [InlineData(4)]
        [InlineData(5)]
        public void CanRetry_RetryTimeGreaterThanMaxRetry_ReturnsFalse(int retryTime)
        {
            // Arrange
            model.RetryTime = retryTime;

            // Assert
            Assert.False(model.CanRetry);
        }
    }
}
