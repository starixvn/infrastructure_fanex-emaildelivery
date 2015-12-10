using System.Collections.Generic;
using Fanex.MailDelivery.Core.Models;
using Fanex.MailDelivery.Core.Models.Group;
using Fanex.MailDelivery.Core.Models.Mails;
using Fanex.MailDelivery.Core.Models.Recipient;
using Fanex.MailDelivery.Core.Models.Report;
using Fanex.MailDelivery.Services.Criteria.Criteria;
using Fanex.MailDelivery.Services.Criteria.Group;
using Fanex.MailDelivery.Services.Criteria.Job;
using Fanex.MailDelivery.Services.Criteria.Mail;
using Fanex.MailDelivery.Services.Criteria.Recipient;
using Fanex.MailDelivery.Services.Criteria.Report;
using Fanex.MailDelivery.Services.Models;
using Fanex.MailDelivery.Services.Repositories;
using Fanex.MailDelivery.Services.Repositories.Admin;

namespace Fanex.MailDelivery.Services.Services.Admin
{
    /// <summary>
    /// Admin Service.
    /// </summary>
    public class AdminDataService : IAdminDataService
    {
        private readonly IAdminRepository _adminRepository;

        public AdminDataService()
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

        public ResultModel CreateRecipient(CreateRecipientCriteria criteria)
        {
            return _adminRepository.CreateRecipient(criteria);
        }

        public ResultModel UpdateRecipient(UpdateRecipientCriteria criteria)
        {
            return _adminRepository.UpdateRecipient(criteria);
        }

        public ResultModel DeleteRecipient(DeleteRecipientCriteria criteria)
        {
            return _adminRepository.DeleteRecipient(criteria);
        }

        public IList<MailModel> GetListMail(GetMailListCriteria criteria)
        {
            return _adminRepository.GetMailList(criteria);
        }

        public MailModel GetMailById(GetMailByIdCriteria criteria)
        {
            return _adminRepository.GetMailById(criteria);
        }

        public ResultModel CreateMail(CreateMailCriteria criteria)
        {
            return _adminRepository.CreateMail(criteria);
        }

        public ResultModel UpdateMail(UpdateMailCriteria criteria)
        {
            return _adminRepository.UpdateMail(criteria);
        }

        public ResultModel DeleteMail(DeleteMailCriteria criteria)
        {
            return _adminRepository.DeleteMail(criteria);
        }

        public IList<GroupModel> GetGroupList(GetGroupListCriteria criteria)
        {
            return _adminRepository.GetGroupList(criteria);
        }

        public GroupModel GetGroupById(GetGroupByIdCriteria criteria)
        {
            return _adminRepository.GetGroupById(criteria);
        }

        public ResultModel CreateGroup(CreateGroupCriteria criteria)
        {
            return _adminRepository.CreateGroup(criteria);
        }

        public ResultModel UpdateGroup(UpdateGroupCriteria criteria)
        {
            return _adminRepository.UpdateGroup(criteria);
        }

        public ResultModel DeleteGroup(DeleteGroupCriteria criteria)
        {
            return _adminRepository.DeleteGroup(criteria);
        }

        public IList<MailTaskModel> GetReportList(GetReportListCriteria criteria)
        {
            return _adminRepository.GetReportList(criteria);
        }

        public Schedule GetScheduleByMailId(GetScheduleByMailIdCriteria criteria)
        {
            throw new System.NotImplementedException();
        }
    }
}