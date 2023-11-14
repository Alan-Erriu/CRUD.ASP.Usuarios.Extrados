using Services.Interfaces;

namespace Services.Security
{
    public class HashService : IHashService
    {

        public string HashPasswordUser(string password)
        {
            var hash = BCrypt.Net.BCrypt.HashPassword(password);

            return hash;

        }
        public bool VerifyPassword(string inputPassword, string hashedPassword)
        {
            return BCrypt.Net.BCrypt.Verify(inputPassword, hashedPassword);
        }
    }

}
