using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBot.DbAccess;

namespace TelegramBot.Services
{
public class MessageScheduleService
       : BackgroundService
{       
    private readonly IServiceScopeFactory scopeFactory;

    private readonly ITelegramBotClient _botClient;
    private readonly ILogger<MessageScheduleService> _logger;

    public MessageScheduleService(ITelegramBotClient botClient
    , ILogger<MessageScheduleService> logger
    ,IServiceScopeFactory scopeFactory)
        {
            //Constructor’s parameters validations... 
            this.scopeFactory = scopeFactory;
            _botClient = botClient;
            _logger=logger;
        }
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        
        string title="";
        while (!stoppingToken.IsCancellationRequested)
        {
             using (var scope = scopeFactory.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<TelegramBotContext>();

                           title= dbContext.Chat.OrderBy(c=>c.Id).Last().Title;

                
            }
            // This eShopOnContainers method is quering a database table 
            // and publishing events into the Event Bus (RabbitMS / ServiceBus)

            try
            {
            await SendMessageToChatId( 851145561,"this is title:"+ title);
            }
            catch (System.Exception)
            {
                //                throw;
            }
            await Task.Delay(1000, stoppingToken);
        }
        
    }

    public  override async Task StopAsync (CancellationToken stoppingToken)
    {
            // Run your graceful clean-up actions
    }


    public  override async Task StartAsync(CancellationToken cancellationToken)
    {
           await ExecuteAsync(cancellationToken);
    }


    public async Task<Message> SendMessageToChatId( long chatId,string message)
    {
        return await _botClient.SendTextMessageAsync(chatId: chatId,text: message);
    }
}
}