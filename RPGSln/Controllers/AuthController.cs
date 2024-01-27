using Microsoft.AspNetCore.Mvc;
using RPGSln.Data;
using RPGSln.Dtos.User;
using RPGSln.Models;

namespace RPGSln.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthRepository authRepo;

        public AuthController(IAuthRepository authRepo)
        {
            this.authRepo = authRepo;
        }

        [HttpPost("Register")]

        public async Task<ActionResult<ServiceResponse<int>>> Register(UserRegisterDto request)
        {
            var response = await authRepo.Register(
                new User { UserName = request.Username }, request.Password
                );
            if (!response.Succes)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }

        [HttpPost("Login")]

        public async Task<ActionResult<ServiceResponse<int>>> Login(UserLoginDto request)
        {
            var response = await authRepo.Login(request.Username, request.Password);
            if (!response.Succes)
            {
                return BadRequest(response);
            }
            return Ok(response);
        }
    }

}
