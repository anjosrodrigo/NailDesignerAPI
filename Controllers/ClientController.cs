using Microsoft.AspNetCore.Mvc;
using NailDesignerAPI.DTOs;
using NailDesignerAPI.Services;

namespace NailDesignerAPI.Controllers {

    [ApiController]
    [Route( "api/[controller]" )]

    public class ClientController : ControllerBase {

        private readonly ClientService _clientService;

        public ClientController( ClientService clientService ) {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var clients = await _clientService.GetAllAsync();

            if( !clients.Success )
                return StatusCode( clients.StatusCode, new { message = clients.ErrorMessage } );

            return Ok( clients.Data );
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetById( int id ) {

            var client = await _clientService.GetByIdAsync( id );

            if( !client.Success )
                return StatusCode( client.StatusCode, new { message = client.ErrorMessage } );

            return Ok( client.Data );
        }

        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] CreateClientDTO dto ) {

            var client = await _clientService.CreateAsync( dto );

            if( !client.Success )
                return StatusCode( client.StatusCode, new { message = client.ErrorMessage } );

            return CreatedAtAction( nameof( GetById ), new { id = client.Data!.Id }, client.Data );
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> Update( int id, [FromBody] UpdateClientDTO dto ) {

            var client = await _clientService.UpdateAsync( id, dto );

            if( !client.Success )
                return StatusCode( client.StatusCode, new { message = client.ErrorMessage } );

            return Ok( client.Data );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> Delete( int id ) {

            var result = await _clientService.DeleteAsync( id );

            if( !result.Success )
                return StatusCode( result.StatusCode, new { message = result.ErrorMessage } );

            return NoContent();
        }
    }
}