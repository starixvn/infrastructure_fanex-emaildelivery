using Fanex.MailDelivery.Core.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Fanex.MailDelivery.Services.Services
{
    public interface UMService
    {
        Task<UMInfoModel> GetUMInfo(int retryCount = 0);

        Task WaitForUMOff();
    }

    public class UMServiceImpl : UMService, IDisposable
    {
        private const int UMGMT = 8;
        private const int RetryIntervalInSeconds = 10;
        private readonly LogService logService;
        private IHttpClientWrapper client;
        private readonly Uri umApiUrl;

        public UMServiceImpl(string umApiUrl, LogService logService) : this(umApiUrl, logService, null)
        {
        }

        public UMServiceImpl(string umApiUrl, LogService logService, IHttpClientWrapper client)
        {
            this.umApiUrl = new Uri(umApiUrl);
            this.logService = logService;
            this.client = client ?? new HttpClientWrapper();
        }

        public async Task<UMInfoModel> GetUMInfo(int retryCount = 0)
        {
            UMInfoModel result;
            bool isFailed = false;

            try
            {
                string content = await client.GetAsync(umApiUrl).ConfigureAwait(false);
                var umData = JsonConvert.DeserializeObject<Dictionary<string, UMInfoModel>>(content);
                result = umData.Values.FirstOrDefault(x => x.IsUnderMaintenanceTime);

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
                    logService.LogInfo($"Waiting for UM off in {(int)waitingTime.TotalMinutes} minutes before continue...");

                    await Task.Delay(waitingTime);
                }
            }
        }

        #region IDisposable Support
        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    client?.Dispose();
                }

                client = null;
                disposedValue = true;
            }
        }

        ~UMServiceImpl()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
