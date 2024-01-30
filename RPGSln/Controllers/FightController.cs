using Microsoft.AspNetCore.Mvc;
using RPGSln.Dtos.Fight;
using RPGSln.Models;
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

        [HttpPost("Weapon")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> WeaponAttack(WeaponAttackDto request)
        {
            return Ok(await fightService.WeaponAttack(request));
        } 
        [HttpPost("Skill")]
        public async Task<ActionResult<ServiceResponse<AttackResultDto>>> SkillAttackDto(SkillAttackDto request)
        {
            return Ok(await fightService.SkillAttack(request));
        }
    }
}
