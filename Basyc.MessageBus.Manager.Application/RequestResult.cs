using System;

namespace Basyc.MessageBus.Manager.Application
{
	public class RequestResult
	{
		public RequestResult(Request request, DateTime requestStartTime, int id)
		{
			Request = request;
			RequestStartTime = requestStartTime;
			Id = id;
			State = RequestResultState.Started;
		}

		public Request Request { get; init; }
		public DateTime RequestStartTime { get; init; }
		public int Id { get; init; }
		public RequestResultState State { get; private set; }
		public string? Response { get; private set; }
		public string? ErrorMessage { get; private set; }
		public TimeSpan? Duration { get; private set; }

		public void Complete(TimeSpan duration, string response)
		{
			State = RequestResultState.Completed;
			Response = response;
			Duration = duration;
			OnStateChanged();
		}
		public void Complete(TimeSpan duration)
		{
			State = RequestResultState.Completed;
			Duration = duration;
			OnStateChanged();
		}

		public void Fail(TimeSpan duration, string errorMessage)
		{
			State = RequestResultState.Failed;
			ErrorMessage = errorMessage;
			Duration = duration;
			OnStateChanged();
		}

		public event EventHandler? StateChanged;
		private void OnStateChanged()
		{
			StateChanged?.Invoke(this, EventArgs.Empty);
		}
	}
}