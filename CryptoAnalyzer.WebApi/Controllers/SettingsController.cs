using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CryptoAnalyzer.Business.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CryptoAnalyzer.WebApi.Controllers
{
    [Route("api/[controller]")]
    public class SettingsController : Controller
    {
        private readonly ISettingService _settingService;

        public SettingsController(ISettingService settingService)
        {
            _settingService = settingService;
        }

        [HttpPatch]
        [Authorize(Roles="Admin")]

        public async Task<IActionResult> ToggleMaintenance()
        {
            await _settingService.ToggleMaintenance();

            return Ok();
        }
    }
}

