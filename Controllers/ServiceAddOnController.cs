using Microsoft.AspNetCore.Mvc;
using NailDesignerAPI.Services;
using NailDesignerAPI.DTOs;

namespace NailDesignerAPI.Controllers {

    [ApiController]
    [Route( "api/[controller]" )]

    public class ServiceAddOnController : ControllerBase {

        private readonly ServiceAddOnService _serviceAddOnService;

        public ServiceAddOnController( ServiceAddOnService serviceAddOnService ) {
            _serviceAddOnService = serviceAddOnService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var serviceAddOns = await _serviceAddOnService.GetAllAsync();
            return Ok( serviceAddOns );
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetById( int id ) {

            var serviceAddOn = await _serviceAddOnService.GetByIdAsync( id );

            if( serviceAddOn == null )
                return NotFound( new { message = "Serviço adicional não encontrado." } );

            return Ok( serviceAddOn );
        }

        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] CreateServiceAddOnDTO dto ) {

            var serviceAddOn = await _serviceAddOnService.CreateAsync( dto );
            return CreatedAtAction( nameof( GetById ), new { Id = serviceAddOn.Id }, serviceAddOn );
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> Update( int id, [FromBody] UpdateServiceAddOnDTO dto ) {

            var serviceAddOn = await _serviceAddOnService.UpdateAsync( id, dto );

            if( serviceAddOn == null )
                return NotFound( new { message = "Serviço adicional não encontrado." } );

            return Ok( serviceAddOn );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> Delete( int id ) {

            var result = await _serviceAddOnService.DeleteAsync( id );

            if( !result )
                return NotFound( new { message = "Serviço adicional não encontrado." } );

            return NoContent();
        }
    }
}