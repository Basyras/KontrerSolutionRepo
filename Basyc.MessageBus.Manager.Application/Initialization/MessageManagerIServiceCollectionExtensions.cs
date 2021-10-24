using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager.Application.Initialization
{
    public static class MessageManagerIServiceCollectionExtensions
    {
        public static MessageManagerBuilder AddMessageExplorer(this IServiceCollection services)
        {
            var builder = new MessageManagerBuilder(services);
            return builder;
        }
    }
}