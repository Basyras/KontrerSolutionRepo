﻿using Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration;
using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Infrastructure;
using Basyc.MessageBus.Manager.Infrastructure.Building;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class MessageManagerBuilderInterfacedExtensions
	{
		//public static TypedFormatterBuilder RegisterCQRSMessages(this SelectMessageRegistrationMethodStage managerBuilder, Type iQueryType, Type iCommandType, Type iCommandWithResponseType, params Assembly[] assembliesToScan)
		//{
		//	managerBuilder.services.Configure<InterfacedDomainProviderOptions>(options =>
		//	{
		//		options.IQueryType = iQueryType;
		//		options.ICommandType = iCommandType;
		//		options.ICommandWithResponseType = iCommandWithResponseType;
		//		options.AssembliesToScan = assembliesToScan;
		//	});
		//	managerBuilder.AddProvider<InterfacedDomainProvider>();
		//	return new TypedFormatterBuilder(managerBuilder.services);
		//}

		//public static TypedFormatterBuilder AddInterfacedProvider(this SelectMessageRegistrationMethodStage managerBuilder, Type iMessageType, Type iMessageWithResponseType, params Assembly[] assemblies)
		//{
		//	managerBuilder.services.Configure<InterfacedDomainProviderOptions>(options =>
		//	{
		//		options.IMessageType = iMessageType;
		//		options.IMessageWithResponseType = iMessageWithResponseType;
		//		options.AssembliesToScan = assemblies;
		//	});
		//	managerBuilder.AddProvider<InterfacedDomainProvider>();
		//	return new TypedFormatterBuilder(managerBuilder.services);
		//}


		public static SetupRequesterStage RegisterMessagesAsCQRS(this RegisterMessagesFromAssemblyStage fromAssemblyStage, Type iQueryType, Type iCommandType, Type iCommandWithResponseType)
		{
			fromAssemblyStage.services.Configure<InterfacedDomainProviderOptions>(options =>
			{
				options.IQueryType = iQueryType;
				options.ICommandType = iCommandType;
				options.ICommandWithResponseType = iCommandWithResponseType;
				options.AssembliesToScan = fromAssemblyStage.assembliesToScan;
			});
			fromAssemblyStage.services.AddSingleton<IDomainInfoProvider, InterfacedDomainProvider>();


			return new SetupRequesterStage(fromAssemblyStage.services);

		}

		public static SetupTypeFormattingStage RegisterMessages(this RegisterMessagesFromAssemblyStage fromAssemblyStage, Type iMessageType, Type iMessageWithResponseType, params Assembly[] assemblies)
		{
			fromAssemblyStage.services.Configure<InterfacedDomainProviderOptions>(options =>
			{
				options.IMessageType = iMessageType;
				options.IMessageWithResponseType = iMessageWithResponseType;
				options.AssembliesToScan = assemblies;
			});
			fromAssemblyStage.services.AddSingleton<IDomainInfoProvider, InterfacedDomainProvider>();

			return new SetupTypeFormattingStage(fromAssemblyStage.services);
		}
	}
}