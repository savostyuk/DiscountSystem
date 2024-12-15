using DiscountSystem.Application.Common;
using DiscountSystem.Application.Users.Commands;
using DiscountSystem.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace DiscountSystem.Application.Tests.Users.Commands;

[TestFixture]
public class CreateUserCommandHandlerTests
{
    private IApplicationDbContext _dbContext;
    private CreateUserCommandHandler _commandHandler;

    [SetUp]
    public void Setup()
    {
        _dbContext = Substitute.For<IApplicationDbContext>();
        _commandHandler = new CreateUserCommandHandler(_dbContext);
    }

    [Test]
    public async Task Handle_ShouldCreateUser_WhenDataIsValid()
    {
        //Arrange
        var command = new CreateUserCommand()
        {
            FirstName = "Natalya",
            LastName = "Shulzhenka",
            Email = "natalya@test.com"
        };

        //Act
        var result = await _commandHandler.Handle(command, CancellationToken.None);

        //Assert
        result.Should().Be(Guid.Empty);

        _dbContext.Users.Received(1).Add(Arg.Is<User> (user =>
            user.FirstName == command.FirstName &&
            user.LastName ==command.LastName &&
            user.Email == command.Email));
        await _dbContext.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
