using FluentValidation;
using NailDesignerAPI.DTOs;

namespace NailDesignerAPI.Validators {
    public class CreateClientValidator : AbstractValidator<CreateClientDTO> {
        public CreateClientValidator() {
            RuleFor( c => c.Name )
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage( "Nome é obrigatório." )
                .MinimumLength( 3 ).WithMessage( "Nome deve ter pelo menos 3 caracteres." )
                .MaximumLength( 100 ).WithMessage( "Nom deve ter no máximo 100 caracteres." );

            RuleFor( c => c.Phone )
                .Cascade( CascadeMode.Stop )
                .NotEmpty().WithMessage( "Telefone é obrigatório." )
                .Matches( @"^\d{12,13}$" ).WithMessage( "Telefone inválido. Use o formato: 5541999990000" );

            RuleFor( c => c.BirthDay )
                .InclusiveBetween( 1, 31 ).WithMessage( "Dia de nascimento deve estar entre 1 e 31." );

            RuleFor( c => c.BirthMonth )
                .InclusiveBetween( 1, 12 ).WithMessage( "Mês de nascimento deve estar entre 1 e 12." );
        }
    }

    public class UpdateClientValidator : AbstractValidator<UpdateClientDTO> {
        public UpdateClientValidator() {
            RuleFor( c => c.Name )
                .Cascade( CascadeMode.Stop )
                .NotEmpty().WithMessage( "Nome é obrigatório." )
                .MinimumLength( 3 ).WithMessage( "Nome deve ter pelo menos 3 caracteres." )
                .MaximumLength( 100 ).WithMessage( "Nome deve ter no máximo 100 caracteres." );

            RuleFor( c => c.Phone )
                .Cascade( CascadeMode.Stop )
                .NotEmpty().WithMessage( "Telefone é obrigatório." )
                .Matches( @"^\d{12,13}$" ).WithMessage( "Telefone inválido.Use o formato: 5541999990000" );

            RuleFor( c => c.BirthDay )
                .InclusiveBetween( 1, 31 ).WithMessage( "Dia de nascimento deve estar entre 1 e 31." );

            RuleFor( c => c.BirthMonth )
                .InclusiveBetween( 1, 12 ).WithMessage( "Mês de nascimento deve estar entre 1 e 12." );
        }
    }
}
