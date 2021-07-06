using System.ComponentModel.DataAnnotations;

namespace TelegramBot.Models
{
    public class ContactInfo
    {
        public int Id { get; set; }
        public long  ChatId { get; set; }
        public ChatType ChatType { get; set; }
        public string? fldSSN { get; set; } //کدملی کاربر
         public string? Title { get; set; }
    }

    public enum ChatType : int
{
   group= 0,
   channel=1,
   person=2
}
}