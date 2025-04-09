using FluentValidation.TestHelper;
using Ambev.Api.Validators;
using Ambev.Application.DTOs;

namespace Ambev.Tests.Validators
{
    public class ProductDTOValidatorTests
    {
        private readonly ProductDTOValidator _validator;
        private readonly ProductDTO _modelFake = new ProductDTO
        {
            Title = "Product Title",
            Price = 99,
            Description = "Product Description",
            Category = "Category",
            Image = "https://images/image01.png",
            Rating = new ProductRatingDTO
            {
                Rate = 4.5m,
                Count = 100
            }
        };

        public ProductDTOValidatorTests()
        {
            _validator = new ProductDTOValidator();
        }

        [Fact]
        public void Should_Have_Error_When_Title_Is_Empty()
        {
            _modelFake.Title = string.Empty;
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Title);
        }

        [Fact]
        public void Should_Have_Error_When_Price_Is_Negative()
        {
            _modelFake.Price = -1;
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Price);
        }

        [Fact]
        public void Should_Have_Error_When_Description_Is_Empty()
        {
            _modelFake.Description = string.Empty;
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Description);
        }

        [Fact]
        public void Should_Have_Error_When_Category_Is_Empty()
        {
            _modelFake.Category = string.Empty;
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Category);
        }

        [Fact]
        public void Should_Have_Error_When_Image_Is_Empty()
        {
            _modelFake.Image = string.Empty;
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Image);
        }

        [Fact]
        public void Should_Have_Error_When_Rating_Is_Null()
        {
            _modelFake.Rating = null;
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Rating);
        }

        [Fact]
        public void Should_Have_Error_When_Rate_Is_Invalid()
        {
            _modelFake.Rating = new ProductRatingDTO
            {
                Rate = -1,
                Count = 100
            };
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Rating.Rate);
        }

        [Fact]
        public void Should_Have_Error_When_Count_Is_Negative()
        {
            _modelFake.Rating = new ProductRatingDTO
            {
                Rate = 4.5m,
                Count = -1
            };
            var result = _validator.TestValidate(_modelFake);
            result.ShouldHaveValidationErrorFor(x => x.Rating.Count);
        }

        [Fact]
        public void Should_Not_Have_Error_When_All_Fields_Are_Valid()
        {
            var result = _validator.TestValidate(_modelFake);
            result.ShouldNotHaveAnyValidationErrors();
        }
    }
} 