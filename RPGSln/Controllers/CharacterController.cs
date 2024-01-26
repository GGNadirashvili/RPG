using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPGSln.Dtos.Character;
using RPGSln.Models;
using RPGSln.Services.CharacterService;

namespace RPGSln.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private readonly ICharacterService characterService;

        // Create Constructor
        public CharacterController(ICharacterService characterService)
        {
            this.characterService = characterService;
        }


        // Get All Character

        [HttpGet("GetAll")]

        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> Get()
        {
            return Ok(await characterService.GetAllCharacters());
        }

        // Get Character  by Id

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> GetSingleCharacter(int id)
        {
            return Ok(await characterService.GetCharacterById(id));
        }
         
        // Create character

        [HttpPost]
        public async  Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> AddCharacter(AddCharacterDto newCharacter)
        {
            return Ok(await characterService.AddCharacter(newCharacter));
        }  
        
        [HttpPut]
        public async  Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> UpdateCharacter(UpdateCharacterDto updatedCharacter)
        {
            var response = await characterService.UpdateCharacter(updatedCharacter);
            if(response.Data is null)
            {
                return NotFound();
            }
            return Ok(response);
        }

        [HttpDelete("{id}")] 
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDto>>>> DeleteCharacter(int id)
        {
            var response = await characterService.DeleteCharacter(id);
            if (response.Data is null)
            {
                return NotFound();
            }
            return Ok(response);
        }

    }

}
