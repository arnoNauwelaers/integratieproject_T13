using BL.Domain;
using DAL.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Repositories
{
    public class SettingsRepository
    {
        private readonly BarometerDbContext ctx;

        public SettingsRepository(UnitOfWork uow)
        {
            ctx = uow.Context;
        }

        public Settings CreateSettings(Settings settings)
        {
            ctx.Settings.Add(settings);
            ctx.SaveChanges();
            return settings;
        }

        public void UpdateSettings(Settings settings)
        {
            ctx.Set<Settings>().AddOrUpdate(settings);
            ctx.SaveChanges();
        }

        public List<Settings> ReadSettings()
        {
            return ctx.Settings.ToList();
        }
    }
}
