using System.Collections.Generic;
using System.Linq;
using DAL;
using BL.Domain;
using DAL.EF;
using System;
using DAL.Repositories;

namespace BL.Managers
{
    public class ItemManager
    {
        private ItemRepository itemRepository;

        public ItemManager()
        {
            itemRepository = RepositoryFactory.CreateItemRepository();
        }

        public IEnumerable<Item> GetItems()
        {
            return itemRepository.ReadItems();
        }

        public List<Item> GetItems(string s)
        {
            return itemRepository.ReadItems(s);
        }

        public IEnumerable<Person> GetPersons()
        {
            return itemRepository.ReadPersons();
        }

        public IEnumerable<Organization> GetOrganizations()
        {
            return itemRepository.ReadOrganizations();
        }

        public IEnumerable<Theme> GetThemes()
        {
            return itemRepository.ReadThemes();
        }

        public List<SocialMediaProfile> GetProfiles(Item item)
        {
            return itemRepository.ReadProfiles(item);
        }

        public Person CreatePersonIfNotExists(string name)
        {
            return itemRepository.CreatePersonIfNotExists(name);
        }

        public void AddItem(string name, string type, int selectedOrganizationId, string keywords, string twitterUrl)
        {
            if (type == "Person")
            {
                Organization tempOrganization = itemRepository.ReadOrganization(selectedOrganizationId);
                Person person = new Person() { Name = name, Organization = tempOrganization };
                Item tempPerson = itemRepository.CreateItem(person);
                if(twitterUrl != null && twitterUrl != " ")
                {
                    SocialMediaProfile socialMediaProfile = new SocialMediaProfile() { Url = twitterUrl, Source = "Twitter", Item = tempPerson };
                    SocialMediaProfile tempSocialMediaProfile = itemRepository.CreateSocialmediaprofile(socialMediaProfile);
                    ((Person)tempPerson).SocialMediaProfiles.Add(tempSocialMediaProfile);
                }
                itemRepository.UpdateItem(tempPerson);
            }
            else if (type == "Organization")
            {
                Organization organization = new Organization() { Name = name };
                Item tempOrganization = itemRepository.CreateItem(organization);
                if (twitterUrl != null && twitterUrl != " ")
                {
                    SocialMediaProfile socialMediaProfile = new SocialMediaProfile() { Url = twitterUrl, Source = "Twitter", Item = tempOrganization };
                    SocialMediaProfile tempSocialMediaProfile = itemRepository.CreateSocialmediaprofile(socialMediaProfile);
                    ((Organization)tempOrganization).SocialMediaProfiles.Add(tempSocialMediaProfile);
                }
                itemRepository.UpdateItem(tempOrganization);
            }
            else if (type == "Theme")
            {
                List<string> keywordsStringList = (keywords.Replace(" ", "").Split(',').ToList<string>());
                List<Keyword> keywordsList = new List<Keyword>();
                Theme theme = new Theme() { Name = name };
                Item item = itemRepository.CreateItem(theme);
                foreach (var keyword in keywordsStringList)
                {
                    Keyword keywordTemp = itemRepository.CreateKeyword(item, keyword);
                    keywordsList.Add(keywordTemp);
                }
                ((Theme)item).Keywords = keywordsList;
                itemRepository.UpdateItem(item);
            }
        }

        public void AddItem(string name, string type, string organization, string keywords)
        {
            if (type.ToUpper() == "PERSOON" || type.ToUpper() == "PERSON")
            {
                List<Organization> tempOrganization = itemRepository.ReadOrganization(organization);
                if (tempOrganization.Count() == 0)
                {
                    Organization newOrganization = new Organization() { Name = organization };
                    itemRepository.CreateItem(newOrganization);
                    Person person = new Person() { Name = name, Organization = newOrganization };
                    itemRepository.CreateItem(person);
                }
                else
                {
                    Person person = new Person() { Name = name, Organization = tempOrganization[0] };
                    itemRepository.CreateItem(person);
                }

            }
            else if (type.ToUpper() == "ORGANISATIE" || type.ToUpper() == "ORGANIZATION")
            {
                if (itemRepository.ReadOrganization(name).Count() == 0)
                {
                    Organization tempOrganization = new Organization() { Name = name };
                    itemRepository.CreateItem(tempOrganization);
                }
            }
            else if (type.ToUpper() == "THEMA" || type.ToUpper() == "THEME")
            {
                if (itemRepository.ReadTheme(name).Count() == 0)
                {
                    List<string> keywordsStringList = (keywords.Replace(" ", "").Split(',').ToList<string>());
                    List<Keyword> keywordsList = new List<Keyword>();
                    Theme theme = new Theme() { Name = name };
                    Item item = itemRepository.CreateItem(theme);
                    foreach (var keyword in keywordsStringList)
                    {
                        Keyword keywordTemp = itemRepository.CreateKeyword(item, keyword);
                        keywordsList.Add(keywordTemp);
                    }
                    ((Theme)item).Keywords = keywordsList;
                    itemRepository.UpdateItem(item);
                }
            }
            else
            {
                throw new Exception();
            }
        }

