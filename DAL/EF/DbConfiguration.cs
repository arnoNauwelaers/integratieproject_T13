using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.EF
{
    internal class SupportCenterDbConfiguration : DbConfiguration
    {
        public SupportCenterDbConfiguration()
        {
            this.SetDefaultConnectionFactory(new System.Data.Entity.Infrastructure.SqlConnectionFactory()); // SQL Server instantie op machine

            this.SetProviderServices("System.Data.SqlClient", System.Data.Entity.SqlServer.SqlProviderServices.Instance);

            this.SetDatabaseInitializer<BarometerDbContext>(new DbInitializer());
        }
    }
}
