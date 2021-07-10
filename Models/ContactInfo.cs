using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TelegramBot.Models
{
    public class ContactInfo
    {
        [Key]
        public long  fldChatId { get; set; }
        public ChatType fldChatType { get; set; }
        public ChatState fldChatState { get; set; }
        // [Required] 
        [Column(TypeName = "varchar(50)")]
      	public string fldMobileNumberOrId{get;set;}// [fldMobileNumber] [varchar](50) NULL,
    }

    public enum ChatType : int
    {
        group= 0,
        channel=1,
        person=2
    }

    public enum ChatState : int
    {
        start=0, //new chat Id recieved
        subscribing= 1, // subscribe request clicked
        getMobileNumber=2,// send mobileNumber request
        subscribed=3,// subscribed
        unsubscribe=20
    }
}