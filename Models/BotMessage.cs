using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using System.Linq;

namespace TelegramBot.Models
{
    public class BotMessage
    {
        [Key]
      	public int fldid{get;set;}//	[fldid] [int] IDENTITY(1,1) NOT NULL,
           
        [Required] 
        [Column(TypeName = "varchar(50)")]
      	public string fldMobileNumberOrId{get;set;}// [fldMobileNumber] [varchar](50) NULL,
	      [Column(TypeName = "varchar(1000)")]
      	public string fldMes{get;set;}	// [fldMes] [varchar](1000) NULL,
        public byte[] ImageData { get; set; }
	      public OkState? fldOK { get; set; }// [fldOK] [smallint] NOT NULL,
        [Column(TypeName = "varchar(20)")]
      	public string fldTime{get;set;}	// [fldTime] [varchar](20) NULL,
        [Column(TypeName = "varchar(50)")]
      	public string flddate{get;set;} // [flddate] [nvarchar](50) NULL,  [fldid]
      	public DateTime? fldSendTime{get;set;} // زمان ارسال 
      	
        public string? fldResponse{get;set;} // نتیجه ارسال پیام(موق/ناموق/ پیام خطا)



    
    }

 


    public enum OkState : int
{
  
  
   IsSending= 10, // آماده ارسال

   hasError=100,
   canceled=1, // انصراف از ارسال
   sent=2 //ارسال شده است
}
}