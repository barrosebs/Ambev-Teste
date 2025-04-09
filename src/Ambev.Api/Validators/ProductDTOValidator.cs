using FluentValidation;
using Ambev.Application.DTOs;

namespace Ambev.Api.Validators
{
    public class ProductDTOValidator : AbstractValidator<ProductDTO>
    {
        public ProductDTOValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("O título é obrigatório")
                .MaximumLength(100).WithMessage("O título deve ter no máximo 100 caracteres");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("O preço deve ser maior que zero");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("A descrição é obrigatória")
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("A categoria é obrigatória")
                .MaximumLength(50).WithMessage("A categoria deve ter no máximo 50 caracteres");

            RuleFor(x => x.Image)
                .NotEmpty().WithMessage("A URL da imagem é obrigatória")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _)).WithMessage("A URL da imagem deve ser válida");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("O status é obrigatório")
                .MaximumLength(20).WithMessage("O status deve ter no máximo 20 caracteres");

            RuleFor(x => x.Rating)
                .NotNull().WithMessage("O rating é obrigatório");

            RuleFor(x => x.Rating.Rate)
                .InclusiveBetween(0, 5).WithMessage("A taxa deve estar entre 0 e 5");

            RuleFor(x => x.Rating.Count)
                .GreaterThanOrEqualTo(0).WithMessage("A contagem deve ser maior ou igual a zero");
        }
    }
} 