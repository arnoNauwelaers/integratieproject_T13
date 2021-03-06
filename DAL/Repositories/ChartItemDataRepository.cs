﻿using BL.Domain;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ChartItemDataRepository
    {
        private readonly BarometerDbContext ctx;

        public ChartItemDataRepository(UnitOfWork uow)
        {
            this.ctx = uow.Context;
        }

        public List<ChartItemData> ReadChartItemData()
        {
            return ctx.ChartItemDatas.ToList();
        }

        public ChartItemData CreateChartItemData(ChartItemData chartItemData)
        {
            ctx.ChartItemDatas.Add(chartItemData);
            ctx.SaveChanges();
            return chartItemData;
        }

        public ChartItemData ReadChartItemData(int id)
        {
            return ctx.ChartItemDatas.ToList().Find(c => c.Id == id);
        }


        public void UpdateChartItemData(ChartItemData chartItemData)
        {
            ctx.Entry(chartItemData).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteChart(int chartItemDataId)
        {
            ChartItemData chartItemData = ctx.ChartItemDatas.Find(chartItemDataId);
            ctx.ChartItemDatas.Remove(chartItemData);
            ctx.SaveChanges();
        }
    }
}
