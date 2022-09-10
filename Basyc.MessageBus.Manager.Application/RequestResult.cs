using Basyc.MessageBus.Manager.Application.Durations;
using System;

namespace Basyc.MessageBus.Manager.Application
{
	public class RequestResult
	{

		private readonly DurationMapBuilder durationMapBuilder;

		public Request Request { get; init; }
		public DateTimeOffset RequestCreationTime { get; init; }
		public int Id { get; init; }
		public RequestResultState State { get; private set; }
		public object? Response { get; private set; }
		public string? ErrorMessage { get; private set; }
		public DurationMap? DurationMap { get; private set; }


		public RequestResult(Request request, DateTimeOffset requestCreationTime, int id)
		{
			Request = request;
			durationMapBuilder = new DurationMapBuilder();
			RequestCreationTime = requestCreationTime;
			Id = id;
			State = RequestResultState.Started;
		}

		/// <summary>
		/// You should call <see cref="Start"/> in moment that all internal processes are done and from now only work related to handeling a request are in process.
		/// </summary>
		public void Start()
		{
			if (durationMapBuilder.HasStartedCounting)
				throw new InvalidOperationException($"{nameof(Start)} was already called");
			durationMapBuilder.Start();
		}

		public DurationSegmentBuilder StartNewSegment(string segmentName)
		{
			return durationMapBuilder.StartNewSegment(segmentName);
		}

		public void Complete(object? response)
		{
			FinishDurationMap();
			if (Request.RequestInfo.HasResponse is false)
			{
				throw new InvalidOperationException($"Can't complete with return value becuase this message does not have return value");
			}

			State = RequestResultState.Completed;
			Response = response;
			OnStateChanged();
		}

		public void Complete()
		{
			FinishDurationMap();

			if (Request.RequestInfo.HasResponse)
			{
				throw new InvalidOperationException($"Can't complete without return value becuase this message has return value. Use {nameof(Fail)} method when error occured and no return value is avaible");
			}

			State = RequestResultState.Completed;
			OnStateChanged();
		}

		public void Fail(string errorMessage)
		{
			FinishDurationMap();
			State = RequestResultState.Failed;
			ErrorMessage = errorMessage;
			OnStateChanged();
		}

		public event EventHandler? StateChanged;
		private void OnStateChanged()
		{
			StateChanged?.Invoke(this, EventArgs.Empty);
		}

		private void FinishDurationMap()
		{
			DurationMap = durationMapBuilder.Build();

		}
	}
}