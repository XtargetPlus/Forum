namespace Domain.Authentication;

public interface ISymmetricDecryptor
{
    Task<string> Decrypt(string encryptedText, byte[] key, CancellationToken cancellationToken);
}