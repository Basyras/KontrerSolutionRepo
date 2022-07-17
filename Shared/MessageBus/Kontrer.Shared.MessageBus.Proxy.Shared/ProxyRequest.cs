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

		public ProxyRequest(string requestType, bool hasResponse, byte[] requestBytes = null, string responseType = null)
		{
			MessageType = requestType;
			MessageBytes = requestBytes ?? Array.Empty<byte>();
			ResponseAssemblyQualifiedTypeName = responseType;
			HasResponse = hasResponse;
		}

		[ProtoMember(1)]
		public string MessageType { get; set; }
		[ProtoMember(2)]
		public byte[] MessageBytes { get; set; }
		[ProtoMember(3)]
		public string ResponseAssemblyQualifiedTypeName { get; set; }
		[ProtoMember(4)]
		public bool HasResponse { get; set; }
	}
}