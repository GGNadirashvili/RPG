using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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

        public async Task<ActionResult<ServiceResponse<List<Character>>>> Get()
        {
            return Ok(await characterService.GetAllCharacters());
        }

        // Get Character  by Id

        [HttpGet("{id}")]
        public async Task<ActionResult<ServiceResponse<List<Character>>>> GetSingleCharacter(int id)
        {
            return Ok(await characterService.GetCharacterById(id));
        }
         
        // Create character

        [HttpPost]
        public async  Task<ActionResult<ServiceResponse<List<Character>>>> AddCharacter(Character newCharacter)
        {
            return Ok(await characterService.AddCharacter(newCharacter));
        }
    }
}
