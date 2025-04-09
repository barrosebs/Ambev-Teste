using FluentValidation;
using Ambev.Application.DTOs;
using Ambev.Domain.DTOs;

namespace Ambev.API.Validators
{
    public class LoginDTOValidator : AbstractValidator<LoginDTO>
    {
        public LoginDTOValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("O nome de usuário é obrigatório")
                .MaximumLength(50).WithMessage("O nome de usuário deve ter no máximo 50 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres");
        }
    }
} 