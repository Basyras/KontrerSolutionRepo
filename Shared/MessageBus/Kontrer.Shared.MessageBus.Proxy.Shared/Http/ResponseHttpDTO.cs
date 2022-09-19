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

		public ResponseHttpDTO(string messageType, int sessionId, bool hasResponse, byte[] responseData = null, string responseType = null)
		{
			MessageType = messageType;
			SessionId = sessionId;
			HasResponse = hasResponse;
			ResponseBytes = responseData ?? Array.Empty<byte>();
			ResponseType = responseType;
		}

		[ProtoMember(1)]
		public string MessageType { get; set; }
		[ProtoMember(5)]
		public int SessionId { get; }

		/// <summary>
		/// Is request/query/message that expects reponse message back.
		/// </summary>
		[ProtoMember(4)]
		public bool HasResponse { get; set; }
		[ProtoMember(2)]
		public byte[] ResponseBytes { get; set; }
		[ProtoMember(3)]
		public string ResponseType { get; set; }

	}
}
