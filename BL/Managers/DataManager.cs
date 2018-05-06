using BL.Domain;
using DAL.EF;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Managers
{
    public class DataManager
    {
        private DataRepository DataRepository;

        public DataManager()
        {
            DataRepository = RepositoryFactory.CreateDataRepository();
        }

        public void UpdateData(Data data)
        {
            DataRepository.UpdateData(data);
        }

        public Data AddData(Data data)
        {
            return DataRepository.CreateData(data);
        }
    }
}
