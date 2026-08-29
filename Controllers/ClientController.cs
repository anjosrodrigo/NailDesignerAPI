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
            return Ok( clients );
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetById( int id ) {

            var client = await _clientService.GetByIdAsync( id );

            if( client == null )
                return NotFound( new { message = "Cliente não encontrado." } );

            return Ok( client );
        }

        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] CreateClientDTO dto ) {

            var client = await _clientService.CreateAsync( dto );
            return CreatedAtAction( nameof( GetById ), new { id = client.Id }, client );
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> Update( int id, [FromBody] UpdateClientDTO dto ) {

            var client = await _clientService.UpdateAsync( id, dto );

            if( client == null )
                return NotFound( new { message = "Cliente não encontrado." } );

            return Ok( client );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> Delete( int id ) {

            var result = await _clientService.DeleteAsync( id );

            if( !result )
                return NotFound( new { message = "Cliente não encontrado." } );

            return NoContent();
        }
    }
}