using EasyNetQ;
using EShop.Domain.Common.BrokerMessages;
using EShop.Service.Interfaces;

namespace EShop.Service.Implementations
{
    public abstract class AsyncJobConsumer<TMessage> : IAsyncJobConsumer<TMessage>
    where TMessage : BaseMessage
    {
        private readonly IBus _bus;

        //Ctor
        public AsyncJobConsumer(IBus bus)
        {
            _bus = bus;
        }


        public async Task SubscribeAsync(string subscriptionId, CancellationToken cancellationToken)
        {
            await _bus.PubSub.SubscribeAsync<TMessage>(subscriptionId, OnMessageWrapper);
        }

        public Task OnMessageWrapper(TMessage message)
        {
            try
            {
                OnMessage(message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return Task.CompletedTask;
        }

        public abstract Task OnMessage(TMessage message);
    }
}
