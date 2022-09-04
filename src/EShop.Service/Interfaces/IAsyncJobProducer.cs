namespace EShop.Service.Interfaces
{
    public interface IAsyncJobProducer
    {
        Task PublishAsync<T>(T message, CancellationToken cancellationToken);
    }
}
