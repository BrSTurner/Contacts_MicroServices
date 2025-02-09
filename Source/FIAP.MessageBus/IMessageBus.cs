using MassTransit;

namespace FIAP.MessageBus
{
    public interface IMessageBus : IDisposable
    {
        Task PublishAsync<T>(T message) where T : class;
        Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class;

        void SubscribeConsumer<TConsumer, TMessage>()
            where TConsumer : class, IConsumer<TMessage>
            where TMessage : class;

        void Subscribe<T>(Action<ConsumeContext<T>> onMessage) where T : class;
        void SubscribeAsync<T>(Func<ConsumeContext<T>, Task> onMessage) where T : class;
    }
}
