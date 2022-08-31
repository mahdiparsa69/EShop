using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EShop.Service.Interfaces
{
    public interface IAsyncJobProducer
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken);
    }
}
