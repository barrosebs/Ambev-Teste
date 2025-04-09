using FluentValidation;
using Ambev.Application.DTOs;

namespace Ambev.Application.Validators
{
    public class CartValidator : AbstractValidator<CartDTO>
    {
        public CartValidator()
        {
            RuleFor(x => x.UserId)
                .GreaterThan(0).WithMessage("O ID do usuário deve ser maior que zero");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("O carrinho deve ter pelo menos um item")
                .Must(items => items.Count <= 100).WithMessage("O carrinho não pode ter mais de 100 itens");

            RuleForEach(x => x.Items)
                .SetValidator(new CartItemValidator());
        }
    }

    public class CartItemValidator : AbstractValidator<CartItemDTO>
    {
        public CartItemValidator()
        {
            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("O ID do produto deve ser maior que zero");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero")
                .LessThanOrEqualTo(100).WithMessage("A quantidade não pode ser maior que 100");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("O preço deve ser maior que zero");
        }
    }
} 