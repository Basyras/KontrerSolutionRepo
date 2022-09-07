using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Net.Http;

namespace Basyc.MessageBus.Client.Diagnostics.Sinks.Http
{
	public class HttpLogSink : ILogSink
	{
		private readonly Dictionary<LogStorageKey, List<string>> messageMap = new();
		private readonly IOptions<HttpLogSinkOptions> options;
		private readonly RestClient httpClient;
		private readonly ILogger<HttpLogSink> logger;

		public HttpLogSink(IOptions<HttpLogSinkOptions> options, HttpClient httpClient, LoggerToBypassCircularDepedency<HttpLogSink> logger)
		{
			this.options = options;
			this.httpClient = new RestClient(httpClient, new RestClientOptions(options.Value.HttpAddressToSendLogs));
			this.logger = logger;
		}

		public void SendLog<TState>(string handlerDisplayName, LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			var logKey = new LogStorageKey(handlerDisplayName, eventId);

			var message = formatter.Invoke(state, exception);
			messageMap.TryAdd(logKey, new List<string>());
			messageMap[logKey].Add(message);
			var logMessageDTO = new LogMessageDTO(logLevel, eventId, message);
			var request = new RestRequest("", Method.Post).AddJsonBody(logMessageDTO);
			var sendResult = httpClient.Execute(request);
			if (sendResult.IsSuccessful is false)
			{
				logger.LogError("Failed to log message");
			}
		}
	}
}