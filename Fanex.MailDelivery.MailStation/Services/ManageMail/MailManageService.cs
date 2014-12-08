using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Fanex.MailDelivery.MailStation.Models;
using Fanex.Data;
using Fanex.MailDelivery.MailStation.Criterias;

namespace Fanex.MailDelivery.MailStation.Services.ManageMail
{
    public class MailManageService
    {
        public List<SiteModel> Sites()
        {
            GetAllSiteCriteria criteria = new GetAllSiteCriteria();
            List<SiteModel> result = new List<SiteModel>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                result = db.Query<SiteModel>().ToList();
            }

            return result;
        }

        public List<MailGroupModel> MailGroups()
        {
            GetAllMailGroupCriteria criteria = new GetAllMailGroupCriteria();
            List<MailGroupModel> result = new List<MailGroupModel>();
            using (ObjectDb db = new ObjectDb(criteria.GetSettingKey()))
            {
                result = db.Query<MailGroupModel>().ToList();
            }

            return result;
        }

        
    }
}
