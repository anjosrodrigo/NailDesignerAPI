using Microsoft.AspNetCore.Mvc;
using NailDesignerAPI.DTOs;
using NailDesignerAPI.Services;

namespace NailDesignerAPI.Controllers {

    [ApiController]
    [Route( "api/[controller]" )]

    public class ServiceTypeController : ControllerBase {

        private readonly ServiceTypeService _serviceTypeService;

        public ServiceTypeController( ServiceTypeService serviceTypeService ) {
            _serviceTypeService = serviceTypeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {
            var serviceTypes = await _serviceTypeService.GetAllAsync();
            return Ok( serviceTypes );
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetById( int id ) {

            var serviceType = await _serviceTypeService.GetByIdAsync( id );

            if( serviceType == null )
                return NotFound( new { message = "Serviço não encontrado." } );

            return Ok( serviceType );
        }

        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] CreateServiceTypeDTO dto ) {

            var serviceType = await _serviceTypeService.CreateAsync( dto );
            return CreatedAtAction( nameof( GetById ), new { id = serviceType.Id }, serviceType );
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> Update( int id, [FromBody] UpdateServiceTypeDTO dto ) {

            var serviceType = await _serviceTypeService.UpdateAsync( id, dto );

            if( serviceType == null )
                return NotFound( new { message = "Serviço não encontrado." } );

            return Ok( serviceType );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> Delete( int id ) {

            var result = await _serviceTypeService.DeleteAsync( id );

            if( !result )
                return NotFound( new { message = "Serviço não encontrado." } );

            return NoContent();
        }
    }
}
