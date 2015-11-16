using System.Collections.Generic;
using Fanex.MailDelivery.Admin.Criteria.Group;
using Fanex.MailDelivery.Admin.Criteria.Mail;
using Fanex.MailDelivery.Admin.Criteria.Recipient;
using Fanex.MailDelivery.Admin.Models;
using Fanex.MailDelivery.Admin.Repositories;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Services.Messaging;

namespace Fanex.MailDelivery.Admin.Services
{
    /// <summary>
    /// Admin Service.
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _mailRepository;

        public AdminService()
        {
            _mailRepository = _mailRepository ?? new AdminRepository();
        }

        public List<RecipientModel> GetListRecipient(RecipientListCriteria criteria)
        {
            return _mailRepository.GetListRecipient(criteria);
        }

        public ResultResponse CreateRecipient(CreateRecipientCriteria criteria)
        {
            return _mailRepository.CreateRecipient(criteria);
        }

        public ResultResponse UpdateRecipient(UpdateRecipientCriteria criteria)
        {
            return _mailRepository.UpdateRecipient(criteria);
        }

        public ResultResponse DeleteRecipient(DeleteRecipientCriteria criteria)
        {
            return _mailRepository.DeleteRecipient(criteria);
        }

        public List<MailModel> GetListMail(GetMailListCriteria criteria)
        {
            return _mailRepository.GetListMail(criteria);
        }

        public ResultResponse CreateMail(CreateMailCriteria criteria)
        {
            return _mailRepository.CreateMail(criteria);
        }

        public ResultResponse UpdateMail(UpdateMailCriteria criteria)
        {
            return _mailRepository.UpdateMail(criteria);
        }

        public ResultResponse DeleteMail(DeleteMailCriteria criteria)
        {
            return _mailRepository.DeleteMail(criteria);
        }

        public RecipientModel RecipientById(RecipientByIdCriteria criteria)
        {
            return _mailRepository.RecipientById(criteria);
        }

        public IList<GroupModel> GetGroupList(GetGroupListCriteria criteria)
        {
            return _mailRepository.GetGroupList(criteria);
        }

        public ResultResponse CreateGroup(CreateGroupCriteria criteria)
        {
            return _mailRepository.CreateGroup(criteria);
        }

        public ResultResponse UpdateGroup(UpdateGroupCriteria criteria)
        {
            return _mailRepository.UpdateGroup(criteria);
        }

        public ResultResponse DeleteGroup(DeleteGroupCriteria criteria)
        {
            return _mailRepository.DeleteGroup(criteria);
        }
    }
}