using System.Collections.Generic;
using BL.Domain;

namespace BL
{
  public interface IItemManager
  {
    Person AddPerson(Person person);
    List<Item> GetAllItemsFromPosts(List<SocialMediaPost> data);
    Organization ReadOrganization(int id);
    Person ReadPerson(int id);
    Theme ReadTheme(int id);
    IEnumerable<Item> SearchItems(string search);
    Organization AddOrganization(Organization organization);
    void RemoveOrganization(Organization organization);
void ChangeItem(Item i);
    List<Organization> GetOrganizations();
    List<Person> GetPersons();
    void RemoveItem(Item i);
    Item GetItem(int id);
  }
}