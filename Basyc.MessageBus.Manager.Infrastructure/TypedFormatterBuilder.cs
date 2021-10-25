using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Infrastructure;
using Basyc.MessageBus.Manager.Infrastructure.Basyc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Manager
{
    public class TypedFormatterBuilder
    {
        public readonly IServiceCollection services;

        public TypedFormatterBuilder(IServiceCollection services)
        {
            this.services = services;
            UseDomainNameFormatter<TypedDomainNameFormatter>();
            UseRequestNameFormatter<TypedRequestNameFormatter>();
            UseParamaterNameFormatter<TypedParameterTypeNameFormatter>();
            UseResponseNameFormatter<TypedResponseNameFormatter>();
            services.AddSingleton<IRequestInfoTypeStorage, InMemoryRequestInfoTypeStorage>();
        }

        public TypedFormatterBuilder UseDomainNameFormatter<TDomainNameFormatter>() where TDomainNameFormatter : class, ITypedDomainNameFormatter
        {
            services.RemoveAll<ITypedDomainNameFormatter>();
            services.AddSingleton<ITypedDomainNameFormatter, TDomainNameFormatter>();
            return this;
        }

        public TypedFormatterBuilder UseRequestNameFormatter<TRequestNameFormatter>() where TRequestNameFormatter : class, ITypedRequestNameFormatter
        {
            services.RemoveAll<ITypedRequestNameFormatter>();
            services.AddSingleton<ITypedRequestNameFormatter, TRequestNameFormatter>();
            return this;
        }

        public TypedFormatterBuilder UseParamaterNameFormatter<TParameterTypeNameFormatter>() where TParameterTypeNameFormatter : class, ITypedParameterNameFormatter
        {
            services.RemoveAll<ITypedParameterNameFormatter>();
            services.AddSingleton<ITypedParameterNameFormatter, TParameterTypeNameFormatter>();
            return this;
        }

        public TypedFormatterBuilder UseResponseNameFormatter<TResponseNameFormatter>() where TResponseNameFormatter : class, ITypedResponseNameFormatter
        {
            services.RemoveAll<ITypedResponseNameFormatter>();
            services.AddSingleton<ITypedResponseNameFormatter, TResponseNameFormatter>();
            return this;
        }
    }
}