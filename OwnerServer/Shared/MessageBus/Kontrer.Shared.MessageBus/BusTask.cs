using Basyc.MessageBus.Shared;
using OneOf;
using System;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
	public struct BusTaskCompleted
	{
	}

	public class BusTask : BusTask<BusTaskCompleted>
	{
		public static BusTask FromTask(int sessionId, Task nestedTask)
		{
			var wrapperTask = nestedTask.ContinueWith(x =>
			{
				return (OneOf<BusTaskCompleted, ErrorMessage>)new BusTaskCompleted();
			});
			return new BusTask(sessionId, wrapperTask);

		}
		public static BusTask FromBusTask(int sessionId, BusTask<BusTaskCompleted> busTask)
		{
			return new BusTask(sessionId, busTask.Task);
		}

		public static BusTask FromBusTask(BusTask<BusTaskCompleted> busTask)
		{
			return new BusTask(busTask.SessionId, busTask.Task);
		}


		protected BusTask(int sessionId, Task<OneOf<BusTaskCompleted, ErrorMessage>> value) : base(sessionId, value) { }
		protected BusTask(int sessionId, ErrorMessage error) : base(sessionId, error) { }
		protected BusTask(int sessionId, BusTaskCompleted value) : base(sessionId, value) { }
	}

	public class BusTask<TValue>
	{

		public static BusTask<TValue> FromTask(int sessionId, Task<OneOf<TValue, ErrorMessage>> nestedTask)
		{
			return new BusTask<TValue>(sessionId, nestedTask);
		}

		public static BusTask<TValue> FromTask(int sessionId, Task<TValue> nestedTask)
		{
			var wrapperTask = nestedTask.ContinueWith<OneOf<TValue, ErrorMessage>>(x =>
			{
				if (x.IsCompletedSuccessfully)
				{
					return (OneOf<TValue, ErrorMessage>)x.Result;
				}

				if (x.IsCanceled)
					return new ErrorMessage("Canceled");

				return new ErrorMessage(x.Exception.Message);
			});
			return FromTask(sessionId, wrapperTask);
		}
		public static BusTask<TValue> FromTask<TNestedValue>(int sessionId, Task<TNestedValue> nestedTask, Func<TNestedValue, OneOf<TValue, ErrorMessage>> converter)
		{
			var wrapperTask = nestedTask.ContinueWith<OneOf<TValue, ErrorMessage>>(x =>
			{
				if (x.IsCompletedSuccessfully)
				{

					var converterResult = converter.Invoke(x.Result);
					return converterResult;
				}

				if (x.IsCanceled)
					return new ErrorMessage("Canceled");

				return new ErrorMessage(x.Exception.Message);

			});
			return FromTask(sessionId, wrapperTask);
		}
		public static BusTask<TValue> FromTask<TNestedValue>(int sessionId, Task<OneOf<TNestedValue, ErrorMessage>> nestedTask, Func<TNestedValue, OneOf<TValue, ErrorMessage>> converter)
		{
			var wrapperTask = nestedTask.ContinueWith<OneOf<TValue, ErrorMessage>>(x =>
			{
				if (x.IsCompletedSuccessfully)
				{
					return x.Result.Match<OneOf<TValue, ErrorMessage>>(
					nestedValue => converter.Invoke(nestedValue),
					error => error);
				}

				if (x.IsCanceled)
					return new ErrorMessage("Canceled");

				return new ErrorMessage(x.Exception.Message);

			});
			return FromTask(sessionId, wrapperTask);
		}
		public static BusTask<TValue> FromError(int sessionId, ErrorMessage error)
		{
			return new BusTask<TValue>(sessionId, error);
		}
		public static BusTask<TValue> FromValue(int sessionId, TValue value)
		{
			return new BusTask<TValue>(sessionId, value);
		}
		public static BusTask<TValue> FromBusTask<TNestedValue>(BusTask<TNestedValue> nestedBusTask, Func<TNestedValue, OneOf<TValue, ErrorMessage>> converter)
		{
			return FromBusTask<TNestedValue>(nestedBusTask.SessionId, nestedBusTask, converter);
		}
		public static BusTask<TValue> FromBusTask<TNestedValue>(int sessionId, BusTask<TNestedValue> nestedBusTask, Func<TNestedValue, OneOf<TValue, ErrorMessage>> converter)
		{
			return FromTask(sessionId, nestedBusTask.Task, converter);
		}

		protected BusTask(int sessionId, Task<OneOf<TValue, ErrorMessage>> value)
		{
			SessionId = sessionId;
			Task = value;
		}

		protected BusTask(int sessionId, ErrorMessage error)
		{
			SessionId = sessionId;
			Task = System.Threading.Tasks.Task.FromResult<OneOf<TValue, ErrorMessage>>(error);
		}

		protected BusTask(int sessionId, TValue value)
		{
			SessionId = sessionId;
			Task = System.Threading.Tasks.Task.FromResult<OneOf<TValue, ErrorMessage>>(value);
		}

		public Task<OneOf<TValue, ErrorMessage>> Task { get; init; }
		public int SessionId { get; init; }

		public BusTask<TNestedValue> ContinueWith<TNestedValue>(Func<TValue, OneOf<TNestedValue, ErrorMessage>> converter)
		{
			return BusTask<TNestedValue>.FromBusTask(this, converter);
		}

		public BusTask ToBusTask()
		{
			return BusTask.FromTask(SessionId, this.Task);
		}
	}
}
