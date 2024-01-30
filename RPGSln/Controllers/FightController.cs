using Microsoft.AspNetCore.Mvc;
using RPGSln.Services.FightService;

namespace RPGSln.Controllers
{

    [ApiController]
    [Route("[controller]")]

    public class FightController : ControllerBase
    {
        private readonly IFightService fightService;

        public FightController(IFightService fightService)
        {
            this.fightService = fightService;
        }


    }
}
