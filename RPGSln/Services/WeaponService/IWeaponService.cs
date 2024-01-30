using RPGSln.Dtos.Character;
using RPGSln.Dtos.Weapon;
using RPGSln.Models;

namespace RPGSln.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDto>> AddWeapon(AddWeaponDto newWeapon);
    }
}
