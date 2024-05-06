using System.Security.Cryptography;
using System.Text;

namespace Forum.Domain.Authentication;

internal class PasswordManager : IPasswordManager
{
    private const int SaltLength = 100;
    private readonly Lazy<SHA256> _sha256 = new(SHA256.Create());

    public (byte[] salt, byte[] hash) GeneratePasswordParts(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(SaltLength);
        var hash = ComputeSha(password, salt);
        return (salt, hash.ToArray());
    }

    public bool ComparePasswords(string password, byte[] salt, byte[] hash) => ComputeSha(password, salt).SequenceEqual(hash);

    private ReadOnlySpan<byte> ComputeSha(string plainText, byte[] salt)
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);

        var buffer = new byte[plainTextBytes.Length + salt.Length];
        Array.Copy(plainTextBytes, buffer, plainTextBytes.Length);
        Array.Copy(salt, 0, buffer, plainTextBytes.Length, salt.Length);

        lock (_sha256)
        {
            return _sha256.Value.ComputeHash(buffer);
        }
    }
}