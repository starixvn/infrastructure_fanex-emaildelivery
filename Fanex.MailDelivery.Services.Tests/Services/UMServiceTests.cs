using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Services;
using NSubstitute;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Fanex.MailDelivery.Services.Tests.Services
{
    public class UMServiceTests
    {
        private readonly UMService umService;
        private readonly IRestClient mockClient;
        private readonly LogService mockLogService;

        public UMServiceTests()
        {
            mockClient = Substitute.For<IRestClient>();
            mockLogService = Substitute.For<LogService>();
            umService = new UMServiceImpl(string.Empty, mockLogService, mockClient);
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
            var response = new RestResponse<Dictionary<string, UMInfoModel>>
            {
                Data = result,
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = HttpStatusCode.OK
            };
            mockClient.ExecuteGetAsync<Dictionary<string, UMInfoModel>>(Arg.Compat.Any<IRestRequest>()).Returns(response);

            // Act
            var actual = await umService.GetUMInfo();

            // Assert
            Assert.True(actual.From.Equals(expectedFrom) && actual.To.Equals(expectedTo));
        }

        [Fact]
        public async Task GetUMInfo_Any_RetryIfFailedFirst()
        {
            // Arrange                    
            var response = new RestResponse<Dictionary<string, UMInfoModel>>
            {
                Data = null,
                ErrorException = new InvalidOperationException(),
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = HttpStatusCode.NotFound
            };
            mockClient.ExecuteGetAsync<Dictionary<string, UMInfoModel>>(Arg.Compat.Any<IRestRequest>()).Returns(response);

            // Act
            await umService.GetUMInfo();

            // Assert
            await mockClient.Received(2).ExecuteGetAsync<Dictionary<string, UMInfoModel>>(Arg.Compat.Any<IRestRequest>());
        }

        [Fact]
        public async Task GetUMInfo_Any_CallRightUrl()
        {
            // Arrange           
            var url = "UnderMaintenance/getall";
            var result = new Dictionary<string, UMInfoModel>();
            result["123"] = new UMInfoModel
            {
                From = Convert.ToDateTime("2020/1/1 8:00 AM"),
                To = Convert.ToDateTime("2020/1/1 10:00 AM"),
                IsUnderMaintenanceTime = true
            };
            var response = new RestResponse<Dictionary<string, UMInfoModel>>
            {
                Data = result,
                ResponseStatus = ResponseStatus.Completed,
                StatusCode = HttpStatusCode.OK
            };
            mockClient.ExecuteGetAsync<Dictionary<string, UMInfoModel>>(Arg.Compat.Any<IRestRequest>()).Returns(response);

            // Act
            await umService.GetUMInfo();

            // Assert
            await mockClient.Received(1).ExecuteGetAsync<Dictionary<string, UMInfoModel>>(
                 Arg.Compat.Is<IRestRequest>(x => x.Resource.Equals(url, StringComparison.OrdinalIgnoreCase)));
        }
    }
}
