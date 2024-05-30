using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class BruteForceCracker
{
    private static readonly string salt = "fixedSaltValue";
    private string hashToCrack;

    public BruteForceCracker(string hash)
    {
        hashToCrack = hash;
    }

    public string BruteForcePassword(int maxThreadCount)
    {
        var charSet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        string password = null;
        bool found = false;

        Parallel.For(1, 5, new ParallelOptions { MaxDegreeOfParallelism = maxThreadCount }, (length, state) =>
        {
            var guess = new char[length];
            if (BruteForceRecursive(guess, charSet, 0, length, ref password, ref found, state))
            {
                state.Stop();
            }
        });

        return password;
    }

    private bool BruteForceRecursive(char[] guess, string charSet, int index, int length, ref string password, ref bool found, ParallelLoopState state)
    {
        if (index == length)
        {
            string guessString = new string(guess);
            if (VerifyPassword(guessString))
            {
                password = guessString;
                found = true;
                return true;
            }
            return false;
        }

        foreach (var c in charSet)
        {
            guess[index] = c;
            if (BruteForceRecursive(guess, charSet, index + 1, length, ref password, ref found, state))
            {
                return true;
            }
        }
        return false;
    }

    private bool VerifyPassword(string password)
    {
        using (var sha256 = SHA256.Create())
        {
            var saltedPassword = password + salt;
            var saltedPasswordBytes = Encoding.UTF8.GetBytes(saltedPassword);
            var hashBytes = sha256.ComputeHash(saltedPasswordBytes);
            var hashString = Convert.ToBase64String(hashBytes);
            return hashString == hashToCrack;
        }
    }
}


