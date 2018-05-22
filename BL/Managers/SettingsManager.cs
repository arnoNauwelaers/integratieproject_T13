using BL.Domain;
using DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL.Managers
{
    public class SettingsManager
    {
        private SettingsRepository settingsRepository;

        public SettingsManager(UnitOfWorkManager unitOfWorkManager)
        {
            settingsRepository = new SettingsRepository(unitOfWorkManager.UnitOfWork);
            CreateSettingsIfNotExists();
        }

        public void CreateSettingsIfNotExists()
        {
            List<Settings> settings = settingsRepository.ReadSettings();
            if (settings.Count == 0)
            {
                Settings setting = new Settings() { ApiFrequency = 10, ApiPort = "0", ApiUrl = "", DataLifetime = 30 };
                AddSettings(setting);
            }
        }

        public Settings GetSettings()
        {
            return settingsRepository.ReadSettings().First();
        }

        public Settings AddSettings(Settings settings)
        {
            return settingsRepository.CreateSettings(settings);
        }

        public void ChangeSettings(Settings settings)
        {
            settingsRepository.UpdateSettings(settings);
        }
    }
}
