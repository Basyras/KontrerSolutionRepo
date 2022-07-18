using System;

namespace Basyc.MessageBus.Manager.Application
{
	public class RequestResult
	{
		public RequestResult(Request request, bool failed, string errorMessage, DateTime requestTime, TimeSpan duration)
			: this(request, failed, null, errorMessage, requestTime, duration, false)
		{
		}

		public RequestResult(Request request, bool failed, string response, string errorMessage, DateTime requestTime, TimeSpan duration)
			: this(request, failed, response, errorMessage, requestTime, duration, true)
		{

		}
		private RequestResult(Request request, bool failed, string response, string errorMessage, DateTime requestTime, TimeSpan duration, bool hasResponse)
		{
			Request = request;
			HasError = failed;
			HasResponse = true;
			Response = response;
			ErrorMessage = errorMessage;
			RequestTime = requestTime;
			Duration = duration;
		}

		public Request Request { get; init; }
		public bool HasResponse { get; init; }
		public string Response { get; init; }
		public bool HasError { get; init; }
		public string ErrorMessage { get; init; }
		public DateTime RequestTime { get; init; }
		public TimeSpan Duration { get; init; }
	}
}