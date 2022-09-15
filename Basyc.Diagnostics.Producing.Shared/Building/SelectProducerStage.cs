using Basyc.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;

namespace Basyc.Diagnostics.Producing.Shared.Building
{
	public class SelectProducerStage : BuilderStageBase
	{
		public SelectProducerStage(IServiceCollection services) : base(services)
		{
		}
	}
}
