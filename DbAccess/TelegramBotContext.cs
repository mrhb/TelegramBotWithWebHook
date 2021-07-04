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

        public DbSet<Chat> Chat { get; set; }
    }
}