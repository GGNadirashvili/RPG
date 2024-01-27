using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPGSln.Data;
using RPGSln.Dtos.Character;
using RPGSln.Models;

namespace RPGSln.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {
   
        private readonly IMapper mapper;
        private readonly DataContext context;

        public CharacterService(IMapper mapper, DataContext context)
        {
            this.mapper = mapper;
            this.context = context;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = mapper.Map<Character>(newCharacter);

            //add character
            context.Characters.Add(character);
            await context.SaveChangesAsync();


            serviceResponse.Data =
                await context.Characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int Id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            { 
                var character = await context.Characters.FirstOrDefaultAsync(c => c.Id == Id);
                if (character is null)
                    throw new Exception($"Character with Id: {Id} not found");
                context.Characters.Remove(character);

                await context.SaveChangesAsync();
                serviceResponse.Data 
                    = await context.Characters.Select(c => mapper.Map<GetCharacterDto>(c)).ToListAsync();

            }
            catch (Exception ex)
            {
                serviceResponse.Succes = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> GetAllCharacters()
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();

            // get data from database
            var dbCharacters = await context.Characters.ToListAsync();

            serviceResponse.Data = dbCharacters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await context.Characters.FirstOrDefaultAsync(c => c.Id == id);
            serviceResponse.Data = mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character = 
                    await context.Characters.FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

                if (character is null)
                {
                    throw new Exception($"Character with Id: {updatedCharacter.Id} not found");
                }

                character.Name = updatedCharacter.Name;
                character.HitPoints = updatedCharacter.HitPoints;
                character.Strenght = updatedCharacter.Strenght;
                character.Defense = updatedCharacter.Defense;
                character.Class = updatedCharacter.Class;
                character.Intelligence = updatedCharacter.Intelligence;

                serviceResponse.Data = mapper.Map<GetCharacterDto>(character);
                await context.SaveChangesAsync();
                return serviceResponse;
            }
            catch (Exception ex)
            {
                serviceResponse.Succes = false;
                serviceResponse.Message = ex.Message;
            }
            return serviceResponse;
        }
    }
}
