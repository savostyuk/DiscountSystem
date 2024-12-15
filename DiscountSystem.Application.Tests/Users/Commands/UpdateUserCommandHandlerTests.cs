using DiscountSystem.Application.Common;
using DiscountSystem.Application.Users.Commands;
using DiscountSystem.Domain.Entities;
using FluentAssertions;
using NSubstitute;

namespace DiscountSystem.Application.Tests.Users.Commands;

[TestFixture]
public class UpdateUserCommandHandlerTests
{
    private IApplicationDbContext _dbContext;
    private UpdateUserCommandHandler _commandHandler;

    [SetUp]
    public void Setup()
    {
        _dbContext = Substitute.For<IApplicationDbContext>();
        _commandHandler = new UpdateUserCommandHandler(_dbContext);
    }

    [Test]
    public async Task Handle_ShouldUpdateUser_WhenUserExists()
    {
        //Arrange
        var userId = Guid.NewGuid();
        var existingUser = new User
        {
            Id = userId,
            FirstName = "Anton",
            LastName = "Shulzhenka",
            Email = "anton@test.com"
        };

        _dbContext.Users.FindAsync(Arg.Is<object[]>(ids => (Guid)ids[0] == userId), Arg.Any<CancellationToken>())
            .Returns(existingUser);

        var command = new UpdateUserCommand
        {
            Id = userId,
            FirstName = "New First Name",
            LastName = "New Last Name",
            Email = "newemail@test.com"
        };

        //Act
        await _commandHandler.Handle(command, CancellationToken.None);

        //Assert
        await _dbContext.Received(1).SaveChangesAsync(CancellationToken.None);

        existingUser.FirstName.Should().Be("New First Name");
        existingUser.LastName.Should().Be("New Last Name");
        existingUser.Email.Should().Be("newemail@test.com");
    }

    [Test]
    public async Task Handle_ShouldThrowException_WhenUserDoesNotExist()
    {
        //Arrange
        var nonExistentUserId = Guid.NewGuid();
        _dbContext.Users.FindAsync(Arg.Is<object[]>(ids => (Guid)ids[0] == nonExistentUserId), Arg.Any<CancellationToken>())
            .Returns((User)null);

        var command = new UpdateUserCommand
        {
            Id = nonExistentUserId,
            FirstName = "New First Name",
            LastName = "New Last Name",
            Email = "newemail@test.com"
        };

        //Act && Assert
        FluentActions.Invoking(() => _commandHandler.Handle(command, CancellationToken.None))
            .Should().ThrowAsync<Exception>()
            .WithMessage($"Entity with Id = {nonExistentUserId} was not found");
    }
}
