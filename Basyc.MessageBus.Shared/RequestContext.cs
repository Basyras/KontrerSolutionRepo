namespace Basyc.MessageBus.Shared
{
	/// <summary>
	/// 
	/// </summary>
	/// <param name="RequesterSpanId"></param>
	/// <param name="TraceId">When not specifiyng Trace id, trace id will be generated</param>
	public record struct RequestContext(string? RequesterSpanId, string? TraceId);
}
