using System.Security.Cryptography;
using System.Text;
using static System.Convert;

namespace cinema.api.Helpers;

public static class SalterAndHasher
{
    public static (string, string) getSaltAndSaltedHashedPassword(string password)
    {
        RandomNumberGenerator rng = RandomNumberGenerator.Create();
        byte[] saltBytes = new byte[16];
        rng.GetBytes(saltBytes);
        string saltText = ToBase64String(saltBytes);
        string saltedHashedPassword = SaltAndHashPassword(password, saltText);

        return (saltText, saltedHashedPassword);
    }

    public static bool CheckPassword(string password, string salt, string hashedPassword)
    {
        string saltedHashedPassword = SaltAndHashPassword(password, salt);

        return (saltedHashedPassword == hashedPassword);
    }

    private static string SaltAndHashPassword(string password, string salt)
    {
        using (SHA256 sha = SHA256.Create())
        {
            string saltedPassword = password + salt;
            return ToBase64String(sha.ComputeHash(Encoding.Unicode.GetBytes(saltedPassword)));
        }
    }
}
