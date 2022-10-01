using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Basyc.MessageBus.Client.Diagnostics.Sinks.Http
{
	public class HttpBusClientLogExporter : IBusClientLogExporter
	{
		private record struct LogStorageKey(string HandlerDisplayName, string TraceId);


		private readonly Dictionary<LogStorageKey, List<string>> messageMap = new();
		private readonly IOptions<HttpLogSinkOptions> options;
		private readonly RestClient httpClient;
		private readonly ILogger<HttpBusClientLogExporter> logger;

		public HttpBusClientLogExporter(IOptions<HttpLogSinkOptions> options, HttpClient httpClient, LoggerToBypassCircularDepedency<HttpBusClientLogExporter> logger)
		{
			this.options = options;
			this.httpClient = new RestClient(httpClient, new RestClientOptions(options.Value.HttpAddressToSendLogs));
			this.logger = logger;
		}

		public void SendLog<TState>(string handlerDisplayName, LogLevel logLevel, string traceId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			var logKey = new LogStorageKey(handlerDisplayName, traceId);

			var message = formatter.Invoke(state, exception);
			messageMap.TryAdd(logKey, new List<string>());
			messageMap[logKey].Add(message);
			var logMessageDTO = new LogMessageDTO(logLevel, DateTimeOffset.UtcNow, traceId, message);
			SendViaHttp(logMessageDTO);
		}

		private void SendViaHttp(LogMessageDTO logMessageDTO)
		{
			var request = new RestRequest("", Method.Post).AddJsonBody(logMessageDTO);
			var sendResult = httpClient.Execute(request);
			if (sendResult.IsSuccessful is false)
			{
				logger.LogError("Failed to log message");
			}
		}
	}
}