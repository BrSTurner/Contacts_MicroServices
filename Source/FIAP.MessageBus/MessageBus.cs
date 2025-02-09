using MassTransit;
using Polly;

namespace FIAP.MessageBus
{
    public class MessageBus : IMessageBus
    {
        private readonly IBusControl _busControl;
        private bool _isConnected;

        public bool IsConnected => _isConnected;

        public MessageBus(IBusControl busControl)
        {
            _busControl = busControl;
            TryConnect();
        }

        public async Task PublishAsync<T>(T message) where T : class
        {
            EnsureConnected();
            await _busControl.Publish(message);
        }

        public async Task<TResponse> RequestAsync<TRequest, TResponse>(TRequest request)
            where TRequest : class
            where TResponse : class
        {
            EnsureConnected();
            var client = _busControl.CreateRequestClient<TRequest>();
            var response = await client.GetResponse<TResponse>(request);
            return response.Message;
        }

        public void Subscribe<T>(Action<ConsumeContext<T>> onMessage) where T : class
        {
            EnsureConnected();
            var endpointName = $"consumer-{typeof(T).Name}";

            _busControl.ConnectReceiveEndpoint(endpointName, endpoint =>
            {
                endpoint.Handler<T>(context =>
                {
                    onMessage(context);
                    return Task.CompletedTask;
                });
            });
        }

        public void SubscribeAsync<T>(Func<ConsumeContext<T>, Task> onMessage) where T : class
        {
            EnsureConnected();
            var endpointName = $"consumer-{typeof(T).Name}";

            _busControl.ConnectReceiveEndpoint(endpointName, endpoint =>
            {
                MessageHandler<T> handler = context => onMessage(context);
                endpoint.Handler(handler);
            });
        }

        public void SubscribeConsumer<TConsumer, TMessage>()
            where TConsumer : class, IConsumer<TMessage>
            where TMessage : class
        {
            EnsureConnected();

            _busControl.ConnectReceiveEndpoint(typeof(TMessage).Name, cfg =>
            {
                cfg.ConfigureConsumer<TConsumer>(null);
            });
        }

        private void TryConnect()
        {
            if (IsConnected) return;

            var policy = Policy.Handle<Exception>()
                .WaitAndRetry(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));

            policy.Execute(() =>
            {
                _busControl.Start();
                _isConnected = true;
            });
        }

        private void EnsureConnected()
        {
            if (!_isConnected)
            {
                TryConnect();
            }
        }

        public void Dispose()
        {
            _busControl?.Stop();
            _isConnected = false;
        }
    }
}
