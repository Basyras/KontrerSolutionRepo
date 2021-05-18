using Kontrer.OwnerServer.PricingService.Application.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Settings
{
    public record ScopedSettingRequest(SettingRequest SettingRequest, int TimeScopeId);
    public record ScopedSettingRequest<TSettings>(SettingRequest<TSettings> SettingRequest, int TimeScopeId);
}
