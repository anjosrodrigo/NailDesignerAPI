using FluentValidation;
using NailDesignerAPI.DTOs;

namespace NailDesignerAPI.Validators {
    public class CreateAppointmentValidator : AbstractValidator<CreateAppointmentDTO> {
        public CreateAppointmentValidator() {
            RuleFor( a => a.ClientId )
                .GreaterThan( 0 ).WithMessage( "Cliente informado não encontrado." );

            RuleFor( a => a.ServiceTypeId )
                .GreaterThan( 0 ).WithMessage( "Serviço informado não encontrado." );

            RuleFor( a => a.StartTime )
                .Must( d => d >= DateTime.Now.AddMinutes( -1 ) ).WithMessage( "Horário do agendamento deve ser maior que a hora atual." );

            RuleFor( a => a.Discount )
                .GreaterThanOrEqualTo( 0 ).WithMessage( "O valor do desconto deve ser igual ou maior do que R$ 0,00." );

            RuleForEach( a => a.AddOns )
                .SetValidator( new CreateAppointmentAddOnValidator() );
        }
    }

    public class CreateAppointmentAddOnValidator : AbstractValidator<CreateAppointmentAddOnDTO> {
        public CreateAppointmentAddOnValidator() {
            RuleFor( ad => ad.ServiceAddOnId )
                .GreaterThan( 0 ).WithMessage( "Serviço Adicional informado não encontrado." );

            RuleFor( ad => ad.Quantity )
                .GreaterThan( 0 ).WithMessage( "A quantidade de serviços adicionais realizados deve ser maior do que zero." );
        }
    }

    public class UpdateAppointmentValidator : AbstractValidator<UpdateAppointmentDTO> {
        public UpdateAppointmentValidator() {
            RuleFor( a => a.ServiceTypeId )
                .GreaterThan( 0 ).WithMessage( "Serviço informado não encontrado." );

            RuleFor( a => a.StartTime )
                .Must( d => d >= DateTime.Now.AddMinutes( -1 ) ).WithMessage( "Horário do agendamento deve ser maior do que a hora atual." );

            RuleFor( a => a.Discount )
                .GreaterThanOrEqualTo( 0 ).WithMessage( "O valor do desconto deve ser igual ou maior do que R$ 0,00." );
        }
    }
}
