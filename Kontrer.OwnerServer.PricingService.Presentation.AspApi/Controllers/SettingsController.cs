using Kontrer.OwnerServer.PricingService.Application;
using Kontrer.OwnerServer.PricingService.Application.Settings;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Presentation.AspApi.Controllers
{
    //[Route("api/[controller]")]
    [Route("api/[controller]/[action]")]    
    [ApiController]
    public class SettingsController : Controller
    {
        private readonly PricingManager _pricingManager;

        public SettingsController(PricingManager pricingManager)
        {
            _pricingManager = pricingManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<TimeScope>>> GetTimeScopes()
        {
            List<TimeScope> timeScopes = await _pricingManager.SettingRepository.GetTimeScopesAsync();
            return Ok(timeScopes);
        }

        [HttpPost]
        public async Task<ActionResult> CreateTimeScope(string timeScopeName, DateTime from, DateTime to)
        {
            _pricingManager.SettingRepository.AddTimeScope(timeScopeName, from, to);
            await _pricingManager.SettingRepository.SaveAsync();
            return Ok();
        }


      

        [HttpPost]
        public async Task<ActionResult> CreateSetting<TSetting>(string settingId) //Generics does not work as HTTP endpoint (they are not shown in swagger)
        {
            _pricingManager.SettingRepository.AddSetting<TSetting>(settingId);
            await _pricingManager.SettingRepository.SaveAsync();
            return Ok();
        }


        [HttpGet]
        public async Task<ActionResult<IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, Kontrer.Shared.Models.NullableResult<object>>>>> GetScopedSettings()
        {
            IDictionary<string, IDictionary<Tuple<DateTime, DateTime>, Kontrer.Shared.Models.NullableResult<object>>> scopedSettings = await _pricingManager.SettingRepository.GetAllScopedSettingsAsync();
            return Ok(scopedSettings);
        }

        [HttpPost]
        public async Task<ActionResult> CreateScopedSetting<TSetting>(string settingId,int timeScopeId,TSetting value)
        {
            _pricingManager.SettingRepository.AddScopedSetting<TSetting>(settingId,timeScopeId, value);
            await _pricingManager.SettingRepository.SaveAsync();
            return Ok();
        }
    }
}
