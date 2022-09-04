using EasyNetQ;
using EShop.Domain.Common.BrokerMessages;
using EShop.Service.Implementations;

namespace EShop.Api.Consumers
{
    public class TransactionMessageConsumer : AsyncJobConsumer<TransactionMessage>
    {
        public TransactionMessageConsumer(IBus bus) : base(bus)
        {
        }

        public override Task OnMessage(TransactionMessage message)
        {
            Console.WriteLine("**********************");
            Console.WriteLine(message.TextMessage);
            Console.WriteLine("**********************");
            return Task.CompletedTask;
        }
    }
}
