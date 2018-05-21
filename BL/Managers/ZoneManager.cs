using BL.Domain;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Managers
{
    public class ZoneManager
    {
        private ZoneRepository ZoneRepository;

        public ZoneManager(UnitOfWorkManager unitOfWorkManager)
        {
            ZoneRepository = new ZoneRepository(unitOfWorkManager.UnitOfWork);
        }

        public void UpdateZone(Zone zone)
        {
            ZoneRepository.UpdateZone(zone);
        }

        public Zone AddZone(Zone zone)
        {
            return ZoneRepository.CreateZone(zone);
        }
    }
}
