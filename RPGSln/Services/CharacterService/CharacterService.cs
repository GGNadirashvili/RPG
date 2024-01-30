using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RPGSln.Data;
using RPGSln.Dtos.Character;
using RPGSln.Dtos.Skill;
using RPGSln.Models;
using System.Security.Claims;

namespace RPGSln.Services.CharacterService
{
    public class CharacterService : ICharacterService
    {

        private readonly IMapper mapper;
        private readonly DataContext context;
        private readonly IHttpContextAccessor httpContextAccesor;

        public CharacterService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccesor)
        {
            this.mapper = mapper;
            this.context = context;
            this.httpContextAccesor = httpContextAccesor;
        }

        private int GetUserId() => int.Parse(httpContextAccesor.HttpContext!.User.
            FindFirstValue(ClaimTypes.NameIdentifier)!);

        public async Task<ServiceResponse<List<GetCharacterDto>>> AddCharacter(AddCharacterDto newCharacter)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            var character = mapper.Map<Character>(newCharacter);

            character.User = await context.Users.FirstOrDefaultAsync(u => u.Id == GetUserId());

            //add character
            context.Characters.Add(character);
            await context.SaveChangesAsync();


            serviceResponse.Data =
                await context.Characters
                .Where(c => c.User!.Id == GetUserId())
                .Select(c => mapper.Map<GetCharacterDto>(c))
                .ToListAsync();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetCharacterDto>>> DeleteCharacter(int Id)
        {
            var serviceResponse = new ServiceResponse<List<GetCharacterDto>>();
            try
            {
                var character = await context.Characters
                    .FirstOrDefaultAsync(c => c.Id == Id && c.User!.Id == GetUserId());
                if (character is null)
                    throw new Exception($"Character with Id: {Id} not found");
                context.Characters.Remove(character);

                await context.SaveChangesAsync();

                serviceResponse.Data
                    = await context.Characters
                    .Where(c => c.User!.Id == GetUserId())
                    .Select(c => mapper.Map<GetCharacterDto>(c)).ToListAsync();

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
            var dbCharacters = await context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .Where(c => c.User!.Id == GetUserId()).ToListAsync();

            serviceResponse.Data = dbCharacters.Select(c => mapper.Map<GetCharacterDto>(c)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> GetCharacterById(int id)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();
            var dbCharacter = await context.Characters
                .Include(c => c.Weapon)
                .Include(c => c.Skills)
                .FirstOrDefaultAsync(c => c.Id == id && c.User!.Id == GetUserId());
            serviceResponse.Data = mapper.Map<GetCharacterDto>(dbCharacter);
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetCharacterDto>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var serviceResponse = new ServiceResponse<GetCharacterDto>();

            try
            {
                var character =
                    await context.Characters
                    .Include(c => c.User)
                    .FirstOrDefaultAsync(c => c.Id == updatedCharacter.Id);

                if (character is null || character.User!.Id != GetUserId())
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

        public async Task<ServiceResponse<GetCharacterDto>> AddCharacterSkill(AddCharacterSkillDto newCharacterSkill)
        {
            var response = new ServiceResponse<GetCharacterDto>();
            try
            {
                var character = await context.Characters
                    .Include(c => c.Weapon)
                    .Include(c => c.Skills)
                    .FirstOrDefaultAsync(c => c.Id == newCharacterSkill.CharacterId &&
                    c.User!.Id == GetUserId());

                if (character is null)
                {
                    response.Succes = false;
                    response.Message = "Character not found";
                    return response;
                }

                var skill = await context.Skills
                    .FirstOrDefaultAsync(s => s.Id == newCharacterSkill.SkillId);
                if (skill is null)
                {
                    response.Succes = false;
                    response.Message = "Skill not found";
                    return response;
                }

                character.Skills!.Add(skill);
                await context.SaveChangesAsync();
                response.Data = mapper.Map<GetCharacterDto>(character);
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
