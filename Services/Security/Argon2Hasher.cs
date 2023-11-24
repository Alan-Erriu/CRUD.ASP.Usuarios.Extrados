using Konscious.Security.Cryptography;
using Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

public class Argon2Hasher : IHashService
{
    public string HashPasswordUser(string password)
    {
        // Configura los parámetros del algoritmo Argon2id
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password))
        {
            Salt = GenerateSalt(),
            DegreeOfParallelism = 8, // Número de hilos de procesamiento 
            Iterations = 4,          // Número de iteraciones de hash
            MemorySize = 65536       // Tamaño de memoria en kilobytes
        };

        // Realiza el hash
        byte[] hashBytes = argon2.GetBytes(32); // Tamaño del hash de salida en bytes

        // Convierte el hash a una cadena hexadecimal
        string hashedPassword = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();

        return hashedPassword;
    }

    private static byte[] GenerateSalt()
    {
        // Genera un nuevo valor de sal aleatorio
        byte[] salt = new byte[16]; // Tamaño de la sal en bytes
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }
        return salt;
    }

    public bool VerifyPassword(string hashedPassword, string inputPassword)
    {
        // Convierte la cadena hexadecimal del hash almacenado a un arreglo de bytes
        byte[] storedHash = Enumerable.Range(0, hashedPassword.Length)
                         .Where(x => x % 2 == 0)
                         .Select(x => Convert.ToByte(hashedPassword.Substring(x, 2), 16))
                         .ToArray();

        // Configura Argon2 con el hash almacenado y la contraseña de entrada
        var argon2 = new Argon2id(Encoding.UTF8.GetBytes(inputPassword))
        {
            Salt = storedHash.Take(16).ToArray(), // Extrae los primeros 16 bytes como sal
            DegreeOfParallelism = 8,
            Iterations = 4,
            MemorySize = 65536
        };

        // Realiza la verificación del hash
        byte[] hashBytes = argon2.GetBytes(32);

        // Compara el hash generado con el almacenado
        return storedHash.Skip(16).SequenceEqual(hashBytes);
    }


}
