using AutoMapper;
using RPGSln.Dtos.Character;
using RPGSln.Dtos.Weapon;
using RPGSln.Models;

namespace RPGSln
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDto>().ReverseMap();
            CreateMap<Character, AddCharacterDto>().ReverseMap();
            CreateMap<Weapon, GetWeaponDto>().ReverseMap();
        }
    }
}
