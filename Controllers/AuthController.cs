using Microsoft.AspNetCore.Mvc;
using NailDesignerAPI.DTOs;
using NailDesignerAPI.Models;
using NailDesignerAPI.Services;

namespace NailDesignerAPI.Controllers {

    [ApiController]
    [Route( "api/[controller]" )]

    public class AuthController : ControllerBase {

        private readonly AuthService _authService;

        public AuthController( AuthService authService ) {
            _authService = authService;
        }

        [HttpPost( "register" )]
        public async Task<IActionResult> Register( [FromBody] RegisterUserDTO dto ) {

            var result = await _authService.RegisterAsync( dto );

            if( !result.Success )
                return StatusCode( result.StatusCode, new { message = result.ErrorMessage } );

            return CreatedAtAction( nameof( Register ), result.Data );
        }

        [HttpPost( "login" )]
        public async Task<IActionResult> Login( [FromBody] LoginDTO dto ) {

            var result = await _authService.LoginAsync( dto );

            if( !result.Success )
                return StatusCode( result.StatusCode, new { message = result.ErrorMessage } );

            return Ok( result.Data );
        }
    }
}
