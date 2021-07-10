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
                if (context.tblContactInfo.Any())
                {
                    return;   // DB has been seeded
                }

                context.tblContactInfo.AddRange(
                    new ContactInfo
                    {
                        fldChatId=851145561,
                        fldChatType=ChatType.person,
                        fldMobileNumberOrId="09151575793",
                        fldChatState=ChatState.subscribed
                    },
                     new ContactInfo
                    {
                        fldChatId=-1001523404462,
                        fldChatType=ChatType.group,
                        fldMobileNumberOrId="@kosarRB",
                        fldChatState=ChatState.subscribed
                    }
                );
                context.SaveChanges();
            }
        }
    }
}