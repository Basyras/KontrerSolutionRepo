using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kontrer.OwnerServer.Shared.Asp
{
    public static class AspIServiceCollectionExtensions
    {
        public static IMvcBuilder FixSerialization(this IMvcBuilder mvcBuilder)
        {
            //mvcBuilder.AddJsonOptions()
            mvcBuilder.AddNewtonsoftJson();

            //var jsonSeri  =services.GetRequiredService<>
            //JsonConvert.DefaultSettings = 
            return mvcBuilder;
        }
    }
}
