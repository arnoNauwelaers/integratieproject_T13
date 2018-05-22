using BL.Domain;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class ZoneRepository
    {
        private readonly BarometerDbContext ctx;

        public ZoneRepository(UnitOfWork uow)
        {
            this.ctx = uow.Context;
        }

        public List<Zone> ReadZone()
        {
            return ctx.Zones.ToList();
        }

        public Zone CreateZone(Zone zone)
        {
            ctx.Zones.Add(zone);
            ctx.SaveChanges();
            return zone;
        }

        public Zone ReadZone(int id)
        {
            return ctx.Zones.ToList().Find(c => c.Id == id);
        }


        public void UpdateZone(Zone zone)
        {
            ctx.Entry(zone).State = System.Data.Entity.EntityState.Modified;
            ctx.SaveChanges();
        }

        public void DeleteZone(int id)
        {
            Zone zone = ctx.Zones.Find(id);
            ctx.Zones.Remove(zone);
            ctx.SaveChanges();
        }
    }
}
