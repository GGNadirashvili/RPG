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
        [HttpPost]
        public async Task<ActionResult<ServiceResponse<FightRequestDto>>> Fight(FightRequestDto request)
        {
            return Ok(await fightService.Fight(request));
        } [HttpGet]
        public async Task<ActionResult<ServiceResponse<List<HighScoreDto>>>> GetHighScore()
        {
            return Ok(await fightService.GetHighScore());
        }
    }
}
