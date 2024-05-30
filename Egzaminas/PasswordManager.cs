using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

public class PasswordManager
{
    private static readonly string salt = "fixedSaltValue";

    public string CreatePasswordHash(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = password + salt;
            var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
            return Convert.ToBase64String(hashBytes);
        }
    }
}


