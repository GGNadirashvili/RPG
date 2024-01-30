using Microsoft.EntityFrameworkCore;
using RPGSln.Data;
using RPGSln.Dtos.Fight;
using RPGSln.Models;

namespace RPGSln.Services.FightService
{
    public class FightService : IFightService
    {
        private readonly DataContext context;

        public FightService(DataContext context)
        {
            this.context = context;
        }

        public async Task<ServiceResponse<AttackResultDto>> WeaponAttack(WeaponAttackDto request)
        {
            var response = new ServiceResponse<AttackResultDto>();

            try
            {
                var attacker = await context.Characters
                    .Include(c => c.Weapon)
                    .FirstOrDefaultAsync(c => c.Id == request.AttackerId);
                var opponent = await context.Characters
                    .FirstOrDefaultAsync(c => c.Id == request.OpponentId);

                if (attacker is null || opponent is null || attacker.Weapon is null)
                    throw new Exception("Smething fishy is going on here...");

                int damage = attacker.Weapon.Damage + (new Random().Next(attacker.Strenght));
                damage -= new Random().Next(opponent.Defeats);
                if (damage > 0)
                    opponent.HitPoints -= damage;

                if (opponent.HitPoints <= 0)
                    response.Message = $"{opponent.Name} has been defeated!";

                await context.SaveChangesAsync();
                response.Data = new AttackResultDto
                {
                    Attacker = attacker.Name,
                    Opponent = opponent.Name,
                    AttackerHP = attacker.HitPoints,
                    OpponentHp = opponent.HitPoints,
                    Damage = damage
                };

            }
            catch (Exception ex)
            {
                response.Succes = false;
                response.Message = ex.Message;
                return response;
            }

            return response;

        }
    }
}
