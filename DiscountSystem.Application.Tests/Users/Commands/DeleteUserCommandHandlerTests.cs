using DiscountSystem.Application.Common;
using DiscountSystem.Application.Users.Commands;
using DiscountSystem.Domain.Entities;
using NSubstitute;

namespace DiscountSystem.Application.Tests.Users.Commands;

[TestFixture]
public class DeleteUserCommandHandlerTests
{
    private IApplicationDbContext _dbContext;
    private DeleteUserCommandHandler _commandHandler;

    [SetUp]
    public void Setup()
    {
        _dbContext = Substitute.For<IApplicationDbContext>();
        _commandHandler = new DeleteUserCommandHandler(_dbContext);
    }

    [Test]
    public async Task ShouldDeleteUser_WhenUserExists()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var existingUser = new User
        {
            Id = userId,
            FirstName = "FirstName",
            LastName = "LastName",
            Email = "email@test.com"
        };

        _dbContext.Users.FindAsync(Arg.Is<object[]>(ids => (Guid)ids[0] == userId), Arg.Any<CancellationToken>())
            .Returns(existingUser);

        var command = new DeleteUserCommand(userId);

        //Act
        await _commandHandler.Handle(command, CancellationToken.None);

        //Assert
        _dbContext.Users.Received(1).Remove(existingUser);
        await _dbContext.Received(1).SaveChangesAsync(CancellationToken.None);
    }
}
