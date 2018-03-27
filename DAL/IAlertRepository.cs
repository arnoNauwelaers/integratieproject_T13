using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;

namespace DAL
{
  public interface IAlertRepository
  {
    void CreateAlert(Alert alert);
    Alert ReadAlert(int alertId);
    void UpdateAlert(Alert alert);
    void DeleteAlert(Alert alert);
  }
}
