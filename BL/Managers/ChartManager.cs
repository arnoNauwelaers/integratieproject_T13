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

        public ChartManager()
        {
            chartRepository = RepositoryFactory.CreateChartRepository();
            itemManager = new ItemManager();
            dataManager = new DataManager();
            socialMediaManager = new SocialMediaManager();
            zoneManager = new ZoneManager();
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
        public Chart GetChart(int id)
        {
            Chart chart = chartRepository.ReadChart(id);
            RetrieveDataChart(chart);
            return chart;
        }

        //TODO kijken of extra nodig is
        public void RetrieveDataChart(Chart chart)
        {
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

                tempData = socialMediaManager.GetDataFromPost(since, chart.ChartValue);
            }
            foreach (var item in chart.Items)
            {
                tempData = socialMediaManager.GetDataFromPost(since, chart.ChartValue, item);
                ChartItemData tempChartItemData = new ChartItemData() { Item = item };
                foreach (var data in tempData)
                {
                    tempChartItemData.Data.Add(new Data() { Name = data.Key, Amount = data.Value });
                }
                chart.ChartItemData.Add(tempChartItemData);
            }
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
                    itemList.Add(itemManager.ReadPerson(Int32.Parse(id)));
                }
                chart.Items = itemList;
            }
            chart.ChartType = (ChartType) Enum.Parse(typeof(ChartType), chartType);
            chart.ChartValue = (ChartValue)Enum.Parse(typeof(ChartValue), chartValue);
            chart.FrequencyType = (DateFrequencyType)Enum.Parse(typeof(DateFrequencyType), dateFrequency);
            chart.Zone = new Zone() { Width = 530, Height = 400, X = 10, Y = 10 };
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
    }
}