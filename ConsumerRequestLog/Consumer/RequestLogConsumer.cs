using EasyNetQ;
using EShop.Domain.Common.BrokerMessages;
using EShop.Domain.Interfaces;
using EShop.Domain.Models;
using EShop.Repository;
using EShop.Service.Implementations;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace ConsumerRequestLog.Consumer
{
    public class RequestLogConsumer : AsyncJobConsumer<RequestLogMessage>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public RequestLogConsumer(IBus bus, IServiceScopeFactory serviceScopeFactory) : base(bus)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task OnMessage(RequestLogMessage message)
        {
            using var scop = _serviceScopeFactory.CreateAsyncScope();

            var requestLogRepository = scop.ServiceProvider.GetRequiredService<IRequestLogRepository>();

            var requestLog = new RequestLog()
            {
                Id = new Guid(),
                AccessToken = message.AccessToken,
                IpAddress = message.IpAddress,
                Path = message.Path,
                ContentType = message.ContentType,
            };

            Console.WriteLine("********** Start Saving To Db ************");
            Console.WriteLine("*************** Path:" + message.Path + " accesstoken:" + message.AccessToken + " contecttype:" + message.ContentType + " ");
            await requestLogRepository.AddAsync(requestLog, CancellationToken.None);
            //await _dbContext.SaveChangesAsync();
            Console.WriteLine("**********  End Saving To Db  ************");

            //Console.WriteLine("*************** Path:" + message.Path + " accesstoken:" + message.AccessToken + " contecttype:" + message.ContentType + " ");
        }
    }
}