        public Item ReadItem(int id)
        {
            return itemRepository.ReadItem(id);
        }

        public Item EditItem(int id, string name, string type, int selectedOrganizationId, IEnumerable<int> selectedKeywords, string stringKeywords)
        {
            if (type == "Persoon")
            {
                Organization tempOrganization = itemRepository.ReadOrganization(selectedOrganizationId);
                Person person = new Person() { ItemId = id, Name = name, Organization = tempOrganization };
                return itemRepository.UpdateItem(person);
            }
            else if (type == "Organisatie")
            {
                Organization organization = new Organization() { ItemId = id, Name = name };
                return itemRepository.UpdateItem(organization);
            }
            else if (type == "Thema")
            {
                Theme TempTheme = itemRepository.ReadTheme(id);
                List<Keyword> keywordsList = TempTheme.Keywords.ToList();

                if (stringKeywords != null && stringKeywords != " ")
                {
                    List<string> keywordsStringList = (stringKeywords.Replace(" ", "").Split(',').ToList<string>());
                    foreach (var keyword in keywordsStringList)
                    {
                        Keyword keywordTemp = itemRepository.CreateKeyword(TempTheme, keyword);
                        keywordsList.Add(keywordTemp);
                    }
                }

                if (selectedKeywords != null)
                {
                    foreach (int KeywordId in selectedKeywords)
                    {
                        if (KeywordId != 0)
                        {
                            keywordsList.Remove(TempTheme.Keywords.Single(k => k.Id == KeywordId));
                            itemRepository.DeleteKeyword(KeywordId);
                        }

                    }
                }
                Theme theme = new Theme() { ItemId = id, Name = name, Keywords = keywordsList };
                return itemRepository.UpdateItem(theme);
            }
            throw new Exception();
        }

        public void EditProfiles(List<int> profileIds, string url)
        {
            if(profileIds != null && profileIds.Count != 0 && url != null && url != " ")
            {
                foreach (var profileId in profileIds)
                {
                    SocialMediaProfile tempProfile = itemRepository.ReadProfiles(profileId);
                    tempProfile.Url = url;
                    itemRepository.UpdateProfile(tempProfile);
                }
            }
        }

        public void AddProfileToItem(Item item, string TwitterUrl)
        {
            SocialMediaProfile tempProfile = new SocialMediaProfile() { Source = "Twitter", Url = TwitterUrl, Item = item};
            itemRepository.CreateSocialmediaprofile(tempProfile);
            if (item.GetType().ToString().Contains("Person"))
            {
                ((Person)item).SocialMediaProfiles.Add(tempProfile);
            }else if (item.GetType().ToString().Contains("Organization"))
            {
                ((Organization)item).SocialMediaProfiles.Add(tempProfile);
            }
            itemRepository.UpdateItem(item);
        }

        public void DeleteProfiles(List<int> profileIds)
        {
            foreach(var profileId in profileIds)
            {
                itemRepository.DeleteProfile(profileId);
            }
        }

        public void DeleteItem(int itemId)
        {
            itemRepository.DeleteItem(itemId);
        }



        public List<Item> GetAllItemsFromPosts(List<SocialMediaPost> data)
        {
            Dictionary<int, Item> alteredItems = new Dictionary<int, Item>();
            foreach (var post in data)
            {
                List<Item> items = itemRepository.ReadItems(post);
                foreach (var item in items)
                {
                    //alteredItems.Add(item);
                    if (!alteredItems.ContainsKey(item.ItemId))
                    {
                        alteredItems.Add(item.ItemId, item);
                    }

                }
            }
            return alteredItems.Values.ToList();
        }


        //TODO delete
        public Person ReadPerson(int id)
        {
            return itemRepository.ReadPerson(id);
        }
        public Organization ReadOrganization(int id)
        {
            return itemRepository.ReadOrganization(id);
        }
        public Theme ReadTheme(int id)
        {
            return itemRepository.ReadTheme(id);
        }
        public IEnumerable<Item> SearchItems(string search)
        {
            return itemRepository.SearchItems(search);
        }

        public Person AddPerson(Person person)
        {
            return (Person)itemRepository.CreateItem(person);
        }

        public Organization AddOrganization(Organization organization)
        {
            return (Organization)itemRepository.CreateItem(organization);
        }

        public void RemoveOrganization(Organization organization)
        {
            itemRepository.DeleteItem(organization);
        }

        public void ChangeItem(Item item)
        {
            itemRepository.UpdateItem(item);
        }



        public void RemoveItem(Item i)
        {
            itemRepository.DeleteItem(i);
        }

        public Item GetItem(int id)
        {
            return itemRepository.ReadItem(id);
        }

        public Item GetItem(string s)
        {
            return itemRepository.ReadItem(s);
        }


    }
}
