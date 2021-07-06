using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Types;
using TelegramBot.DbAccess;
using System.Linq;
using TelegramBot.Services;

namespace KosarRB_TelegramBot.Controllers
{
    public class WebhookController : ControllerBase
    {
        private readonly TelegramBotContext _context;
        private readonly string  _title;
        public WebhookController(TelegramBotContext context)
        {
            _context = context;
      }

        [HttpPost]
        public async Task<IActionResult> Post([FromServices] HandleUpdateService handleUpdateService,
                                              [FromBody] Update update)
        {
            await handleUpdateService.EchoAsync(update);
            return Ok();
        }

         [HttpGet]
        public async Task<IActionResult> Get([FromServices] HandleUpdateService handleUpdateService)
        {
            await handleUpdateService.SendMessageToChatId(851145561,_title );
            return Ok();
        }
    }
}
