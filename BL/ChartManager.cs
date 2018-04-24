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

        public ChartManager()
        {
            chartRepository = RepositoryFactory.CreateChartRepository();
        }

        public void UpdateChart(Chart chart)
        {
            chartRepository.UpdateChart(chart);
        }

        public Chart AddChart(Chart chart)
        {
            return chartRepository.CreateChart(chart);
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
    }
}
