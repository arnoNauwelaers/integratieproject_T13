using BL.Domain;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class ChartManager
    {
        private ChartRepository chartRepository;
        private DataManager dataManager;
        private ItemManager itemManager;
        private SocialMediaManager socialMediaManager;

        public ChartManager()
        {
            chartRepository = RepositoryFactory.CreateChartRepository();
            itemManager = new ItemManager();
            dataManager = new DataManager();
            socialMediaManager = new SocialMediaManager();
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
                chart.Zone.Height = tempChart.Height;
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
            foreach (var id in itemIds)
            {
                itemList.Add(itemManager.ReadPerson(Int32.Parse(id)));
            }
            chart.Items = itemList;
            chart.ChartType = (ChartType) Enum.Parse(typeof(ChartType), chartType);
            chart.ChartValue = (ChartValue)Enum.Parse(typeof(ChartValue), chartValue);
            chart.FrequencyType = (DateFrequencyType)Enum.Parse(typeof(DateFrequencyType), dateFrequency);
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
                chartObj.Zone.Height = chart.Height;
                chartObj.Zone.Width = chart.Width;
                chartRepository.UpdateChart(chartObj);
            }
        }
    }
}