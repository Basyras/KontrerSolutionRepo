using Microsoft.Extensions.DependencyInjection;
using System;

namespace Basyc.DependencyInjection
{
    public abstract class DependencyBuilderBase<TParentBuilder>
    {
        private readonly IServiceCollection services;
        private readonly TParentBuilder parentBuilder;

        public DependencyBuilderBase(IServiceCollection services, TParentBuilder parentBuilder)
        {
            this.services = services;
            this.parentBuilder = parentBuilder;
        }

        /// <summary>
        /// Allows continuing configuring previous builder
        /// </summary>
        /// <returns></returns>
        public TParentBuilder Back()
        {
            return parentBuilder;
        }

        //public TChildBuilder UseChildBuilder<TChildBuilder>(Func<IServiceCollection, TChildBuilder> createBuilder)
        //{
        //    return createBuilder(services);
        //}
    }

    public abstract class DependencyBuilderBase
    {
        private readonly IServiceCollection services;

        public DependencyBuilderBase(IServiceCollection services)
        {
            this.services = services;
        }
    }
}