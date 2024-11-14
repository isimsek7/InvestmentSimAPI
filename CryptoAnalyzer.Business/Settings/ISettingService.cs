using System;
namespace CryptoAnalyzer.Business.Settings
{
	public interface ISettingService
	{
		Task ToggleMaintenance();

		bool GetMaintenanceState();
	}
}

