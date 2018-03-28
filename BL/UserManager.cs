using BL.Domain;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class UserManager
    {
        private AlertRepository alertRepository;
        private SocialMediaManager socialMediaManager;

        public UserManager(SocialMediaManager socialMediaManager)
        {
            alertRepository = new AlertRepository();
            this.socialMediaManager = socialMediaManager;
        }

        public List<Alert> GetAlerts(Item item)
        {
           return alertRepository.GetAlerts(item);
        }

        public void InspectAlert(Alert alert)
        {
            socialMediaManager.VerifyCondition(alert);
        }
    }
}
