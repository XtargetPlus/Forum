namespace Forum.Domain.Authentication;

public interface IPasswordManager
{
    bool ComparePasswords(string password, byte[] salt, byte[] hash);
    (byte[] salt, byte[] hash) GeneratePasswordParts(string password);
}