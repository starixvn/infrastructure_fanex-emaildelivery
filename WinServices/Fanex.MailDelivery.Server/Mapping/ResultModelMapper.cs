using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Services.Models;

namespace Fanex.MailDelivery.AdminWinService.Mapping
{
    public static class ResultModelMapper
    {
        public static ResultResponse ToResultResponse(this ResultModel resultModel)
        {
            return new ResultResponse
            {
                ErrCode = resultModel.ErrCode,
                ErrMsg = resultModel.ErrMsg
            };
        }
    }
}
