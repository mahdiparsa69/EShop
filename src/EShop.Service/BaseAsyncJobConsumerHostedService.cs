using EShop.Domain.Common.BrokerMessages;
using EShop.Service.Interfaces;
using Microsoft.Extensions.Hosting;

namespace EShop.Service;

public class BaseAsyncJobConsumerHostedService<TMessage> : IHostedService
where TMessage : BaseMessage
{
    private readonly IAsyncJobConsumer<TMessage> _consumer;

    public BaseAsyncJobConsumerHostedService(IAsyncJobConsumer<TMessage> consumer)
    {
        _consumer = consumer;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await _consumer.SubscribeAsync("Consumer", cancellationToken);
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}