using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Services;
using Newtonsoft.Json;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Services
{
    public class UMServiceTests
    {
        private readonly UMService umService;
        private readonly IHttpClientWrapper mockHttpClient;
        private readonly LogService mockLogService;

        public UMServiceTests()
        {
            mockHttpClient = Substitute.For<IHttpClientWrapper>();
            mockLogService = Substitute.For<LogService>();
            umService = new UMServiceImpl("http://UMApiUrl.com", mockLogService, mockHttpClient);
        }

        [Fact]
        public async Task GetUMInfo_Any_ReturnUtcDateTime()
        {
            // Arrange
            var expectedFrom = Convert.ToDateTime("2020/1/1 12:00 AM");
            var expectedTo = Convert.ToDateTime("2020/1/1 2:00 AM");
            var result = new Dictionary<string, UMInfoModel>();
            result["123"] = new UMInfoModel
            {
                From = Convert.ToDateTime("2020/1/1 8:00 AM"),
                To = Convert.ToDateTime("2020/1/1 10:00 AM"),
                IsUnderMaintenanceTime = true
            };
            mockHttpClient.GetAsync(Arg.Any<Uri>()).Returns(JsonConvert.SerializeObject(result));

            // Act
            var actual = await umService.GetUMInfo();

            // Assert
            Assert.True(actual.From.Equals(expectedFrom) && actual.To.Equals(expectedTo));
        }

        [Fact]
        public async Task GetUMInfo_Any_RetryIfFailedFirst()
        {
            // Arrange                    
            mockHttpClient.When(x => x.GetAsync(Arg.Any<Uri>())).Throw(new InvalidOperationException());

            // Act
            await umService.GetUMInfo();

            // Assert
            await mockHttpClient.Received(2).GetAsync(Arg.Any<Uri>());
        }
    }
}
