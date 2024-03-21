using System.Security.Cryptography;
using Domain.Authentication;
using FluentAssertions;

namespace Domain.Tests.Authentication;

public class AesSymmetricEncryptorDecryptorShould
{
    private readonly AesSymmetricEncryptorDecryptor _sut = new();

    [Fact]
    public async Task ReturnMeaningfulEncryptorString()
    {
        var key = RandomNumberGenerator.GetBytes(32);
        var plainText = "qwerty";
        var actual = await _sut.Encrypt(plainText, key, CancellationToken.None);

        actual.Should().NotBeEmpty();
    }

    [Fact]
    public async Task DecryptEncryptedString_WhenKeyItSame()
    {
        var key = RandomNumberGenerator.GetBytes(32);
        var plainText = "qwerty";
        var encrypted = await _sut.Encrypt(plainText, key, CancellationToken.None);
        var decrypted = await _sut.Decrypt(encrypted, key, CancellationToken.None);

        decrypted.Should().Be(plainText);
    }

    [Fact]
    public async Task ThrowException_WhenDecryptingWithDifferentKey()
    {
        var key = RandomNumberGenerator.GetBytes(32);
        var invalidKey = RandomNumberGenerator.GetBytes(32);
        var plainText = "qwerty";
        var encrypted = await _sut.Encrypt(plainText, key, CancellationToken.None);
        await _sut.Invoking(s => s.Decrypt(encrypted, invalidKey, CancellationToken.None)).Should().ThrowAsync<CryptographicException>();
    }

    //[Fact]
    //public async Task GiveMeBase64Key()
    //{
    //    _outputHelper.WriteLine(Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)));
    //}
}
