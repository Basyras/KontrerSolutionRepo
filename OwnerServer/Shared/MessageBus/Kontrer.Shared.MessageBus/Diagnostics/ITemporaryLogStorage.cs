using Basyc.MessageBus.Client.Diagnostics;

namespace Microsoft.Extensions.DependencyInjection
{
	public interface ITemporaryLogStorage
	{
		void AddLog(LogStorageKey key, string message);
		void RemoveLogs(LogStorageKey key);
	}
}