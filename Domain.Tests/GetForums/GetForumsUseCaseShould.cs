using Domain.Dtos;
using Domain.Monitoring;
using Domain.UseCases.GetForums;
using FluentAssertions;
using Moq;
using Moq.Language.Flow;

namespace Domain.Tests.GetForums;

public class GetForumsUseCaseShould
{
    private readonly GetForumsUseCase _sut;
    private readonly ISetup<IGetForumsStorage, Task<IEnumerable<ForumDto>>> _storageSetup;
    private readonly Mock<IGetForumsStorage> _storage;

    public GetForumsUseCaseShould()
    {
        _storage = new Mock<IGetForumsStorage>();
        _storageSetup = _storage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

        _sut = new GetForumsUseCase(_storage.Object, new DomainMetrics());
    }

    [Fact]
    public async Task ReturnForums_FromStorage()
    {
        var forums = new ForumDto[]
        {
            new() { ForumId = Guid.Parse("98cb1896-b105-4022-b4ca-527034313e4a"), Title = "Qwerty1" },
            new() { ForumId = Guid.Parse("9983ebe4-08f5-410a-8d1f-cca7e6311821"), Title = "Qwerty2" },
        };
        _storageSetup.ReturnsAsync(forums);

        var actual = await _sut.Execute(CancellationToken.None);
        actual.Should().BeSameAs(forums);
        _storage.Verify(s => s.GetForums(CancellationToken.None), Times.Once);
        _storage.VerifyNoOtherCalls();
    }
}