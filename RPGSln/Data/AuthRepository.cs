using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RPGSln.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace RPGSln.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
        private readonly IConfiguration configuration;

        public AuthRepository(DataContext context, IConfiguration configuration)
        {
            this.context = context;
            this.configuration = configuration;
        }
        public async Task<ServiceResponse<string>> Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            var user = await context.Users
                .FirstOrDefaultAsync(u => u.UserName.ToLower().Equals(username.ToLower()));
            if (user is null)
            {
                response.Succes = false;
                response.Message = "User not found.";
            }
            else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswrodSalt))
            {
                response.Succes = false;
                response.Message = "Wrong Password.";
            }
            else
            {
                response.Data = CreateToken(user);
            }
            return response;
        }

        public async Task<ServiceResponse<int>> Register(User user, string password)
        {
            var response = new ServiceResponse<int>();
            if (await UserExists(user.UserName))
            {
                response.Succes = false;
                response.Message = "User already exists.";
                return response;
            }

            CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);

            user.PasswordHash = passwordHash;
            user.PasswrodSalt = passwordSalt;

            context.Users.Add(user);
            await context.SaveChangesAsync();
            response.Data = user.Id;
            return response;
        }

        public async Task<bool> UserExists(string username)
        {
            if (await context.Users.AnyAsync(u => u.UserName.ToLower() == username.ToLower()))
            {
                return true;
            }
            return false;
        }

        // Hash Password
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        // Verify Password
        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        // Implement JWT 

        private string CreateToken(User user)
        {
            // Creating a list to hold claims about the user
            var claims = new List<Claim>
             {
        // Adding a claim for the user's unique identifier
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        
        // Adding a claim for the user's username
        new Claim(ClaimTypes.Name, user.UserName)
               };

            // Retrieving the secret key used to sign the token from app settings
            var appSettingsToken = configuration.GetSection("AppSettings:Token").Value;
            if (appSettingsToken is null)
                throw new Exception("AppSettings Token is null");

            // Creating a symmetric security key from the secret key
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8
                .GetBytes(appSettingsToken));

            // Creating signing credentials using the key and HMAC-SHA512 algorithm
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            // Creating a token descriptor with the claims, expiration time, and signing credentials
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            // Creating a JWT token handler
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            // Creating a JWT token based on the token descriptor
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

            // Writing the JWT token to a string
            return tokenHandler.WriteToken(token);
        }

    }
}
