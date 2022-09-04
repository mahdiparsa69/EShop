using EasyNetQ;
using EShop.Service.Interfaces;

namespace EShop.Service.Implementations
{
    public class AsyncJobProducer : IAsyncJobProducer
    {
        private readonly IBus _bus;
        public AsyncJobProducer(IBus bus)
        {
            _bus = bus;
        }
        public async Task PublishAsync<T>(T message, CancellationToken cancellationToken)
        {
            await _bus.PubSub.PublishAsync(message, cancellationToken);
        }
    }
}
