namespace Services.Interfaces
{
    public interface IHashService
    {
        string HashPasswordUser(string password);
        bool VerifyPassword(string inputPassword, string hashedPassword);
    }
}