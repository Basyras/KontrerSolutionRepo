using ProtoBuf;
using System;

namespace Basyc.MessageBus.HttpProxy.Shared.Http
{
	[ProtoContract]
	public class ResponseHttpDTO
	{
		protected ResponseHttpDTO()
		{

		}

		public ResponseHttpDTO(int sessionId) : this(sessionId,null,null)
		{
		}

		public ResponseHttpDTO(int sessionId, byte[]? responseData, string? responseType)
		{
			SessionId = sessionId;
			ResponseBytes = responseData ?? Array.Empty<byte>();
			ResponseType = responseType;
		}

		[ProtoMember(5)]
		public int SessionId { get; }	
		[ProtoMember(2)]
		public byte[]? ResponseBytes { get; set; }
		[ProtoMember(3)]
		public string? ResponseType { get; set; }

	}
}
