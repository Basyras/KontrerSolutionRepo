namespace Basyc.MessageBus.Client.NetMQ
{
	public class MessageHandlerManagerOptions
	{
		public List<NetMQMessageHandlerInfo> HandlerInfos { get; } = new List<NetMQMessageHandlerInfo>();
	}
}