﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Kontrer.Shared.Localizator
{
    public class LocalizationDependencyBuilder
    {
        IServiceCollection _servics;
        public LocalizationDependencyBuilder(IServiceCollection services)
        {
            _servics = services;
        }

        public LocalizationDependencyBuilder AddStorage<TLocalizatorStorage>() where TLocalizatorStorage : class, ILocalizatorStorage
        {
            _servics.AddSingleton<ILocalizatorStorage, TLocalizatorStorage>();
            return this;
        }




    }
}