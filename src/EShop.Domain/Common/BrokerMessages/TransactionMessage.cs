using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;
using EasyNetQ;

namespace EShop.Domain.Common.BrokerMessages
{
    [Queue(queueName: "TransactionQueue", ExchangeName = "TransactionExchange")]
    public class TransactionMessage : BaseMessage
    {
        public string TextMsg { get; set; }
    }
}
