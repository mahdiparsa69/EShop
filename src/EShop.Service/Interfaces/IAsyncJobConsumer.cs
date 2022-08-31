using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;
using EShop.Domain.Common.BrokerMessages;
using Microsoft.Extensions.Hosting;

namespace EShop.Service.Interfaces
{
    public interface IAsyncJobConsumer<TMessage>
    where TMessage : BaseMessage
    {
        //todo
        public Task SubscribeAsync(string subscriptionId, CancellationToken cancellationToken);
    }
}
