using System.Collections.Generic;
using Fanex.MailDelivery.Admin.Messaging;
using Fanex.MailDelivery.Admin.Models;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Server.Criteria.Group;
using Fanex.MailDelivery.Server.Criteria.Mail;
using Fanex.MailDelivery.Server.Criteria.Recipient;
using Fanex.MailDelivery.Server.Criteria.Report;
using Fanex.MailDelivery.Server.Repositories;

namespace Fanex.MailDelivery.Server.Services
{
    /// <summary>
    /// Admin Service.
    /// </summary>
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminService()
        {
            _adminRepository = _adminRepository ?? new AdminRepository();
        }

        public List<RecipientModel> GetListRecipient(GetRecipientListCriteria criteria)
        {
            return _adminRepository.GetRecipientList(criteria);
        }

        public RecipientModel GetRecipientById(GetRecipientByIdCriteria criteria)
        {
            return _adminRepository.GetRecipientById(criteria);
        }

        public ResultResponse CreateRecipient(CreateRecipientCriteria criteria)
        {
            return _adminRepository.CreateRecipient(criteria);
        }

        public ResultResponse UpdateRecipient(UpdateRecipientCriteria criteria)
        {
            return _adminRepository.UpdateRecipient(criteria);
        }

        public ResultResponse DeleteRecipient(DeleteRecipientCriteria criteria)
        {
            return _adminRepository.DeleteRecipient(criteria);
        }

        public List<MailModel> GetListMail(GetMailListCriteria criteria)
        {
            return _adminRepository.GetMailList(criteria);
        }

        public MailModel GetMailById(GetMailByIdCriteria criteria)
        {
            return _adminRepository.GetMailById(criteria);
        }

        public ResultResponse CreateMail(CreateMailCriteria criteria)
        {
            return _adminRepository.CreateMail(criteria);
        }

        public ResultResponse UpdateMail(UpdateMailCriteria criteria)
        {
            return _adminRepository.UpdateMail(criteria);
        }

        public ResultResponse DeleteMail(DeleteMailCriteria criteria)
        {
            return _adminRepository.DeleteMail(criteria);
        }

        public RecipientModel RecipientById(GetRecipientByIdCriteria criteria)
        {
            return _adminRepository.RecipientById(criteria);
        }

        public IList<GroupModel> GetGroupList(GetGroupListCriteria criteria)
        {
            return _adminRepository.GetGroupList(criteria);
        }

        public GroupModel GetGroupById(GetGroupByIdCriteria criteria)
        {
            return _adminRepository.GetGroupById(criteria);
        }

        public ResultResponse CreateGroup(CreateGroupCriteria criteria)
        {
            return _adminRepository.CreateGroup(criteria);
        }

        public ResultResponse UpdateGroup(UpdateGroupCriteria criteria)
        {
            return _adminRepository.UpdateGroup(criteria);
        }

        public ResultResponse DeleteGroup(DeleteGroupCriteria criteria)
        {
            return _adminRepository.DeleteGroup(criteria);
        }

        public IList<MailTaskModel> GetReportList(GetReportListCriteria criteria)
        {
            return _adminRepository.GetReportList(criteria);
        }
    }
}