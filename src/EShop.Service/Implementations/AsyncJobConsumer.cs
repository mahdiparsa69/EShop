using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EShop.Domain.Common.BrokerMessages;
using EShop.Domain.Enums;
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
            Console.WriteLine("********************* Rabbit MQ Started *********************");
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
