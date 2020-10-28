using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using KuberaManager.Models.Data.Ajax;
using KuberaManager.Models.Database;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace KuberaManager.Controllers.Api
{
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AjaxController : ControllerBase
    {
        // GET api/ajax/<AjaxController>/<arg>
        [HttpGet]
        public List<BotStatus> AllBotStatus()
        {
            return BotStatus.GetAllBotStatus();
        }

        [HttpGet("{accountId}")]
        public Levels GetLevels(int accountId)
        {
            return Levels.FromAccount(accountId);
        }
    }
}
