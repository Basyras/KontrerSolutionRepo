using ProtoBuf;
using System;

namespace Basyc.MessageBus.HttpProxy.Shared.Http
{
	[ProtoContract]
	public class RequestHttpDTO
	{
		protected RequestHttpDTO()
		{

		}

		public RequestHttpDTO(string requestType, bool hasResponse, byte[] requestBytes = null, string responseType = null)
		{
			MessageType = requestType;
			MessageBytes = requestBytes ?? Array.Empty<byte>();
			ResponseType = responseType;
			HasResponse = hasResponse;
		}

		[ProtoMember(1)]
		public string MessageType { get; set; }
		[ProtoMember(2)]
		public byte[] MessageBytes { get; set; }
		[ProtoMember(3)]
		public string ResponseType { get; set; }
		[ProtoMember(4)]
		public bool HasResponse { get; set; }
	}
}