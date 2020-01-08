using Fanex.MailDelivery.Admin.Mapping;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Messaging.ManageRecipient;
using Fanex.MailDelivery.Core.Models.Recipient;
using Fanex.MailDelivery.Services.Criteria.Recipient;
using System.Collections.Generic;

namespace Fanex.MailDelivery.Admin.Services
{
    public interface IRecipientService
    {
        GetRecipientListResponse GetRecipientList(GetRecipientListRequest request);

        GetRecipientByIdResponse GetRecipientById(GetRecipientByIdRequest request);

        ResultResponse CreateRecipient(CreateRecipientRequest request);

        ResultResponse UpdateRecipient(UpdateRecipientRequest request);

        ResultResponse DeleteRecipient(DeleteRecipientRequest request);
    }

    public class RecipientService : BaseAdminService, IRecipientService
    {
        public GetRecipientListResponse GetRecipientList(GetRecipientListRequest request)
        {
            IList<RecipientModel> recipientModels = adminDataService.GetListRecipient(new GetRecipientListCriteria
            {
                Emails = request.Emails,
                Status = request.Status,
                UserName = request.UserName
            });

            GetRecipientListResponse getRecipientListResponse = new GetRecipientListResponse
            {
                RecipientModels = recipientModels
            };

            return getRecipientListResponse;
        }

        public GetRecipientByIdResponse GetRecipientById(GetRecipientByIdRequest request)
        {
            RecipientModel recipientModel = adminDataService.GetRecipientById(new GetRecipientByIdCriteria
            {
                RecipientId = request.Id
            });

            return new GetRecipientByIdResponse { RecipientModel = recipientModel };
        }

        public ResultResponse CreateRecipient(CreateRecipientRequest request)
        {
            return adminDataService.CreateRecipient(new CreateRecipientCriteria()
            {
                Email1 = request.Email1,
                Email2 = request.Email2,
                Email3 = request.Email3,
                IsNAPUser = request.IsNapUser,
                NAPUserID = request.NapUserId,
                Note = request.Note,
                Status = request.Status,
                UserName = request.UserName
            }).ToResultResponse();
        }

        public ResultResponse UpdateRecipient(UpdateRecipientRequest request)
        {
            return adminDataService.UpdateRecipient(new UpdateRecipientCriteria()
            {
                RecipientId = request.RecipientId,
                Username = request.UserName,

                EmailID1 = request.EmailId1,
                EmailID2 = request.EmailId2,
                EmailID3 = request.EmailId3,
                Email1 = request.Email1,
                Email2 = request.Email2,
                Email3 = request.Email3,

                Note = request.Note,
                Status = request.Status,
            }).ToResultResponse();
        }

        public ResultResponse DeleteRecipient(DeleteRecipientRequest request)
        {
            return adminDataService.DeleteRecipient(new DeleteRecipientCriteria
            {
                RecipientId = request.RecipientId
            }).ToResultResponse();
        }
    }
}