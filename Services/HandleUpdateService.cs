using System;
using System.IO;
using System.Linq;
using TelegramBot.DbAccess;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InlineQueryResults;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using Microsoft.Extensions.DependencyInjection;

namespace TelegramBot.Services
{
    public class HandleUpdateService
    {
        private readonly ITelegramBotClient _botClient;
        private readonly ILogger<HandleUpdateService> _logger;
        private readonly IServiceScopeFactory scopeFactory;

        private string cacheKey_state_chatId ;


        public HandleUpdateService(
            ITelegramBotClient botClient,
            ILogger<HandleUpdateService> logger,
            IServiceScopeFactory scopeFactory)
        {
            this.scopeFactory = scopeFactory;
            this._botClient = botClient;
            this._logger = logger;
        }
        public async Task<Message> SendFileToChatId( long chatId)
        {
            await _botClient.SendChatActionAsync(chatId, ChatAction.UploadPhoto);

            const string filePath = @"Files/tux.png";
            using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();
            return await _botClient.SendPhotoAsync(chatId: chatId,
                                            photo: new InputOnlineFile(fileStream, fileName),
                                            caption: "Nice Picture");
        }
        public async Task<Message> SendMessageToChatId( long chatId,string message)
        {
            await _botClient.SendChatActionAsync(chatId, ChatAction.UploadPhoto);

        return await _botClient.SendTextMessageAsync(chatId: chatId,text: message);
        }
        public async Task EchoAsync(Update update)
        {
            _logger.LogInformation("update.Message.ToString()");

            var handler = update.Type switch
            {
                // UpdateType.Unknown:
                // UpdateType.ChannelPost:
                // UpdateType.EditedChannelPost:
                // UpdateType.ShippingQuery:
                // UpdateType.PreCheckoutQuery:
                // UpdateType.Poll:
                UpdateType.Message            => BotOnMessageReceived(update.Message),
                UpdateType.EditedMessage      => BotOnMessageReceived(update.Message),
                UpdateType.CallbackQuery      => BotOnCallbackQueryReceived(update.CallbackQuery),
                UpdateType.InlineQuery        => BotOnInlineQueryReceived(update.InlineQuery),
                UpdateType.ChosenInlineResult => BotOnChosenInlineResultReceived(update.ChosenInlineResult),
                _                             => UnknownUpdateHandlerAsync(update)
            };

            try
            {
                await handler;
            }
            catch (Exception exception)
            {
                await HandleErrorAsync(exception);
            }
        }
          // ???????? ??????????  ??????????????  ???????? 
        private  Task<int> SaveGroupChatId(ITelegramBotClient bot, Message message)
        {
            int result =-1;
            using (var scope = scopeFactory.CreateScope())
                { 
                    TelegramBot.Models.ContactInfo? contactInfo ;
                    var dbContext = scope.ServiceProvider.GetRequiredService<TelegramBotContext>();
                    switch (message.Type)
                    {
                        case MessageType.ChatMemberLeft:
                        contactInfo= (TelegramBot.Models.ContactInfo)dbContext.tblContactInfo.Where(b => b.fldChatId == message.Chat.Id).FirstOrDefault();
                          var removed=dbContext.tblContactInfo.Remove(contactInfo);
                          result =2;
                        break;
                        case MessageType.ChatMembersAdded:
                        contactInfo=new Models.ContactInfo(){
                            fldChatId  =message.Chat.Id,
                            fldMobileNumberOrId=message.Chat.Title,
                            fldChatType=Models.ChatType.group,
                            fldChatState=Models.ChatState.subscribed
                        };
                        dbContext.tblContactInfo.Add(contactInfo);
                    
                            result =2;
                        break;
                        default:
                        break;   
                    }
                    dbContext.SaveChanges();
                }

            return  Task.FromResult(result);
        }
        private async Task<Message>  Unsubscribe(ITelegramBotClient bot, Message message)
        {
        using (var scope = scopeFactory.CreateScope())
            { 
                TelegramBot.Models.ContactInfo? contactInfo ;
                var dbContext = scope.ServiceProvider.GetRequiredService<TelegramBotContext>();
                contactInfo= dbContext.tblContactInfo.Where(b => b.fldChatId == message.Chat.Id).FirstOrDefault() as TelegramBot.Models.ContactInfo;
                dbContext.tblContactInfo.Remove(contactInfo);
                
                dbContext.SaveChanges();
            }
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                    text: " ???? ???????????? ?????????????? ?????? ????????!");
        }
        private async Task<Message>  Subscribe(ITelegramBotClient bot, Message message)
        {
            using (var scope = scopeFactory.CreateScope())
            { 
                TelegramBot.Models.ContactInfo contactInfo ;
                var dbContext = scope.ServiceProvider.GetRequiredService<TelegramBotContext>();
                contactInfo=new Models.ContactInfo(){
                    fldChatId  =message.Chat.Id,
                    fldChatState=Models.ChatState.subscribing,
                    fldChatType=TelegramBot.Models.ChatType.person,
                    fldMobileNumberOrId=""
                };
                dbContext.tblContactInfo.Add(contactInfo);
                var result=dbContext.SaveChanges();
            }

                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                    text: "?????? ?????? ?????????????? ???????? ?????????? ???????????? ?????? ???? ???????? ????????????:");

        }
           
        private async Task BotOnMessageReceived(Message message)
        {
            _logger.LogInformation($"Receive message type: {message.Type}");
            if (message.Type != MessageType.Text)
            {
                var SaveMessage =  SaveGroupChatId(_botClient, message);;
                _logger.LogInformation($"The group was saved with id: {SaveMessage.Result}");
                return;
            }

        
            var action = message.Text.Split(' ').First() switch
            {
                "/unsubscribe"   => Unsubscribe(_botClient, message),
                "/subscribe"   => Subscribe(_botClient, message),
                "/inline"   => SendInlineKeyboard(_botClient, message),
                "/keyboard" => SendReplyKeyboard(_botClient, message),
                "/remove"   => RemoveKeyboard(_botClient, message),
                "/photo"    => SendFile(_botClient, message),
                "/request"  => RequestContactAndLocation(_botClient, message),
                _           => Usage(_botClient, message)
            };
            var sentMessage = await action;
            _logger.LogInformation($"The message was sent with id: {sentMessage.MessageId}");
        }
        // Send inline keyboard
        // You can process responses in BotOnCallbackQueryReceived handler
        static async Task<Message> SendInlineKeyboard(ITelegramBotClient bot, Message message)
        {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.Typing);

            // Simulate longer running task
            await Task.Delay(500);

            InlineKeyboardMarkup inlineKeyboard = new(new[]
            {
                // first row
                new []
                {
                    InlineKeyboardButton.WithCallbackData("?????????? ?????????? ????????", "11"),
                    InlineKeyboardButton.WithCallbackData("1.2", "12"),
                },
                // second row
                new []
                {
                    InlineKeyboardButton.WithCallbackData("2.1", "21"),
                    InlineKeyboardButton.WithCallbackData("2.2", "22"),
                },
            });
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                    text: "?????? ???? ?????????? ?????? ???? ???????????? ????????",
                                                    replyMarkup: inlineKeyboard);
        }

        static async Task<Message> SendReplyKeyboard(ITelegramBotClient bot, Message message)
        {
            ReplyKeyboardMarkup replyKeyboardMarkup = new(
                new KeyboardButton[][]
                {
                    new KeyboardButton[] { "1.1", "1.2" },
                    new KeyboardButton[] { "2.1", "2.2" },
                },
                resizeKeyboard: true
            );

            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                    text: "Choose",
                                                    replyMarkup: replyKeyboardMarkup);
        }

        static async Task<Message> RemoveKeyboard(ITelegramBotClient bot, Message message)
        {
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                    text: "Removing keyboard",
                                                    replyMarkup: new ReplyKeyboardRemove());
        }

        static async Task<Message> SendFile(ITelegramBotClient bot, Message message)
        {
            await bot.SendChatActionAsync(message.Chat.Id, ChatAction.UploadPhoto);

            const string filePath = @"Files/tux.png";
            using FileStream fileStream = new(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            var fileName = filePath.Split(Path.DirectorySeparatorChar).Last();
            return await bot.SendPhotoAsync(chatId: message.Chat.Id,
                                            photo: new InputOnlineFile(fileStream, fileName),
                                            caption: "Nice Picture");
        }

        static async Task<Message> RequestContactAndLocation(ITelegramBotClient bot, Message message)
        {
            ReplyKeyboardMarkup RequestReplyKeyboard = new(new[]
            {
                KeyboardButton.WithRequestLocation("Location"),
                KeyboardButton.WithRequestContact("Contact"),
            });
            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                    text: "Who or Where are you?",
                                                    replyMarkup: RequestReplyKeyboard);
        }

        private async Task<Message> Usage(ITelegramBotClient bot, Message message)
            {
                const string usage = "Usage:\n" +

                                    "/subscribe   - ?????? ?????? ???? ??????????????\n" +
                                    "/unsubscribe   - ???????????? ???? ??????????????\n" +
                                    "/inline   - send inline keyboard\n" +
                                    "/keyboard - send custom keyboard\n" +
                                    "/remove   - remove custom keyboard\n" +
                                    "/photo    - send a photo\n" +
                                    "/request  - request location or contact";

                using (var scope = scopeFactory.CreateScope())
                    { 
                        var dbContext = scope.ServiceProvider.GetRequiredService<TelegramBotContext>();
                        var contactInfo=dbContext.tblContactInfo.Where(inf =>inf.fldChatId ==message.Chat.Id).FirstOrDefault();

                        if (contactInfo != null && contactInfo.fldChatState==Models.ChatState.subscribing)
                        {
                            contactInfo.fldChatState=Models.ChatState.subscribed;
                            contactInfo.fldMobileNumberOrId=message.Text;
                            dbContext.SaveChanges();
                            
                            return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                            text: "???????????? ?????? ???? ?????????? '" +message.Text + "' ???? ???????????? ?????? ????.");
                        } 
                    }


                return await bot.SendTextMessageAsync(chatId: message.Chat.Id,
                                                    text: usage,
                                                    replyMarkup: new ReplyKeyboardRemove());
            }
                

        // Process Inline Keyboard callback data
        private async Task BotOnCallbackQueryReceived(CallbackQuery callbackQuery)
        {
            await _botClient.AnswerCallbackQueryAsync(callbackQuery.Id,
                                                      $"Received {callbackQuery.Data}");

            await _botClient.SendTextMessageAsync(callbackQuery.Message.Chat.Id,
                                                  $"Received {callbackQuery.Data}");
        }
        #region Inline Mode
            private async Task BotOnInlineQueryReceived(InlineQuery inlineQuery)
            {
                _logger.LogInformation($"Received inline query from: {inlineQuery.From.Id}");
                InlineQueryResultBase[] results = {
                    // displayed result
                    new InlineQueryResultArticle(
                        id: "3",
                        title: "MRHB:",
                        inputMessageContent: new InputTextMessageContent(
                            "hello, I'm here to help you to find funnies"
                        )
                    )
                };
                await _botClient.AnswerInlineQueryAsync(inlineQueryId: inlineQuery.Id,
                                                        results: results,
                                                        isPersonal: true,
                                                        cacheTime: 0);
            }
            private Task BotOnChosenInlineResultReceived(ChosenInlineResult chosenInlineResult)
            {
                _logger.LogInformation($"Received inline result: {chosenInlineResult.ResultId}");
                return Task.CompletedTask;
            }
        #endregion
        private Task UnknownUpdateHandlerAsync(Update update)
        {
            _logger.LogInformation($"Unknown update type: {update.Type}");
            return Task.CompletedTask;
        }
        public Task HandleErrorAsync(Exception exception)
        {
            var ErrorMessage = exception switch
            {
                ApiRequestException apiRequestException => $"Telegram API Error:\n[{apiRequestException.ErrorCode}]\n{apiRequestException.Message}",
                _ => exception.ToString()
            };

            _logger.LogInformation(ErrorMessage);
            return Task.CompletedTask;
        }
    }

}
