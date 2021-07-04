using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using TelegramBot.DbAccess;

namespace TelegramBot.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new TelegramBotContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<TelegramBotContext>>()))
            {
                // Look for any chats.
                if (context.Chat.Any())
                {
                    return;   // DB has been seeded
                }

                context.Chat.AddRange(
                    new Chat
                    {
                        ChatId=851145561,
                        Title = "When Harry Met Sally",
                        
                    },

                    new Chat
                    {
                        ChatId=851145561,
                        Title = "Ghostbusters ",
                    },

                    new Chat
                    {
                        ChatId=851145561,
                        Title = "Ghostbusters 2",
                    }

                );
                context.SaveChanges();
            }
        }
    }
}