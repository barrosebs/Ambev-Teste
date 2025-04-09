using FluentValidation;
using Ambev.Application.DTOs;

namespace Ambev.Api.Validators
{
    public class CartDTOValidator : AbstractValidator<CartDTO>
    {
        public CartDTOValidator()
        {
            RuleFor(x => x.UserId)
                .NotEmpty().WithMessage("O ID do usuário é obrigatório");

            RuleFor(x => x.Items)
                .NotEmpty().WithMessage("O carrinho deve ter pelo menos um item");

            RuleForEach(x => x.Items)
                .SetValidator(new CartItemDTOValidator());
        }
    }

    public class CartItemDTOValidator : AbstractValidator<CartItemDTO>
    {
        public CartItemDTOValidator()
        {
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("O ID do produto é obrigatório");

            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("A quantidade deve ser maior que zero");
        }
    }
} 