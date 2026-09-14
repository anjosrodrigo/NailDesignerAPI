using FluentValidation;
using NailDesignerAPI.DTOs;
using NailDesignerAPI.Models;

namespace NailDesignerAPI.Validators {
    public class CreateServiceTypeValidator : AbstractValidator<CreateServiceTypeDTO> {
        public CreateServiceTypeValidator() {
            RuleFor( s => s.Name )
                .Cascade( CascadeMode.Stop )
                .NotEmpty().WithMessage( "Nome do serviço é obrigatório." )
                .MinimumLength( 3 ).WithMessage( "Nome do serviço deve ter pelo menos 3 caracteres." )
                .MaximumLength( 100 ).WithMessage( "Nome do serviço deve ter no máximo 100 caracteres." );

            RuleFor( s => s.Price )
                .GreaterThan( 0 ).WithMessage( "O valor do serviço deve ser maior do que R$ 0,00." );

            RuleFor( s => s.DurationMinutes )
                .Cascade( CascadeMode.Stop )
                .GreaterThan( 0 ).WithMessage( "A duração do serviço em minutos deve ser maior que zero." )
                .Must( d => d % 30 == 0 ).WithMessage( "A duração em minutos do serviço deve ser de 30 em 30 minutos. Ex.: 30, 60, 120." );

            RuleFor( s => s.PricePerUnit )
                .GreaterThan( 0 ).WithMessage( "Preço por unidade do serviço deve ser maior do que R$ 0,00." )
                .When( s => s.PricingType == PricingType.PerUnit || s.PricingType == PricingType.Both );
        }
    }

    public class UpdateServiceTypeValidator : AbstractValidator<UpdateServiceTypeDTO> {
        public UpdateServiceTypeValidator() {
            RuleFor( s => s.Name )
                .Cascade( CascadeMode.Stop )
                .NotEmpty().WithMessage( "Nome do serviço é obrigatório." )
                .MinimumLength( 3 ).WithMessage( "Nome do serviço deve ter pelo menos 3 caracteres." )
                .MaximumLength( 100 ).WithMessage( "Nome do serviço deve ter no máximo 100 caracteres." );

            RuleFor( s => s.Price )
                .GreaterThan( 0 ).WithMessage( "O valor do serviço deve ser maior do que R$ 0,00." );

            RuleFor( s => s.DurationMinutes )
                .Cascade( CascadeMode.Stop )
                .GreaterThan( 0 ).WithMessage( "A duração do serviço em minutos deve ser maior que zero." )
                .Must( d => d % 30 == 0 ).WithMessage( "A duração em minutos do serviço deve ser de 30 em 30 minutos. Ex.: 30, 60, 120." );

            RuleFor( s => s.PricePerUnit )
                .GreaterThan( 0 ).WithMessage( "Preço por unidade do serviço deve ser maior do que R$ 0,00." )
                .When( s => s.PricingType == PricingType.PerUnit || s.PricingType == PricingType.Both );
        }
    }
}
