using BL.Domain;
using DAL.EF;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Managers
{
    public class ChartManager
    {
        private ChartRepository chartRepository;
        private DataManager dataManager;
        private ItemManager itemManager;
        private SocialMediaManager socialMediaManager;
        private ZoneManager zoneManager;
        private static Dictionary<string, Chart> standardCharts = new Dictionary<string, Chart>();
        private const int AMOUNT_OF_ELEMENTS = 20;
        private const int AMOUNT_OF_CHARTS_ITEM = 2;

        public ChartManager(UnitOfWorkManager unitOfWorkManager)
        {
            chartRepository = new ChartRepository(unitOfWorkManager.UnitOfWork);
            itemManager = new ItemManager(unitOfWorkManager);
            dataManager = new DataManager(unitOfWorkManager);
            socialMediaManager = new SocialMediaManager(unitOfWorkManager);
            zoneManager = new ZoneManager(unitOfWorkManager);
            CreateStandardChartsIfNotExists();
        }

        public List<Chart> GetChartsFromItem(Item item)
        {
            List<Chart> charts = new List<Chart>();
            if (item.StandardCharts.Count == 0)
            {
                List<Item> items = new List<Item>
                {
                    item
                };
                Chart itemWordsWeekly = AddChart(new Chart() { Items = items, ChartType = ChartType.bar, ChartValue = ChartValue.words, FrequencyType = DateFrequencyType.weekly });
                Chart itemHashtagsWeekly = AddChart(new Chart() { Items = items, ChartType = ChartType.pie, ChartValue = ChartValue.hashtags, FrequencyType = DateFrequencyType.weekly });
                Chart itemPostsMonthly = AddChart(new Chart() { Items = items, ChartType = ChartType.line, ChartValue = ChartValue.postsPerDate, FrequencyType = DateFrequencyType.monthly });
                item.StandardCharts.Add(itemWordsWeekly);
                item.StandardCharts.Add(itemHashtagsWeekly);
                item.StandardCharts.Add(itemPostsMonthly);
                itemManager.ChangeItem(item);
            }
            charts = item.StandardCharts.Take(AMOUNT_OF_CHARTS_ITEM).ToList();
            foreach (var chart in charts)
            {
                RetrieveDataChart(chart);
            }
            return charts;
        }

        public Dictionary<string, Chart> GetStandardChart()
        {
            return standardCharts;
        }

        public void CreateStandardChartsIfNotExists()
        {
            int amountStandardCharts = 12;
            if (standardCharts.Count < amountStandardCharts && chartRepository.ReadStandardCharts().Count < amountStandardCharts)
            {

                standardCharts = new Dictionary<string, Chart>();
                Chart trendingPersonWeek = new Chart() { StandardChart = true, ChartType = ChartType.bar, ChartValue = ChartValue.trendPersons, FrequencyType = DateFrequencyType.weekly };
                Chart trendingPersonMonth = new Chart() { StandardChart = true, ChartType = ChartType.bar, ChartValue = ChartValue.trendPersons, FrequencyType = DateFrequencyType.monthly };
                Chart trendingOrganizationWeek = new Chart() { StandardChart = true, ChartType = ChartType.bar, ChartValue = ChartValue.trendOrganizations, FrequencyType = DateFrequencyType.weekly };
                Chart trendingOrganizationMonth = new Chart() { StandardChart = true, ChartType = ChartType.bar, ChartValue = ChartValue.trendOrganizations, FrequencyType = DateFrequencyType.monthly };
                Chart trendingThemeWeek = new Chart() { StandardChart = true, ChartType = ChartType.bar, ChartValue = ChartValue.trendThemes, FrequencyType = DateFrequencyType.weekly };
                Chart trendingThemeMonth = new Chart() { StandardChart = true, ChartType = ChartType.bar, ChartValue = ChartValue.trendThemes, FrequencyType = DateFrequencyType.monthly };
                Chart mostPositivePersons = new Chart() { StandardChart = true, ChartType = ChartType.bar, ChartValue = ChartValue.trendMostPositive, FrequencyType = DateFrequencyType.weekly, ItemType="Person" };
                Chart mostNegativePersons = new Chart() { StandardChart = true, ChartType = ChartType.bar, ChartValue = ChartValue.trendMostNegative, FrequencyType = DateFrequencyType.weekly, ItemType = "Person" };
                Chart mostPositiveOrganizations = new Chart() { StandardChart = true, ChartType = ChartType.bar, ChartValue = ChartValue.trendMostPositive, FrequencyType = DateFrequencyType.weekly, ItemType = "Organization" };
                Chart mostNegativeOrganizations = new Chart() { StandardChart = true, ChartType = ChartType.bar, ChartValue = ChartValue.trendMostNegative, FrequencyType = DateFrequencyType.weekly, ItemType = "Organization" };
                Chart mostPositiveThemes = new Chart() { StandardChart = true, ChartType = ChartType.bar, ChartValue = ChartValue.trendMostPositive, FrequencyType = DateFrequencyType.weekly, ItemType = "Theme" };
                Chart mostNegativeThemes = new Chart() { StandardChart = true, ChartType = ChartType.bar, ChartValue = ChartValue.trendMostNegative, FrequencyType = DateFrequencyType.weekly, ItemType = "Theme" };


                standardCharts.Add("trendingPersonWeek", chartRepository.CreateChart(trendingPersonWeek));
                standardCharts.Add("trendingPersonMonth", chartRepository.CreateChart(trendingPersonMonth));
                standardCharts.Add("trendingOrganizationWeek", chartRepository.CreateChart(trendingOrganizationWeek));
                standardCharts.Add("trendingOrganizationMonth", chartRepository.CreateChart(trendingOrganizationMonth));
                standardCharts.Add("trendingThemeWeek", chartRepository.CreateChart(trendingThemeWeek));
                standardCharts.Add("trendingThemeMonth", chartRepository.CreateChart(trendingThemeMonth));
                standardCharts.Add("mostPositivePersons", chartRepository.CreateChart(mostPositivePersons));
                standardCharts.Add("mostNegativePersons", chartRepository.CreateChart(mostNegativePersons));
                standardCharts.Add("mostPositiveOrganizations", chartRepository.CreateChart(mostPositiveOrganizations));
                standardCharts.Add("mostNegativeOrganizations", chartRepository.CreateChart(mostNegativeOrganizations));
                standardCharts.Add("mostPositiveThemes", chartRepository.CreateChart(mostPositiveThemes));
                standardCharts.Add("mostNegativeThemes", chartRepository.CreateChart(mostNegativeThemes));
            }
            else if (standardCharts.Count < amountStandardCharts)
            {
                standardCharts = new Dictionary<string, Chart>();
                List<Chart> tempStandardCharts = chartRepository.ReadStandardCharts();
                standardCharts.Add("trendingPersonWeek", tempStandardCharts.First(c => c.ChartValue == ChartValue.trendPersons && c.FrequencyType == DateFrequencyType.weekly));
                standardCharts.Add("trendingPersonMonth", tempStandardCharts.First(c => c.ChartValue == ChartValue.trendPersons && c.FrequencyType == DateFrequencyType.monthly));
                standardCharts.Add("trendingOrganizationWeek", tempStandardCharts.First(c => c.ChartValue == ChartValue.trendOrganizations && c.FrequencyType == DateFrequencyType.weekly));
                standardCharts.Add("trendingOrganizationMonth", tempStandardCharts.First(c => c.ChartValue == ChartValue.trendOrganizations && c.FrequencyType == DateFrequencyType.monthly));
                standardCharts.Add("trendingThemeWeek", tempStandardCharts.First(c => c.ChartValue == ChartValue.trendThemes && c.FrequencyType == DateFrequencyType.weekly));
                standardCharts.Add("trendingThemeMonth", tempStandardCharts.First(c => c.ChartValue == ChartValue.trendThemes && c.FrequencyType == DateFrequencyType.monthly));
                standardCharts.Add("mostPositivePersons", tempStandardCharts.First(c => c.ChartValue == ChartValue.trendMostPositive && c.FrequencyType == DateFrequencyType.weekly && c.ItemType == "Person"));
                standardCharts.Add("mostNegativePersons", tempStandardCharts.First(c => c.ChartValue == ChartValue.trendMostNegative && c.FrequencyType == DateFrequencyType.weekly && c.ItemType == "Person"));
                standardCharts.Add("mostPositiveOrganizations", tempStandardCharts.First(c => c.ChartValue == ChartValue.trendMostPositive && c.FrequencyType == DateFrequencyType.weekly && c.ItemType == "Organization"));
                standardCharts.Add("mostNegativeOrganizations", tempStandardCharts.First(c => c.ChartValue == ChartValue.trendMostNegative && c.FrequencyType == DateFrequencyType.weekly && c.ItemType == "Organization"));
                standardCharts.Add("mostPositiveThemes", tempStandardCharts.First(c => c.ChartValue == ChartValue.trendMostPositive && c.FrequencyType == DateFrequencyType.weekly && c.ItemType == "Theme"));
                standardCharts.Add("mostNegativeThemes", tempStandardCharts.First(c => c.ChartValue == ChartValue.trendMostNegative && c.FrequencyType == DateFrequencyType.weekly && c.ItemType == "Theme"));
            }
        }

        public void UpdateChart(Chart chart)
        {
            chartRepository.UpdateChart(chart);
        }

        public Chart AddChart(Chart chart)
        {
            return chartRepository.CreateChart(chart);
        }


        //Non saved chart -> data realtime ophalen
        public Chart GetChart(int id, Boolean retrieveData)
        {
            Chart chart = chartRepository.ReadChart(id);
            if (retrieveData == true)
            {
                RetrieveDataChart(chart);
            }
            return chart;
        }

        public void RetrieveDataChart(Chart chart)
        {
            if (chart.Saved == true)
            {
                return;
            }
            if (chart.LastRead > Read.LastRead && Read.LastRead != null && chart.ChartItemData.Count > 0)
            {
                return;
            }
            chart.ChartItemData.Clear();
            Boolean trend = false;
            Dictionary<Item, int> itemData = new Dictionary<Item, int>();
            DateTime since = DateTime.Now.AddDays(-7);
            switch (chart.FrequencyType)
            {
                case DateFrequencyType.hourly: since = DateTime.Now.AddMinutes(-60); break;
                case DateFrequencyType.daily: since = DateTime.Now.AddDays(-1); break;
                case DateFrequencyType.weekly: since = DateTime.Now.AddDays(-7); break;
                case DateFrequencyType.monthly: since = DateTime.Now.AddMonths(-1); break;
                case DateFrequencyType.yearly: since = DateTime.Now.AddYears(-1); break;
            }
            Dictionary<string, int> tempData;
            chart.ChartItemData = new List<ChartItemData>();
            if (chart.Items.Count == 0)
            {
                trend = true;
                itemData = socialMediaManager.GetItemsFromChartWithoutItems(since, chart.ChartValue, chart.ItemType);
            }
            if (trend == false && chart.ChartValue != ChartValue.postsPerDate)
            {
                Dictionary<Item, Dictionary<string, int>> listItems = new Dictionary<Item, Dictionary<string, int>>();
                foreach (var item in chart.Items)
                {
                    tempData = socialMediaManager.GetDataFromPost(since, chart.ChartValue, item);
                    if (chart.Items.Count == 1)
                    {
                        tempData = socialMediaManager.GetDataFromPost(since, chart.ChartValue, item).OrderByDescending(i => i.Value).Take(AMOUNT_OF_ELEMENTS).ToDictionary(pair => pair.Key, pair => pair.Value).Shuffle();
                        ChartItemData tempChartItemData = new ChartItemData() { Item = item };
                        foreach (var data in tempData)
                        {
                            tempChartItemData.Data.Add(new Data() { Name = data.Key, Amount = data.Value });
                        }
                        chart.ChartItemData.Add(tempChartItemData);
                    }
                    else
                    {
                        listItems.Add(item, tempData);
                    }
                }
                if (chart.Items.Count > 1)
                {
                    //gemeenschappelijke data uit items halen
                    Dictionary<Item, Dictionary<string, int>> tempListItems = new Dictionary<Item, Dictionary<string, int>>();
                    foreach (var item in listItems) //alle items loopen
                    {
                        Dictionary<string, int> itemDictionary = new Dictionary<string, int>();
                        foreach (var valueItem in item.Value) //alle values van die items loopen (string, int)
                        {
                            foreach (var otherItem in listItems) //alle items nog eens loopen
                            {
                                if (otherItem.Key == item.Key) //niet dezelfde items met mekaar vergelijken
                                {
                                    continue;
                                }
                                foreach (var otherValueItem in otherItem.Value) //alle values van laatste loop loopen (string, int)
                                {
                                    if (valueItem.Key == otherValueItem.Key)
                                    {
                                        if (!itemDictionary.Any(i => i.Key == valueItem.Key))
                                        {
                                            itemDictionary.Add(valueItem.Key, valueItem.Value);
                                        }
                                    }
                                }
                            }
                        }
                        tempListItems.Add(item.Key, itemDictionary);
                    }
                    //nu data toevoegen aan chart
                    foreach (var item in tempListItems)
                    {
                        ChartItemData tempChartItemData = new ChartItemData() { Item = item.Key };
                        item.Value.OrderByDescending(i => i.Value).Take(AMOUNT_OF_ELEMENTS).ToDictionary(pair => pair.Key, pair => pair.Value).Shuffle();
                        foreach (var data in item.Value)
                        {
                            tempChartItemData.Data.Add(new Data() { Name = data.Key, Amount = data.Value });
                        }
                        chart.ChartItemData.Add(tempChartItemData);
                    }
                }
            }
            else if (trend == false && chart.ChartValue == ChartValue.postsPerDate)
            {
                foreach (var item in chart.Items)
                {
                    ChartItemData tempChartItemData = new ChartItemData() { Item = item };
                    foreach (var result in socialMediaManager.GetAmountPostsPerItem(since, item).OrderBy(p => p.Key))
                    {
                        tempChartItemData.Data.Add(new Data() { Name = result.Key, Amount = result.Value });
                    }
                    chart.ChartItemData.Add(tempChartItemData);
                }

            }
            else if (trend == true)
            {
                ChartItemData tempChartItemData = new ChartItemData();
                if (chart.ChartValue == ChartValue.trendMostNegative)
                {
                    itemData = itemData.OrderBy(i => i.Value).Take(AMOUNT_OF_ELEMENTS).ToDictionary(pair => pair.Key, pair => pair.Value).Shuffle();
                }
                else
                {
                    itemData = itemData.OrderByDescending(i => i.Value).Take(AMOUNT_OF_ELEMENTS).ToDictionary(pair => pair.Key, pair => pair.Value).Shuffle();
                }
                foreach (var item in itemData)
                {
                    tempChartItemData.Item = item.Key;
                    tempChartItemData.Data.Add(new Data() { Name = item.Key.Name, Amount = item.Value });
                }
                chart.ChartItemData.Add(tempChartItemData);
            }
            chartRepository.UpdateChart(chart);
            chart.LastRead = DateTime.Now;
        }

        public void UpdateChartsFromTempChart(List<TempChartEdit> chartList)
        {
            foreach (var tempChart in chartList)
            {
                Chart chart = chartRepository.ReadChart(tempChart.Id);
                chart.Zone.X = tempChart.X;
                chart.Zone.Y = tempChart.Y;
                chart.Zone.Width = tempChart.Width;
                chartRepository.UpdateChart(chart);
            }
        }

        public void SaveChart(int id)
        {
            Chart chart = chartRepository.ReadChart(id);
            chart.Saved = true;
            chartRepository.UpdateChart(chart);
        }

        public Chart CreateChartFromDashboard(string items, string chartType, string chartValue, string dateFrequency)
        {
            List<Item> itemList = new List<Item>();
            Chart chart = new Chart();
            char[] whitespace = new char[] { ' ', '\t' };
            string[] itemIds = items.Split(whitespace);
            if (items != "")
            {
                foreach (var id in itemIds)
                {
                    itemList.Add(itemManager.ReadItem(Int32.Parse(id)));
                }
                chart.Items = itemList;
            }
            chart.ChartType = (ChartType)Enum.Parse(typeof(ChartType), chartType);
            chart.ChartValue = (ChartValue)Enum.Parse(typeof(ChartValue), chartValue);
            chart.FrequencyType = (DateFrequencyType)Enum.Parse(typeof(DateFrequencyType), dateFrequency);
            chart.Zone = new Zone() { Width = 2.43, X = 10, Y = 10 };
            Chart finalChart = AddChart(chart);
            return finalChart;
        }

        public void EditCharts(List<TempChartEdit> charts)
        {
            foreach (var chart in charts)
            {
                Chart chartObj = chartRepository.ReadChart(chart.Id);
                chartObj.Zone.X = chart.X;
                chartObj.Zone.Y = chart.Y;
                chartObj.Zone.Width = chart.Width;
                zoneManager.UpdateZone(chartObj.Zone);
                chartRepository.UpdateChart(chartObj);
            }
        }

        public void EditChartFromDashboard(int id, string items, string type, string frequency)
        {
            Chart chart = chartRepository.ReadChart(id);
            chart.LastRead = null;
            List<Item> itemList = new List<Item>();
            char[] whitespace = new char[] { ' ', '\t' };
            string[] itemIds = items.Split(whitespace);
            if (items != "")
            {
                foreach (var itemid in itemIds)
                {
                    chart.Items.Add(itemManager.ReadItem(Int32.Parse(itemid)));
                }
            }
            chart.ChartType = (ChartType)Enum.Parse(typeof(ChartType), type);
            chart.FrequencyType = (DateFrequencyType)Enum.Parse(typeof(DateFrequencyType), frequency);
            chartRepository.UpdateChart(chart);
        }
    }
}