using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Models
{
    public class Chat
    {
        public int Id { get; set; }
        public int ChatId { get; set; }
        public string Title { get; set; }
    }
}