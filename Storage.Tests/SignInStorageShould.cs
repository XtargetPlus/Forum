using Domain.Dtos;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Storage.Models;
using Storage.Storages;

namespace Storage.Tests;

public class SignInStorageFixture : StorageTextFixture
{
    public Guid UserId { get; private set; }

    public override async Task InitializeAsync()
    {
        await base.InitializeAsync();

        UserId = Guid.Parse("c9d9402e-391e-4ee2-82eb-2101dd864990");

        await using var dbContext = GetDbContext();
        await dbContext.Users.AddRangeAsync(
            new User
            {
                Login = "Test 1",
                Id = UserId,
                Salt = new byte[] { 1 },
                PasswordHash = new byte[] { 2 },
            },
            new User
            {
                Login = "Test 2",
                Id = Guid.Parse("d4767603-b990-474d-ae55-5af7e19a9ec7"),
                Salt = new byte[] { 1 },
                PasswordHash = new byte[] { 2 },
            });
        await dbContext.SaveChangesAsync();
    }
}

public class SignInStorageShould : IClassFixture<SignInStorageFixture>
{
    private readonly SignInStorageFixture _fixture;
    private readonly SignInStorage _sut;

    public SignInStorageShould(SignInStorageFixture fixture)
    {
        _fixture = fixture;

        _sut = new SignInStorage(_fixture.GetMapper(), _fixture.GetDbContext());
    }

    [Fact]
    public async Task ReturnUser_WhenDatabaseContainsUserWithSameLogin()
    {
        var actual = await _sut.FindUser("Test 1", CancellationToken.None);
        actual.Should().NotBeNull()
            .And.Subject.As<RecognizedUser>().UserId.Should().Be(_fixture.UserId);
    }

    [Fact]
    public async Task ReturnNull_WhenDatabaseDoesNotContainsUserWithSameLogin()
    {
        var actual = await _sut.FindUser("Invalid login", CancellationToken.None);
        actual.Should().BeNull();
    }

    [Fact]
    public async Task ReturnNewlyCreatedSessionId()
    {
        var sessionId = await _sut.CreateSession(
            _fixture.UserId, 
            new DateTimeOffset(2024, 2, 28, 1, 37, 0, 0, TimeSpan.Zero),
            CancellationToken.None);

        await using var dbContext = _fixture.GetDbContext();
        (await dbContext.Sessions
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == sessionId)).Should().NotBeNull();
    }
}