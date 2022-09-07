using EShop.Domain.Common.BrokerMessages;
using EShop.Service;
using EShop.Service.Interfaces;

namespace ConsumerRequestLog.Consumer
{
    public class RequestLogConsumerHost : BaseAsyncJobConsumerHostedService<RequestLogMessage>
    {
        public RequestLogConsumerHost(IAsyncJobConsumer<RequestLogMessage> consumer) : base(consumer)
        {
        }
    }
}
