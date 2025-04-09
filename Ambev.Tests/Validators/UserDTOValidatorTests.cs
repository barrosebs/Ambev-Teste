using Xunit;
using FluentValidation.TestHelper;
using Ambev.API.Validators;
using Ambev.Application.DTOs;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Ambev.Tests.Validators
{
    public class UserDTOValidatorTests
    {
        private readonly UserDTOValidator _validator;
        private readonly UserDTO _modelFake = new UserDTO
        {
            Username = "fulano",
            Password = "password123",
            Email = "teste@teste.com.br",
            Name = new UserNameDTO
            {
                Firstname = "Fulano",
                Lastname = "Ciclano"
            },
            Address = new UserAddressDTO
            {
                City = "New York",
                Street = "Main St",
                Number = 123,
                Zipcode = "12345-678",
                Geolocation = new UserGeoLocationDTO
                {
                    Lat = "12354545",
                    Long = "4565555"
                }
            },
            Phone = "(12) 34567-8901",
            Status = "Active",
            Role = "Customer"
        };

        public UserDTOValidatorTests()
        {
            _validator = new UserDTOValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Empty()
        {
            _modelFake.Email = string.Empty;
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Email_Is_Invalid()
        {
            _modelFake.Email = "invalid-email";
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Email);
        }

        [Fact]
        public void Should_Have_Error_When_Username_Is_Empty()
        {
            _modelFake.Username = string.Empty;
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Username);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Empty()
        {
            _modelFake.Password = string.Empty;
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Have_Error_When_Password_Is_Too_Short()
        {
            _modelFake.Password = "12345";
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Password);
        }

        [Fact]
        public void Should_Have_Error_When_Name_Is_Null()
        {
            _modelFake.Name = null!;
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        [Fact]
        public void Should_Have_Error_When_Firstname_Is_Empty()
        {
            _modelFake.Name = new UserNameDTO
            {
                Firstname = string.Empty,
                Lastname = "Doe"
            };
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Name.Firstname);
        }

        [Fact]
        public void Should_Have_Error_When_Lastname_Is_Empty()
        {
            _modelFake.Name = new UserNameDTO
            {
                Firstname = "John",
                Lastname = string.Empty
            };
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Name.Lastname);
        }

        [Fact]
        public void Should_Have_Error_When_Address_Is_Null()
        {
            _modelFake.Address = null!;
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Address);
        }

        [Fact]
        public void Should_Have_Error_When_City_Is_Empty()
        {
            _modelFake.Address = new UserAddressDTO
            {
                City = string.Empty,
                Street = "Main St",
                Number = 123,
                Zipcode = "12345-678",
                Geolocation = new UserGeoLocationDTO
                {
                    Lat = "123",
                    Long = "456"
                }
            };
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Address.City);
        }

        [Fact]
        public void Should_Have_Error_When_Street_Is_Empty()
        {
            _modelFake.Address = new UserAddressDTO
            {
                City = "New York",
                Street = string.Empty,
                Number = 123,
                Zipcode = "12345-678",
                Geolocation = new UserGeoLocationDTO
                {
                    Lat = "123",
                    Long = "456"
                }
            };
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Address.Street);
        }

        [Fact]
        public void Should_Have_Error_When_Number_Is_Zero()
        {
            _modelFake.Address = new UserAddressDTO
            {
                City = "New York",
                Street = "Main St",
                Number = 0,
                Zipcode = "12345-678",
                Geolocation = new UserGeoLocationDTO
                {
                    Lat = "123",
                    Long = "456"
                }
            };
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Address.Number);
        }

        [Fact]
        public void Should_Have_Error_When_Zipcode_Is_Invalid()
        {
            _modelFake.Address = new UserAddressDTO
            {
                City = "New York",
                Street = "Main St",
                Number = 123,
                Zipcode = "12345678",
                Geolocation = new UserGeoLocationDTO
                {
                    Lat = "123",
                    Long = "456"
                }
            };
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Address.Zipcode);
        }

        [Fact]
        public void Should_Have_Error_When_Geolocation_Is_Null()
        {
            _modelFake.Address = new UserAddressDTO
            {
                City = "New York",
                Street = "Main St",
                Number = 123,
                Zipcode = "12345-678",
                Geolocation = null
            };

            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Address.Geolocation);
        }

        [Fact]
        public void Should_Have_Error_When_Lat_Is_Invalid()
        {
            _modelFake.Address = new UserAddressDTO
            {
                City = "New York",
                Street = "Main St",
                Number = 123,
                Zipcode = "12345-678",
                Geolocation = new UserGeoLocationDTO
                {
                    Lat = "-1",
                    Long = "456"
                }
            };
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Address.Geolocation.Lat);
        }

        [Fact]
        public void Should_Have_Error_When_Long_Is_Invalid()
        {
            _modelFake.Address = new UserAddressDTO
            {
                City = "New York",
                Street = "Main St",
                Number = 123,
                Zipcode = "12345-678",
                Geolocation = new UserGeoLocationDTO
                {
                    Lat = "123",
                    Long = "-1"
                }
            };
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Address.Geolocation.Long);
        }

        [Fact]
        public void Should_Have_Error_When_Phone_Is_Invalid()
        {
            _modelFake.Phone = "12345";
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }

        [Fact]
        public void Should_Have_Error_When_Status_Is_Invalid()
        {
            _modelFake.Status = "Invalid";
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Status);
        }

        [Fact]
        public void Should_Have_Error_When_Role_Is_Invalid()
        {
            _modelFake.Role = "Invalid";
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Role);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            var result = _validator.TestValidate(_modelFake);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
}