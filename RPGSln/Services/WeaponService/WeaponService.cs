using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPGSln.Data;
using RPGSln.Dtos.Character;
using RPGSln.Dtos.Weapon;
using RPGSln.Models;
using System.Security.Claims;

namespace RPGSln.Services.WeaponService
{
    public class WeaponService : IWeaponService
    {
        private readonly DataContext context;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly IMapper mapper;

        public WeaponService(DataContext context, IHttpContextAccessor httpContextAccessor, IMapper mapper)
        {
            this.context = context;
            this.httpContextAccessor = httpContextAccessor;
            this.mapper = mapper;
        }
        public async Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var charachter = await context.Characters
                     .FirstOrDefaultAsync(c => c.Id == newWeapon.CharacterId &&
                      c.User!.Id == int.Parse(httpContextAccessor.HttpContext!.User
                     .FindFirstValue(ClaimTypes.NameIdentifier)!));

                if (charachter is null)
                {
                    response.Succes = false;
                    response.Message = "Charachter not found";
                    return response;
                }

                var weapon = new Weapon
                {
                    Name = newWeapon.Name,
                    Damage = newWeapon.Damage,
                    Character = charachter
                };

                context.Weapons.Add(weapon);
                await context.SaveChangesAsync();
                response.Data = mapper.Map<GetCharacterDto>(charachter);
            }
            catch (Exception ex)
            {
                response.Succes = false;
                response.Message = ex.Message;
            }
            return response;
        }
    }
}
