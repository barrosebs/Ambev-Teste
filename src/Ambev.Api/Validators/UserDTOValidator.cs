using Ambev.Application.DTOs;
using Ambev.Domain.DTOs;
using FluentValidation;

namespace Ambev.API.Validators
{
    public class UserDTOValidator : AbstractValidator<UserDTO>
    {
        public UserDTOValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("O email é obrigatório")
                .EmailAddress().WithMessage("Email inválido")
                .MaximumLength(100).WithMessage("O email deve ter no máximo 100 caracteres");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("O nome de usuário é obrigatório")
                .MaximumLength(50).WithMessage("O nome de usuário deve ter no máximo 50 caracteres");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("A senha é obrigatória")
                .MinimumLength(6).WithMessage("A senha deve ter no mínimo 6 caracteres");

            RuleFor(x => x.Name)
                .NotNull().WithMessage("O nome é obrigatório");

            RuleFor(x => x.Name.Firstname)
                .NotEmpty().WithMessage("O primeiro nome é obrigatório")
                .MaximumLength(50).WithMessage("O primeiro nome deve ter no máximo 50 caracteres");

            RuleFor(x => x.Name.Lastname)
                .NotEmpty().WithMessage("O sobrenome é obrigatório")
                .MaximumLength(50).WithMessage("O sobrenome deve ter no máximo 50 caracteres");

            RuleFor(x => x.Address)
                .NotNull().WithMessage("O endereço é obrigatório");

            RuleFor(x => x.Address.City)
                .NotEmpty().WithMessage("A cidade é obrigatória")
                .MaximumLength(100).WithMessage("A cidade deve ter no máximo 100 caracteres");

            RuleFor(x => x.Address.Street)
                .NotEmpty().WithMessage("A rua é obrigatória")
                .MaximumLength(200).WithMessage("A rua deve ter no máximo 200 caracteres");

            RuleFor(x => x.Address.Number)
                .GreaterThan(0).WithMessage("O número deve ser maior que zero");

            RuleFor(x => x.Address.Zipcode)
                .NotEmpty().WithMessage("O CEP é obrigatório")
                .Matches(@"^\d{5}-\d{3}$").WithMessage("CEP inválido");

            RuleFor(x => x.Address.Geolocation)
                .NotNull().WithMessage("A geolocalização é obrigatória");

            RuleFor(x => x.Address.Geolocation.Lat)
                .NotEmpty().WithMessage("A latitude é obrigatória");

            RuleFor(x => x.Address.Geolocation.Long)
                .NotEmpty().WithMessage("A longitude é obrigatória");

            RuleFor(x => x.Phone)
                .NotEmpty().WithMessage("O telefone é obrigatório")
                .Matches(@"^\(\d{2}\) \d{5}-\d{4}$").WithMessage("Telefone inválido");

            RuleFor(x => x.Status)
                .NotEmpty().WithMessage("O status é obrigatório")
                .Must(x => x == "Active" || x == "Inactive" || x == "Suspended")
                .WithMessage("Status inválido. Valores permitidos: Active, Inactive, Suspended");

            RuleFor(x => x.Role)
                .NotEmpty().WithMessage("O papel é obrigatório")
                .Must(x => x == "Customer" || x == "Manager" || x == "Admin")
                .WithMessage("Papel inválido. Valores permitidos: Customer, Manager, Admin");
        }
    }
} 