using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Telegram.Bot.Examples.WebHook.Services;
using Telegram.Bot.Types;

namespace Telegram.Bot.Examples.WebHook.Controllers
{
    public class cmdController : ControllerBase
    {

     [HttpGet]
        public string   POST()
        {
                return  "This is my default action...";
        }
    [HttpGet]
        public async Task<IActionResult> Get([FromServices] HandleUpdateService handleUpdateService)
        {
            await handleUpdateService.SendFileToChatId(851145561);
            return Ok();
        }
    }
}
