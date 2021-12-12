namespace Basyc.MessageBus.Broker
{
    public interface IMessageBroker : IDisposable
    {
        void Start();
        Task StartAsync();
    }
}