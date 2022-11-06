﻿using Basyc.DependencyInjection;
using Basyc.MessageBus.Manager.Infrastructure.MessageRegistration.Interface;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Basyc.MessageBus.Manager.Infrastructure.Building.Interface
{
	public class SetupDisplayNameStage : BuilderStageBase
	{
		private readonly InterfaceRegistration interfaceRegistration;

		public SetupDisplayNameStage(IServiceCollection services, InterfaceRegistration interfaceRegistration) : base(services)
		{
			this.interfaceRegistration = interfaceRegistration;
		}

		public SelectMessageTypeStage SetDisplayName(Func<Type, string> formatter)
		{
			interfaceRegistration.DisplayNameFormatter = formatter;
			return new SelectMessageTypeStage(services, interfaceRegistration);
		}
	}
}
