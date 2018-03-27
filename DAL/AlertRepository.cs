using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class AlertRepository : IAlertRepository
    {

      IList<Alert> Alerts { get; set; }

        public AlertRepository()
        {
          Alerts = new List<Alert>();

        }

      public Alert ReadAlert(int alertId)
      {
      return Alerts.First(a => a.AlertId == alertId);
      }

    public void CreateAlert(Alert alert)
    {
      Alerts.Add(alert);
    }

    public void DeleteAlert(Alert alert)
    {
      Alerts.Remove(alert);
      
    }

    

    public void UpdateAlert(Alert alert)
    {
      Alert a = ReadAlert(alert.AlertId);
    }
  }
}
