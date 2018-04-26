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
            DateTime since = DateTime.Now.AddDays(-7);
            switch (chart.FrequencyType)
            {
                case DateFrequencyType.hourly: since = DateTime.Now.AddMinutes(-60); break;
                case DateFrequencyType.daily: since = DateTime.Now.AddDays(-1); break;
                case DateFrequencyType.weekly: since = DateTime.Now.AddDays(-7); break;
                case DateFrequencyType.monthly: since = DateTime.Now.AddMonths(-1); break;
                case DateFrequencyType.yearly: since = DateTime.Now.AddYears(-1); break;
            }
            //TODO data toevoegen aan chart
            Dictionary<string, int> tempData = socialMediaManager.GetDataFromPost(since, chart.ChartValue, (List<Item>) chart.Items);
            foreach (var item in tempData)
            {
                chart.Data.Add(new Data() { Name = item.Key, Amount = item.Value });
            }
            return chartRepository.CreateChart(chart);
        }

        public Chart GetChart(int id)
        {
            return chartRepository.ReadChart(id);
        }

        public void UpdateChartsFromTempChart(List<TempChart> chartList)
        {
            foreach (var tempChart in chartList)
            {
                Chart chart = chartRepository.ReadChart(tempChart.Id);
                chart.X = tempChart.X;
                chart.Y = tempChart.Y;
                chart.Width = tempChart.Width;
                chart.Height = tempChart.Height;
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
    }
}