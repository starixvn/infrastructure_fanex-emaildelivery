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
        Task<UMInfoModel> GetUMInfo();

        Task WaitForUMOff();
    }

    public class UMServiceImpl : UMService
    {
        private readonly LogService logService;
        private readonly string umApiBaseUrl;
        private const int UMGMT = 8;
        private readonly int checkUMIntervalInMinutes;

        public UMServiceImpl(string umApiBaseUrl, int checkUMIntervalInMinutes, LogService logService)
        {
            this.umApiBaseUrl = umApiBaseUrl;
            this.logService = logService;
            this.checkUMIntervalInMinutes = checkUMIntervalInMinutes;
        }

        public async Task<UMInfoModel> GetUMInfo()
        {
            UMInfoModel result;

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

                result = new UMInfoModel();
            }

            return result;
        }

        public async Task WaitForUMOff()
        {
            var isUM = true;

            while (isUM)
            {
                var umInfo = await GetUMInfo();
                isUM = umInfo != null && umInfo.IsUnderMaintenanceTime;

                if (isUM)
                {
                    await Task.Delay(TimeSpan.FromMinutes(checkUMIntervalInMinutes));
                }
            }
        }
    }
}
