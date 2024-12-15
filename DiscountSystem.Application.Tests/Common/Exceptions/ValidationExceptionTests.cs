using FluentAssertions;
using FluentValidation.Results;
using ValidationException = DiscountSystem.Application.Common.Exceptions.ValidationException;

namespace DiscountSystem.Application.Tests.Common.Exceptions;

[TestFixture]
public class ValidationExceptionTests
{
    [Test]
    public void ValidationException_ShouldContainEmptyErrors()
    {
        //Act
        var exception = new ValidationException();

        //Assert
        exception.Errors.Should().NotBeNull();
        exception.Errors.Should().BeEmpty();
        exception.Message.Should().Be("One or more validation errors have occured. See the list below.");
    }

    [Test]
    public void ValidationException_ShouldContainFormattedErrorDictionary() 
    {
        //Arrange
        var failures = new[]
        {
            new ValidationFailure("Property1", "Error1"),
            new ValidationFailure("Property2", "Error2")
        };

        //Act
        var exception = new ValidationException(failures);

        //Assert
        exception.Errors.Should().NotBeNull();
        exception.Errors.Should().ContainKey("Property1");
        exception.Errors["Property1"].Should().Contain("Error1");
    }
}
