using ProtoBuf;
using System;

namespace Basyc.MessageBus.HttpProxy.Shared
{
	[ProtoContract]
	public class ProxyRequest
	{
		protected ProxyRequest()
		{

		}

		public ProxyRequest(string requestType, bool hasResponse, byte[] requestData = null, string responseType = null)
		{
			MessageType = requestType;
			MessageData = requestData ?? Array.Empty<byte>();
			ResponseAssemblyQualifiedTypeName = responseType;
			HasResponse = hasResponse;
		}

		[ProtoMember(1)]
		public string MessageType { get; set; }
		[ProtoMember(2)]
		public byte[] MessageData { get; set; }
		[ProtoMember(3)]
		public string ResponseAssemblyQualifiedTypeName { get; set; }
		[ProtoMember(4)]
		public bool HasResponse { get; set; }
	}
}