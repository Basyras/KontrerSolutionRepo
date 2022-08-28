using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.DependencyInjection
{
	public class BusHandlerLoggerProvider : ILoggerProvider
	{
		public ILogger CreateLogger(string categoryName)
		{
			throw new System.NotImplementedException();
		}

		public void Dispose()
		{
			throw new System.NotImplementedException();
		}
	}
}