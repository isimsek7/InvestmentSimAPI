using CryptoAnalyzer.Data.Entities;
using CryptoAnalyzer.Data.Repositories;
using CryptoAnalyzer.Data.UnitOfWork;

namespace CryptoAnalyzer.Business.Settings
{
    public class SettingManager:ISettingService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly IRepository<SettingEntity> _settingsRepository;

		public SettingManager(IUnitOfWork unitOfWork,IRepository<SettingEntity> settingsRepository)
		{
			_unitOfWork = unitOfWork;
			_settingsRepository = settingsRepository;
		}

        public bool GetMaintenanceState()
        {
			var maintenanceState = _settingsRepository.GetById(1).MaintenanceMode;

			return maintenanceState;
        }

        async Task ISettingService.ToggleMaintenance()
        {
			var setting = _settingsRepository.GetById(1);

			setting.MaintenanceMode = !setting.MaintenanceMode;

			_settingsRepository.Update(setting);

			try
			{
				await _unitOfWork.SaveChangesAsync();
			}
			catch (Exception)
			{
				throw new Exception("An error occured while toggling maintenance mode");
			}

        }
    }
}

