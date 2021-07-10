using System;
using System.Globalization;
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
    private Timer _timer;

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
         _timer = new Timer(DoWork, null, TimeSpan.Zero, 
            TimeSpan.FromSeconds(5));        
    }
    public  override async Task StopAsync (CancellationToken stoppingToken)
    {
            // Run your graceful clean-up actions
    }
    private async void DoWork(object? state)
    {
        PersianCalendar pc = new PersianCalendar();
        DateTime thisDate = DateTime.Now;
        string todayDate= pc.GetYear(thisDate)
                +"/"+pc.GetMonth(thisDate) 
                + "/"+pc.GetDayOfMonth(thisDate);

        using (var scope = scopeFactory.CreateScope())
            {
               var dbContext = scope.ServiceProvider.GetRequiredService<TelegramBotContext>();

               var sendList = dbContext.tblBotMessage
                .Where(
                   mes => mes.fldOK==0 &&
                   mes.flddate==todayDate)
                   .Join(
                       dbContext.tblContactInfo,
                       mes =>mes.fldMobileNumberOrId,
                       inf =>inf.fldMobileNumberOrId,
                       (Mes , inf)=>new {
                           mesId =Mes.fldid,
                           infoChatId=inf.fldChatId,
                           mes = Mes.fldMes,
                           ChatId=inf.fldChatId
                           }
                   )
                   .ToList();
                foreach (var item in sendList)
                {  
                    try
                    {
                    //  SendMessageToChatId( 851145561,"this is title:"+ title);
                    await _botClient.SendTextMessageAsync(item.ChatId,"you subscribed:"+ item.mes);
                    var entity = dbContext.tblBotMessage.FirstOrDefault(botMes => botMes.fldid == item.mesId);
                    // Validate entity is not null
                    if (entity != null)
                        {
                            entity.fldOK =Models.OkState.sent;
                            dbContext.SaveChanges();
                        }
                    }
                    catch (System.Exception c)
                    {
                        var entity = dbContext.tblBotMessage.FirstOrDefault(botMes => botMes.fldid == item.mesId);
                        // Validate entity is not null
                        if (entity != null)
                            {
                                entity.fldOK =Models.OkState.hasError;
                                entity.fldResponse=c.ToString();
                                dbContext.SaveChanges();
                            }
                    }
                }
            }
        
        return ;
    }
    public  override async Task StartAsync(CancellationToken cancellationToken)
    {
        await ExecuteAsync(cancellationToken);
    }
}
}