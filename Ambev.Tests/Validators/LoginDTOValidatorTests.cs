using Xunit;
using FluentValidation.TestHelper;
using Ambev.API.Validators;
using Ambev.Domain.DTOs;

namespace Ambev.Tests.Validators
{
    public class LoginDTOValidatorTests
    {
        private readonly LoginDTOValidator _validator;

        public LoginDTOValidatorTests()
        {
            _validator = new LoginDTOValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Username_Is_Empty()
        {
            var model = new LoginDTO { Username = string.Empty,  };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Should_Have_Error_When_Username_Is_Too_Long()
        {
            var model = new LoginDTO { Username = new string('a', 51) };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Empty()
        {
            var model = new LoginDTO { Password = string.Empty };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Too_Short()
        {
            var model = new LoginDTO { Password = "12345" };
            var result = _validator.TestValidate(model);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            var model = new LoginDTO 
            {
                Username = "johndoe",
                Password = "password123"
            };
            var result = _validator.TestValidate(model);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
} 