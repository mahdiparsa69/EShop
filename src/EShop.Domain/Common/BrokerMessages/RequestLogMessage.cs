using EasyNetQ;

namespace EShop.Domain.Common.BrokerMessages
{
    [Queue(queueName: "RequestLogQueue", ExchangeName = "RequestLogExchange")]
    public class RequestLogMessage : BaseMessage
    {
        public string? Path { get; set; }

        public string? IpAddress { get; set; }

        public string? AccessToken { get; set; }

        public string? ContentType { get; set; }
    }
}
