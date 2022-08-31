using EShop.Domain.Common.BrokerMessages;
using EShop.Service;
using EShop.Service.Interfaces;

namespace EShop.Api.Consumers;

public class TransactionMessageConsumerHost : BaseAsyncJobConsumerHostedService<TransactionMessage>
{
    public TransactionMessageConsumerHost(IAsyncJobConsumer<TransactionMessage> consumer) : base(consumer)
    {
    }
}