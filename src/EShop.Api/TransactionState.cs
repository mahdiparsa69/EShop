using EasyNetQ;
using EShop.Domain.Enums;
using EShop.Domain.Models;

namespace EShop.Api
{

    public class TransactionState
    {
        /*  public async Task StartAsync(CancellationToken cancellationToken)
          {
              using (var bus = RabbitHutch.CreateBus("host=localhost"))
              {
                  Console.WriteLine("*********************Rabbit MQ Strated*********************************");
                  await bus.PubSub.SubscribeAsync<Transaction>("mparsa", HandleTransactionState, cancellationToken);
                  *//* Console.WriteLine("********************Listening for messages*****************************");
                   Console.ReadLine();*//*
              }
              //return Task.CompletedTask;
          }*/
        /*static void HandleTransactionState(Transaction transaction)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            if (transaction.Status == TransactionStatus.Successful)
                Console.WriteLine("***********************************Transaction Successfully Done*****************************************************");

            Console.WriteLine("*****************************************************Transaction Aborted*****************************************************");
        }*/

        /*public Task StopAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => { Console.WriteLine("********************Stoped Transaction status checking ********************"); }, cancellationToken);

            return Task.CompletedTask;
        }*/
    }

    /*public class TransactionState
    {
        public async Task Main()
        {
            using (var bus = RabbitHutch.CreateBus("host=localhost"))
            {
                await bus.PubSub.SubscribeAsync<Transaction>("mparsa", HandleTransactionState);

            }
            //return Task.CompletedTask;

            static void HandleTransactionState(Transaction transaction)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                if (transaction.Status == TransactionStatus.Successful)
                    Console.WriteLine("***********************************Transaction Successfully Done*****************************************************");

                Console.WriteLine("*****************************************************Transaction Aborted*****************************************************");
            }
        }
    }*/
}
