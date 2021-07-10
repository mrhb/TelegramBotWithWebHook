using Microsoft.EntityFrameworkCore;
using TelegramBot.Models;

namespace TelegramBot.DbAccess
{
    public class TelegramBotContext : DbContext
    {
          public TelegramBotContext (DbContextOptions<TelegramBotContext> options)
            : base(options)
        {
        }

        public DbSet<ContactInfo> tblContactInfo { get; set; }
        public DbSet<BotMessage> tblBotMessage { get; set; }
    }
}