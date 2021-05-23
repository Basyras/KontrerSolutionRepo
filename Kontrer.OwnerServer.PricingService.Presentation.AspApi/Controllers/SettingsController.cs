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
            _pricingManager.SettingRepository.CreateNewTimeScope(timeScopeName, from, to);
            await _pricingManager.SettingRepository.SaveAsync();
            return Ok();
        }


        /// <summary>
        /// Key is setting name, value is <see cref="Type.AssemblyQualifiedName"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet]        
        public async Task<ActionResult<Dictionary<string,string>>> GetSettings()
        {                  
            var settings = await _pricingManager.SettingRepository.GetSettingsAsync();            
            return Ok(settings.ToDictionary(x=>x.Key,x=>x.Value.AssemblyQualifiedName));
        }

        [HttpPost]
        public async Task<ActionResult> CreateSetting(string settingId, string settingTypeName)
        {
            var settingType = Type.GetType(settingTypeName);
            if (settingType == null) throw new Exception("Unknown type");
            _pricingManager.SettingRepository.CreateNewSetting(settingId, settingType);
            await _pricingManager.SettingRepository.SaveAsync();
            return Ok();
        }


        [HttpGet]
        public async Task<ActionResult<Dictionary<string, Dictionary<Tuple<DateTime, DateTime>, Kontrer.Shared.Models.NullableResult<object>>>>> GetScopedSettings()
        {
            Dictionary<string, Dictionary<Tuple<DateTime, DateTime>, Kontrer.Shared.Models.NullableResult<object>>> scopedSettings = (Dictionary<string, Dictionary<Tuple<DateTime, DateTime>, Kontrer.Shared.Models.NullableResult<object>>>) (await _pricingManager.SettingRepository.GetAllScopedSettingsAsync());
            return Ok(scopedSettings);
        }

        [HttpPost]
        public async Task<ActionResult> CreateScopedSetting(string settingId,int timeScopeId,object value,string settingTypeName)
        {
            _pricingManager.SettingRepository.AddScopedSetting(settingId,timeScopeId, value, Type.GetType(settingTypeName));
            await _pricingManager.SettingRepository.SaveAsync();
            return Ok();
        }
    }
}
