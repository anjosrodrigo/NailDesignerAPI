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

            if( !serviceTypes.Success )
                return StatusCode( serviceTypes.StatusCode, new { message = serviceTypes.ErrorMessage } );

            return Ok( serviceTypes.Data );
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetById( int id ) {

            var serviceType = await _serviceTypeService.GetByIdAsync( id );

            if( !serviceType.Success )
                return StatusCode( serviceType.StatusCode, new { message = serviceType.ErrorMessage } );

            return Ok( serviceType.Data );
        }

        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] CreateServiceTypeDTO dto ) {

            var serviceType = await _serviceTypeService.CreateAsync( dto );

            if( !serviceType.Success )
                return StatusCode( serviceType.StatusCode, new { message = serviceType.ErrorMessage } );

            return CreatedAtAction( nameof( GetById ), new { id = serviceType.Data!.Id }, serviceType.Data );
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> Update( int id, [FromBody] UpdateServiceTypeDTO dto ) {

            var serviceType = await _serviceTypeService.UpdateAsync( id, dto );

            if( !serviceType.Success )
                return StatusCode(serviceType.StatusCode, new { message = serviceType.ErrorMessage} );

            return Ok( serviceType.Data );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> Delete( int id ) {

            var result = await _serviceTypeService.DeleteAsync( id );

            if( !result.Success )
                return StatusCode( result.StatusCode, new {message = result.ErrorMessage});

            return NoContent();
        }
    }
}
