using Basyc.MessageBus.Manager.Infrastructure.Building;
using System.Collections.Generic;

namespace Basyc.MessageBus.Manager.Infrastructure.Basyc.Basyc.MessageBus
{
	public class BusManagerBasycDiagnosticsReceiverSessionMapper : IBasycDiagnosticsReceiverSessionMapper
	{
		private readonly Dictionary<int, int> foreinfIdToSessionIdMap = new();
		public int GetSessionId(int sessionId)
		{
			return foreinfIdToSessionIdMap[sessionId];
		}

		public void AddMapping(int sessionId, int foreingId)
		{
			foreinfIdToSessionIdMap.Add(foreingId, sessionId);
		}
	}
}
