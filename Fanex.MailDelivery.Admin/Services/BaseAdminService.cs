using Fanex.MailDelivery.Services.Services.Admin;

namespace Fanex.MailDelivery.Admin.Services
{
    public abstract class BaseAdminService
    {      
        protected readonly IAdminDataService adminDataService;

        protected BaseAdminService() : this(null)
        {
        }

        protected BaseAdminService(IAdminDataService adminDataService)
        {
            this.adminDataService = adminDataService ?? new AdminDataService();
        }
    }
}