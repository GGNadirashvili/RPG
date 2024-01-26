using AutoMapper;
using RPGSln.Dtos.Character;
using RPGSln.Models;

namespace RPGSln.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
        private static List<Character> characters = new List<Character> {
        new Character(),
        new Character{Id = 1, Name = "Sam"}
        };
        private readonly IMapper mapper;

        public CharacterService(IMapper mapper)
        {
            this.mapper = mapper;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            //increase id
            var character = mapper.Map<Character>(newCharacter);
            character.Id = characters.Max(c => c.Id) + 1;

            //add character
            characters.Add(character);
            serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            serviceResponse.Data = characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var character = characters.FirstOrDefault(c => c.Id == id);
            serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
            return serviceResponse;
        }
    }
}
