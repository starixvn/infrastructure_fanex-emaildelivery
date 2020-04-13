using Fanex.MailDelivery.Core.Models;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.Services.Services
{
    public interface UMService
    {
        Task<UMInfoModel> GetUMInfo(int retryCount = 0);

        Task WaitForUMOff();
    }

    public class UMServiceImpl : UMService
    {
        private const int UMGMT = 8;
        private const int RetryIntervalInSeconds = 10;
        private readonly LogService logService;
        private readonly string umApiBaseUrl;

        public UMServiceImpl(string umApiBaseUrl, LogService logService)
        {
            this.umApiBaseUrl = umApiBaseUrl;
            this.logService = logService;
        }

        public async Task<UMInfoModel> GetUMInfo(int retryCount = 0)
        {
            UMInfoModel result;
            bool isFailed = false;

            try
            {
                var client = new RestClient(new Uri(umApiBaseUrl));
                var request = new RestRequest("UnderMaintenance/getall", DataFormat.Json);

                var allUMInfos = await client.GetAsync<Dictionary<string, UMInfoModel>>(request);

                result = allUMInfos.Values.FirstOrDefault(x => x.IsUnderMaintenanceTime);

                if (result != null)
                {
                    result.From = result.From.AddHours(UMGMT * -1); //Convert to UTC
                    result.To = result.To.AddHours(UMGMT * -1);
                }
                else
                {
                    result = new UMInfoModel();
                }
            }
            catch (Exception ex)
            {
                logService.LogError($"Cannot get UM info -> Error: {ex.InnerException?.Message ?? ex.Message}", ex);
                isFailed = true;
                result = new UMInfoModel();
            }

            if (isFailed && retryCount == 0)
            {
                retryCount++;

                await Task.Delay(TimeSpan.FromSeconds(RetryIntervalInSeconds));

                result = await GetUMInfo(retryCount);
            }

            return result;
        }

        public async Task WaitForUMOff()
        {
            var umInfo = await GetUMInfo();
            bool isUM = umInfo != null && umInfo.IsUnderMaintenanceTime;

            if (isUM)
            {
                var afterUMTime = new DateTime(umInfo.To.Ticks, DateTimeKind.Utc).AddMinutes(1);
                TimeSpan waitingTime = afterUMTime - DateTime.UtcNow;

                if (waitingTime.TotalSeconds > 0)
                {
                    logService.LogInfo($"Waiting for UM off in {waitingTime.TotalMinutes} minutes before continue...");

                    await Task.Delay(waitingTime);
                }
            }
        }
    }
}
