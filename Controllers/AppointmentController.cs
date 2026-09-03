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

            if( !appointments.Success )
                return StatusCode( appointments.StatusCode, new { message = appointments.ErrorMessage } );

            return Ok( appointments.Data );
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetById( int id ) {

            var appointment = await _appointmentService.GetByIdAsync( id );

            if( !appointment.Success )
                return StatusCode( appointment.StatusCode, new { message = appointment.ErrorMessage } );

            return Ok( appointment.Data );
        }

        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] CreateAppointmentDTO dto ) {

            var appointment = await _appointmentService.CreateAsync( dto );

            if( !appointment.Success )
                return StatusCode( appointment.StatusCode, new { message = appointment.ErrorMessage } );

            return CreatedAtAction( nameof( GetById ), new { id = appointment.Data!.Id }, appointment.Data );
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> Update( int id, [FromBody] UpdateAppointmentDTO dto ) {

            var appointment = await _appointmentService.UpdateAsync( id, dto );

            if( !appointment.Success )
                return StatusCode( appointment.StatusCode, new { message = appointment.ErrorMessage } );

            return Ok( appointment.Data );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> Delete( int id ) {

            var result = await _appointmentService.DeleteAsync( id );

            if( !result.Success )
                return StatusCode( result.StatusCode, new { message = result.ErrorMessage } );

            return NoContent();
        }
    }
}
