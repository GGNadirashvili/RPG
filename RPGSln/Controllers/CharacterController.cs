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

        public ActionResult<List<Character>> Get()
        {
            return Ok(characterService.GetAllCharacters());
        }

        // Get Character  by Id

        [HttpGet("{id}")]
        public ActionResult<List<Character>> GetSingleCharacter(int id)
        {
            return Ok(characterService.GetCharacterById(id));
        }

        // Create character

        [HttpPost]
        public ActionResult<List<Character>> CreateCharacter(Character newCharacter)
        {
            return Ok(characterService.AddCharacter(newCharacter));
        }
    }
}
