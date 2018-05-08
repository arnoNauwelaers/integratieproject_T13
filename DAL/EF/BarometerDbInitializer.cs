using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BL.Domain;
using DAL.EF;
using DAL;
using System.Diagnostics;

namespace DAL.EF
{
    internal class BarometerDbInitializer : MigrateDatabaseToLatestVersion<BarometerDbContext,Migrations.Configuration>
    {

        
    }
}
