using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Infrastructure;
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
            AddDefaultFormatting();
            services.AddSingleton<IRequestInfoTypeStorage, InMemoryRequestInfoTypeStorage>();
        }

        private void AddDefaultFormatting()
        {
            AddDomainNameFormatter<TypedDomainNameFormatter>();
            AddRequestNameFormatter<TypedRequestNameFormatter>();
            AddParamaterNameFormatter<TypedParameterTypeNameFormatter>();
            AddResponseNameFormatter<TypedResponseNameFormatter>();
        }

        //Removes all formatters
        public TypedFormatterBuilder ResetFormatting()
        {
            services.RemoveAll<ITypedDomainNameFormatter>();
            services.RemoveAll<ITypedRequestNameFormatter>();
            services.RemoveAll<ITypedParameterNameFormatter>();
            services.RemoveAll<ITypedResponseNameFormatter>();
            AddDefaultFormatting();
            return this;
        }

        public TypedFormatterBuilder AddDomainNameFormatter<TDomainNameFormatter>() where TDomainNameFormatter : class, ITypedDomainNameFormatter
        {
            services.RemoveAll<ITypedDomainNameFormatter>();
            services.AddSingleton<ITypedDomainNameFormatter, TDomainNameFormatter>();
            return this;
        }

        public TypedFormatterBuilder AddRequestNameFormatter<TRequestNameFormatter>() where TRequestNameFormatter : class, ITypedRequestNameFormatter
        {
            services.RemoveAll<ITypedRequestNameFormatter>();
            services.AddSingleton<ITypedRequestNameFormatter, TRequestNameFormatter>();
            return this;
        }

        public TypedFormatterBuilder AddParamaterNameFormatter<TParameterTypeNameFormatter>() where TParameterTypeNameFormatter : class, ITypedParameterNameFormatter
        {
            services.RemoveAll<ITypedParameterNameFormatter>();
            services.AddSingleton<ITypedParameterNameFormatter, TParameterTypeNameFormatter>();
            return this;
        }

        public TypedFormatterBuilder AddResponseNameFormatter<TResponseNameFormatter>() where TResponseNameFormatter : class, ITypedResponseNameFormatter
        {
            services.RemoveAll<ITypedResponseNameFormatter>();
            services.AddSingleton<ITypedResponseNameFormatter, TResponseNameFormatter>();
            return this;
        }
    }
}