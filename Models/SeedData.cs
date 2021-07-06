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
                if (context.ContactInfo.Any())
                {
                    return;   // DB has been seeded
                }

                context.ContactInfo.AddRange(
                    new ContactInfo
                    {
                        ChatId=851145561,
                        ChatType=ChatType.person,
                        Title="hajjr"
                    },
                     new ContactInfo
                    {
                        ChatId=-1001523404462,
                        ChatType=ChatType.group,
                        Title="kosarRB Group"
                    }
                );
                context.SaveChanges();
            }
        }
    }
}