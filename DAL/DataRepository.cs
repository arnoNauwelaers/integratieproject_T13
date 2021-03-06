﻿using BL.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    public class DataRepository
    {
        private BarometerDbContext ctx;

        public DataRepository(BarometerDbContext ctx)
        {
            this.ctx = ctx;
        }

        public List<Data> ReadData()
        {
            return ctx.Data.ToList();
        }

        public Data CreateData(Data data)
        {
            ctx.Data.Add(data);
            ctx.SaveChanges();
            return data;
        }

        public Data ReadData(int id)
        {
            return ctx.Data.ToList().Find(c => c.Id == id);
        }


        public void UpdateData(Data data)
        {
            ctx.Entry(data).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteData(int id)
        {
            Data data = ctx.Data.Find(id);
            ctx.Data.Remove(data);
            ctx.SaveChanges();
        }
    }
}
