using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NailDesignerAPI.Services;
using NailDesignerAPI.DTOs;

namespace NailDesignerAPI.Controllers {

    [ApiController]
    [Route( "api/[controller]" )]
    [Authorize]

    public class BlockedTimeController : ControllerBase {
        private readonly BlockedTimeService _blockedTimeService;

        public BlockedTimeController( BlockedTimeService blockedTimeService ) {
            _blockedTimeService = blockedTimeService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() {

            var blockedTimes = await _blockedTimeService.GetAllAsync();

            if( !blockedTimes.Success )
                return StatusCode( blockedTimes.StatusCode, new { message = blockedTimes.ErrorMessage } );

            return Ok( blockedTimes.Data );
        }

        [HttpGet( "{id}" )]
        public async Task<IActionResult> GetById( int id ) {

            var blockedTime = await _blockedTimeService.GetByIdAsync( id );

            if( !blockedTime.Success )
                return StatusCode( blockedTime.StatusCode, new { message = blockedTime.ErrorMessage } );

            return Ok( blockedTime.Data );
        }

        [HttpPost]
        public async Task<IActionResult> Create( [FromBody] CreateBlockedTimeDTO dto ) {

            var blockedTime = await _blockedTimeService.CreateAsync( dto );

            if( !blockedTime.Success )
                return StatusCode( blockedTime.StatusCode, new { message = blockedTime.ErrorMessage } );

            return CreatedAtAction( nameof( GetById ), new { id = blockedTime.Data!.Id }, blockedTime.Data );
        }

        [HttpDelete( "{id}" )]
        public async Task<IActionResult> DeleteAsync( int id ) {

            var result = await _blockedTimeService.DeleteAsync( id );

            if(!result.Success)
                return StatusCode( result.StatusCode, new { message = result.ErrorMessage } );

            return NoContent();
        }
    }
}
