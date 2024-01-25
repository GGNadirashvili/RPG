using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RPGSln.Models;

namespace RPGSln.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CharacterController : ControllerBase
    {
        private static List<Character> characters = new List<Character> {
        new Character(),
        new Character{Id = 1, Name = "Sam"}
        };

        // implement get method

        [HttpGet("GetAll")]

        public ActionResult<List<Character>> Get()
        {
            return Ok(characters);
        }


        [HttpGet("{id}")]
        public ActionResult<List<Character>> GetSingleCharacter(int id)
        {
            return Ok(characters.FirstOrDefault(c => c.Id == id));
        }
    }
}
