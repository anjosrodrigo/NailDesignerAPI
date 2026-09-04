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

            if( !serviceAddOns.Success )
                return StatusCode( serviceAddOns.StatusCode, new { message = serviceAddOns.ErrorMessage } );

            return Ok( serviceAddOns.Data );
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetById( int id ) {

            var serviceAddOn = await _serviceAddOnService.GetByIdAsync( id );

            if( !serviceAddOn.Success )
                return StatusCode( serviceAddOn.StatusCode, new { message = serviceAddOn.ErrorMessage } );

            return Ok( serviceAddOn.Data );
        }

        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] CreateServiceAddOnDTO dto ) {

            var serviceAddOn = await _serviceAddOnService.CreateAsync( dto );

            if( !serviceAddOn.Success )
                return StatusCode( serviceAddOn.StatusCode, new { message = serviceAddOn.ErrorMessage } );

            return CreatedAtAction( nameof( GetById ), new { Id = serviceAddOn.Data!.Id }, serviceAddOn.Data );
        }

        [HttpPut( "{id}" )]
        public async Task<IActionResult> Update( int id, [FromBody] UpdateServiceAddOnDTO dto ) {

            var serviceAddOn = await _serviceAddOnService.UpdateAsync( id, dto );

            if( !serviceAddOn.Success )
                return StatusCode( serviceAddOn.StatusCode, new { message = serviceAddOn.ErrorMessage } );

            return Ok( serviceAddOn.Data );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> Delete( int id ) {

            var result = await _serviceAddOnService.DeleteAsync( id );

            if( !result.Success )
                return StatusCode( result.StatusCode, new { message = result.ErrorMessage } );

            return NoContent();
        }
    }
}