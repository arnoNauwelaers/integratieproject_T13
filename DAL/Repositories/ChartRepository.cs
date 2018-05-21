using BL.Domain;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Data.Entity;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity.Migrations;
using DAL.EF;

namespace DAL.Repositories
{
    public class ChartRepository
    {
        private readonly BarometerDbContext ctx;

        public ChartRepository(UnitOfWork uow)
        {
            this.ctx = uow.Context;
        }

        public List<Chart> ReadCharts()
        {
            return ctx.Charts.Include(i => i.Items).Include(i => i.SavedChartItemData).ToList();
        }

        public Chart CreateChart(Chart chart)
        {
            ctx.Charts.Add(chart);
            ctx.SaveChanges();
            return chart;
        }

        public List<Chart> ReadStandardCharts()
        {
            if (ctx.Charts.Any(c => c.StandardChart == true))
            {
                return ctx.Charts.Where(c => c.StandardChart == true).ToList();
            }
            else
            {
               return new List<Chart>();
            }
        }

        public Chart ReadChart(int id)
        {
            return ctx.Charts.Include(i => i.Items).Include(i => i.SavedChartItemData).ToList().Find(c => c.ChartId == id);
        }


        public void UpdateChart(Chart chart)
        {
            ctx.Entry(chart).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteChart(int chartId)
        {
            Chart chart = ctx.Charts.Find(chartId);
            ctx.Charts.Remove(chart);
            ctx.SaveChanges();
        }
    }
}
