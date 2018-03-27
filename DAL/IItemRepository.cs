using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;

namespace DAL
{
  public interface IItemRepository
  {
    void CreatePerson(Person person);
    void CreateOrganization(Organization organization);
    void CreateTheme(Theme theme);
    Person ReadPerson(int id);
    Organization ReadOrganization(int id);
    Theme ReadTheme(int id);
    void UpdatePerson(Person person);
    void UpdateOrganization(Organization organisation);
    void UpdateTheme(Theme theme);
    void DeletePerson(Person person);
    void DeleteOrganization(Organization organization);
    void DeleteTheme(Theme theme);



  }
}
