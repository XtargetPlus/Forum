using FluentAssertions;
using Forum.Domain.UseCases.CreateTopic;

namespace Forum.Domain.Tests.CreateTopic;

public class CreateTopicCommandValidatorShould
{
    private readonly CreateTopicCommandValidator _sut = new();

    [Fact]
    public void ReturnSuccess_WhenCommandIsValid()
    {
        var actual = _sut.Validate(new CreateTopicCommand(Guid.Parse("9ce52359-fbe0-40bf-ab13-b5dcd073d365"), "Hello world"));
        actual.IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new CreateTopicCommand(Guid.Parse("9ce52359-fbe0-40bf-ab13-b5dcd073d365"), "Hello world");
        yield return new object[] { validCommand with { ForumId = Guid.Empty } };
        yield return new object[] { validCommand with { Title = string.Empty } };
        yield return new object[] { validCommand with { Title = "    " } };
        yield return new object[] { validCommand with { Title = "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Nunc eget leo ac lectus ultricies ullamcorper. Etiam lobortis, augue faucibus tristique." } };
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public void ReturnSuccess_WhenCommandIsInvalid(CreateTopicCommand command)
    {
        var actual = _sut.Validate(command);
        actual.IsValid.Should().BeFalse();
    }
}