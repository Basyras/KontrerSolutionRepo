using Basyc.MessageBus.Client.Diagnostics;
using System.Collections.Generic;

namespace Microsoft.Extensions.DependencyInjection
{
	public class InMemoryTemporaryLogStorage : ITemporaryLogStorage
	{
		private readonly Dictionary<LogStorageKey, string> messageMap = new();
		public void AddLog(LogStorageKey key, string message)
		{
			messageMap.Add(key, message);
		}

		public void RemoveLogs(LogStorageKey key)
		{
			messageMap.Remove(key);

		}
	}
}