using Kontrer.OwnerServer.Shared.Data.Abstraction.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.PricingService.Application.Settings
{
    public interface ISettingsUnitOfWork : IUnitOfWork
    {
        ISettingsRepository PricingSettingsRepository { get; }
    }
}
