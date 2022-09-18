using Basyc.MessageBus.Shared;
using OneOf;
using System;
using System.Threading.Tasks;

namespace Basyc.MessageBus.Client
{
	[Obsolete("Not used")]
	public struct BusTask<TValue>
	{

		public static BusTask<TValue> FromTask(int sessioId, Task<OneOf<TValue, ErrorMessage>> value)
		{
			return new BusTask<TValue>(sessioId, value);
		}

		public static BusTask<TValue> FromError(int sessioId, ErrorMessage error)
		{
			return new BusTask<TValue>(sessioId, error);
		}

		public static BusTask<TValue> FromValue(int sessioId, TValue value)
		{
			return new BusTask<TValue>(sessioId, value);
		}

		public static BusTask<TValue> FromBusTask<TNestedValue>(int sessioId, BusTask<TNestedValue> nestedBusTask, Func<TNestedValue, TValue> converter)
		{
			var task = nestedBusTask.Value.ContinueWith<OneOf<TValue, ErrorMessage>>(x =>
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
			return new BusTask<TValue>(sessioId, task);
		}

		private BusTask(int sessionId, Task<OneOf<TValue, ErrorMessage>> value)
		{
			SessionId = sessionId;
			Value = value;
		}

		private BusTask(int sessionId, ErrorMessage error)
		{
			SessionId = sessionId;
			Value = Task.FromResult<OneOf<TValue, ErrorMessage>>(error);
		}

		private BusTask(int sessionId, TValue value)
		{
			SessionId = sessionId;
			Value = Task.FromResult<OneOf<TValue, ErrorMessage>>(value);
		}

		public Task<OneOf<TValue, ErrorMessage>> Value { get; init; }
		public int SessionId { get; init; }
	}
}
