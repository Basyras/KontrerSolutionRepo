using Basyc.MessageBus.Manager.Application;
using Basyc.MessageBus.Manager.Infrastructure;
using Basyc.MessageBus.Manager.Infrastructure.Formatters;
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
            SetDefaultFormatting();
            services.AddSingleton<IRequestInfoTypeStorage, InMemoryRequestInfoTypeStorage>();
        }

        private void SetDefaultFormatting()
        {
            SetDomainNameFormatter<TypedDomainNameFormatter>();
            SetRequestNameFormatter<TypedRequestNameFormatter>();
            SetParamaterNameFormatter<TypedParameterTypeNameFormatter>();
            SetResponseNameFormatter<TypedResponseNameFormatter>();
            SetResponseFormatter<JsonResponseFormatter>();
        }

        //Removes all formatters
        public TypedFormatterBuilder ResetFormatting()
        {
            services.RemoveAll<ITypedDomainNameFormatter>();
            services.RemoveAll<ITypedRequestNameFormatter>();
            services.RemoveAll<ITypedParameterNameFormatter>();
            services.RemoveAll<ITypedResponseNameFormatter>();
            services.RemoveAll<IResponseFormatter>();
            SetDefaultFormatting();
            return this;
        }

        public TypedFormatterBuilder SetDomainNameFormatter<TDomainNameFormatter>() where TDomainNameFormatter : class, ITypedDomainNameFormatter
        {
            services.RemoveAll<ITypedDomainNameFormatter>();
            services.AddSingleton<ITypedDomainNameFormatter, TDomainNameFormatter>();
            return this;
        }

        public TypedFormatterBuilder SetRequestNameFormatter<TRequestNameFormatter>() where TRequestNameFormatter : class, ITypedRequestNameFormatter
        {
            services.RemoveAll<ITypedRequestNameFormatter>();
            services.AddSingleton<ITypedRequestNameFormatter, TRequestNameFormatter>();
            return this;
        }

        public TypedFormatterBuilder SetParamaterNameFormatter<TParameterTypeNameFormatter>() where TParameterTypeNameFormatter : class, ITypedParameterNameFormatter
        {
            services.RemoveAll<ITypedParameterNameFormatter>();
            services.AddSingleton<ITypedParameterNameFormatter, TParameterTypeNameFormatter>();
            return this;
        }

        public TypedFormatterBuilder SetResponseNameFormatter<TResponseNameFormatter>() where TResponseNameFormatter : class, ITypedResponseNameFormatter
        {
            services.RemoveAll<ITypedResponseNameFormatter>();
            services.AddSingleton<ITypedResponseNameFormatter, TResponseNameFormatter>();
            return this;
        }

        public TypedFormatterBuilder SetResponseFormatter<TResponseFormatter>() where TResponseFormatter : class, IResponseFormatter
        {
            services.RemoveAll<IResponseFormatter>();
            services.AddSingleton<IResponseFormatter, TResponseFormatter>();
            return this;
        }
    }
}