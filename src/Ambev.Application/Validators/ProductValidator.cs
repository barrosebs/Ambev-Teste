using FluentValidation;
using Ambev.Application.DTOs;

namespace Ambev.Application.Validators
{
    public class ProductValidator : AbstractValidator<ProductDTO>
    {
        public ProductValidator()
        {
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("O título é obrigatório")
                .MaximumLength(100).WithMessage("O título deve ter no máximo 100 caracteres");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("O preço deve ser maior que zero")
                .LessThan(1000000).WithMessage("O preço deve ser menor que 1.000.000");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("A descrição é obrigatória")
                .MaximumLength(500).WithMessage("A descrição deve ter no máximo 500 caracteres");

            RuleFor(x => x.Category)
                .NotEmpty().WithMessage("A categoria é obrigatória")
                .MaximumLength(50).WithMessage("A categoria deve ter no máximo 50 caracteres");

            RuleFor(x => x.Image)
                .NotEmpty().WithMessage("A URL da imagem é obrigatória")
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _))
                .When(x => !string.IsNullOrEmpty(x.Image))
                .WithMessage("A URL da imagem deve ser válida");

            RuleFor(x => x.Rating)
                .NotNull().WithMessage("O rating é obrigatório");

            RuleFor(x => x.Rating.Rate)
                .InclusiveBetween(0, 5).WithMessage("A taxa deve estar entre 0 e 5");

            RuleFor(x => x.Rating.Count)
                .GreaterThanOrEqualTo(0).WithMessage("A contagem deve ser maior ou igual a zero");
        }
    }
} 