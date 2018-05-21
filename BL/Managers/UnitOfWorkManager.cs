using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Managers
{
    public class UnitOfWorkManager
    {
        public readonly UnitOfWork UnitOfWork = new UnitOfWork();
    }
}
