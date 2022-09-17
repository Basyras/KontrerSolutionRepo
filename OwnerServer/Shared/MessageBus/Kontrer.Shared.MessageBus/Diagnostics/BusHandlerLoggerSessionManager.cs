using System;
using System.Threading;

namespace Basyc.MessageBus.Client.Diagnostics
{
	public static class BusHandlerLoggerSessionManager
	{
		private static AsyncLocal<int> SessionId { get; } = new AsyncLocal<int>();
		private static AsyncLocal<bool> HasSesion { get; } = new AsyncLocal<bool>() { Value = false };

		public static void StartSession(int sessionId)
		{
			if (HasSesion.Value is true)
			{
				throw new InvalidOperationException($"Cant call {nameof(StartSession)} twice on same async context");
			}

			SessionId.Value = sessionId;
			HasSesion.Value = true;
		}

		public static void EndSession()
		{
			if (HasSesion.Value is false)
			{
				throw new InvalidOperationException($"Cant call {nameof(EndSession)} without calling {nameof(StartSession)} before");
			}

			SessionId.Value = 0;
			HasSesion.Value = false;
		}

		public static bool HasSession(out int sessionId)
		{
			sessionId = SessionId.Value;
			return HasSesion.Value;
		}
	}
}
