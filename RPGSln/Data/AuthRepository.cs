using Microsoft.EntityFrameworkCore;
using RPGSln.Models;

namespace RPGSln.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;

        public AuthRepository(DataContext context)
        {
            this.context = context;
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
                response.Data = user.Id.ToString();
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
    }
}
