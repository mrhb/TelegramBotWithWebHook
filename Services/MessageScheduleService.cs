using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

public class MessageScheduleService
       : BackgroundService
{       

    public MessageScheduleService()
        {
            //Constructor’s parameters validations...      
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
         
            while (!stoppingToken.IsCancellationRequested)
            {
              
                // This eShopOnContainers method is quering a database table 
                // and publishing events into the Event Bus (RabbitMS / ServiceBus)
                //CheckConfirmedGracePeriodOrders();

            //    await Task.Delay(_settings.CheckUpdateTime, stoppingToken);
            }
         
        }

        public  override async Task StopAsync (CancellationToken stoppingToken)
        {
               // Run your graceful clean-up actions
        }
}