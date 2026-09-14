using FluentValidation;
using NailDesignerAPI.DTOs;

namespace NailDesignerAPI.Validators {
    public class CreateServiceAddOnValidator : AbstractValidator<CreateServiceAddOnDTO> {
        public CreateServiceAddOnValidator() {
            RuleFor( sa => sa.Name )
                .Cascade( CascadeMode.Stop )
                .NotEmpty().WithMessage( "Nome do serviço adicional é obrigatório." )
                .MinimumLength( 3 ).WithMessage( "O nome do serviço adicional deve ter pelo menos 3 caracteres." )
                .MaximumLength( 100 ).WithMessage( "O nome do serviço adicional deve ter no máximo 100 caracteres." );

            RuleFor( sa => sa.PricePerUnit )
                .GreaterThan( 0 ).WithMessage( "Preço do serviço adicional deve ser maior do que R$ 0,00." );

            RuleFor( sa => sa.PriceAll )
                .GreaterThan( 0 ).WithMessage( "Preço combinado do serviço adicional deve ser maior do que R$ 0,00." );

            RuleFor( sa => sa.DurationMinutes )
                .GreaterThan( 0 ).WithMessage( "Duração em minutos do serviço adicional deve ser maior do que zero." )
                .Must( d => d % 30 == 0 ).WithMessage( "A duração em minutos do serviço adicional deve ser de 30 em 30 minutos. Ex.: 30, 60, 120." );

        }
    }

    public class UpdateServiceAddOnValidator : AbstractValidator<UpdateServiceAddOnDTO> {
        public UpdateServiceAddOnValidator() {
            RuleFor( sa => sa.Name )
                .Cascade( CascadeMode.Stop )
                .NotEmpty().WithMessage( "Nome do serviço adicional é obrigatório." )
                .MinimumLength( 3 ).WithMessage( "O nome do serviço adicional deve ter pelo menos 3 caracteres." )
                .MaximumLength( 100 ).WithMessage( "O nome do serviço adicional deve ter no máximo 100 caracteres." );

            RuleFor( sa => sa.PricePerUnit )
                .GreaterThan( 0 ).WithMessage( "Preço do serviço adicional deve ser maior do que R$ 0,00." );

            RuleFor( sa => sa.PriceAll )
                .GreaterThan( 0 ).WithMessage( "Preço combinado do serviço adicional deve ser maior do que R$ 0,00." );

            RuleFor( sa => sa.DurationMinutes )
                .Cascade(CascadeMode.Stop)
                .GreaterThan( 0 ).WithMessage( "Duração em minutos do serviço adicional deve ser maior do que zero." )
                .Must( d => d % 30 == 0 ).WithMessage( "A duração em minutos do serviço adicional deve ser de 30 em 30 minutos. Ex.: 30, 60, 120." );
        }
    }
}
