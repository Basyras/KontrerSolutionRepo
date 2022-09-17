using Basyc.MessageBus.Manager.Application.Building.Stages.MessageRegistration;
using Basyc.MessageBus.Manager.Application.Initialization;
using Basyc.MessageBus.Manager.Infrastructure;
using Basyc.MessageBus.Manager.Infrastructure.Building;
using System;
using System.Reflection;

namespace Microsoft.Extensions.DependencyInjection
{
	public static class MessageManagerBuilderInterfacedExtensions
	{
		public static SetupDiagnosticsStage RegisterMessagesAsCQRS(this RegisterMessagesFromAssemblyStage parentStage, Type iQueryType, Type iCommandType, Type iCommandWithResponseType)
		{
			parentStage.services.Configure<InterfacedDomainProviderOptions>(options =>
			{
				options.IQueryType = iQueryType;
				options.ICommandType = iCommandType;
				options.ICommandWithResponseType = iCommandWithResponseType;
				options.AssembliesToScan = parentStage.assembliesToScan;
			});
			parentStage.services.AddSingleton<IDomainInfoProvider, InterfacedDomainProvider>();
			return new SetupDiagnosticsStage(parentStage.services);

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