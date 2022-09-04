using EShop.Domain.Common.BrokerMessages;

namespace EShop.Service.Interfaces
{
    public interface IAsyncJobConsumer<TMessage>
    where TMessage : BaseMessage
    {
        public Task SubscribeAsync(string subscriptionId, CancellationToken cancellationToken);
    }
}
