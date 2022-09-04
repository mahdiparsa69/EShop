using EasyNetQ;

namespace EShop.Domain.Common.BrokerMessages
{
    [Queue(queueName: "TransactionQueue", ExchangeName = "TransactionExchange")]
    public class TransactionMessage : BaseMessage
    {
        public string TextMessage { get; set; }
    }
}
