using Microsoft.AspNetCore.Mvc;
using NailDesignerAPI.DTOs;
using NailDesignerAPI.Services;

namespace NailDesignerAPI.Controllers {

    [ApiController]
    [Route( "api/[controller]" )]

    public class AppointmentController : ControllerBase {

        private readonly AppointmentService _appointmentService;

        public AppointmentController( AppointmentService appointmentService ) {
            _appointmentService = appointmentService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var appointments = await _appointmentService.GetAllAsync();
            return Ok( appointments );
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetById( int id ) {

            var appointment = await _appointmentService.GetByIdAsync( id );

            if( appointment == null )
                return NotFound( new { message = "Agendamento não encontrado." } );

            return Ok( appointment );
        }

        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] CreateAppointmentDTO dto ) {

            var appointment = await _appointmentService.CreateAsync( dto );

            if( appointment == null )
                return Conflict( new { message = "Não foi possível criar o agendamento. Verifique os dados e conflitos de horário." } );

            return CreatedAtAction( nameof( GetById ), new { Id = appointment.Id }, appointment );
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> Update( int id, [FromBody] UpdateAppointmentDTO dto ) {

            var appointment = await _appointmentService.UpdateAsync( id, dto );

            if( appointment == null )
                return Conflict( new { message = "Não foi possível atualizar o agendamento. Verifique os dados e conflitos de horário." } );

            return Ok( appointment );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> Delete( int id ) {

            var result = await _appointmentService.DeleteAsync( id );

            if( ( !result ) )
                return NotFound( new { message = "Agendamento não encontrado." } );

            return NoContent();
        }
    }
}
