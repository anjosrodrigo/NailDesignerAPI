using Microsoft.EntityFrameworkCore;
using NailDesignerAPI.Models;
using NailDesignerAPI.DTOs;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace NailDesignerAPI.Services {
    public class AuthService {

        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public AuthService( AppDbContext context, IConfiguration configuration ) {
            _context = context;
            _configuration = configuration;
        }

        public async Task<ServiceResult<AuthResponseDTO>> RegisterAsync( RegisterUserDTO dto ) {

            // check if email already exists
            var emailExists = await _context.Users
                .AnyAsync( u => u.Email == dto.Email );

            if( emailExists )
                return ServiceResult<AuthResponseDTO>.Conflict( "Email já cadastrado" );

            // create user with hashed password
            var user = new User {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword( dto.Password )
            };

            _context.Users.Add( user );
            await _context.SaveChangesAsync();

            return ServiceResult<AuthResponseDTO>.Created( GenerateToken( user ) );
        }

        public async Task<ServiceResult<AuthResponseDTO>> LoginAsync( LoginDTO dto ) {

            // Take the user from the database by email and chack if it is active
            var user = await _context.Users
                .FirstOrDefaultAsync( u => u.Email == dto.Email && u.IsActive );

            if( user == null )
                return ServiceResult<AuthResponseDTO>.NotFound( "Email ou senha inválidos." );

            // Check if the password is correct
            bool validPassword = BCrypt.Net.BCrypt.Verify( dto.Password, user.PasswordHash );

            if( !validPassword )
                return ServiceResult<AuthResponseDTO>.NotFound( "Email ou senha inválidos." );

            return ServiceResult<AuthResponseDTO>.Ok( GenerateToken( user ) );
        }

        private AuthResponseDTO GenerateToken( User user ) {

            var key = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( _configuration[ "Jwt:Key" ]! ) );
            var creds = new SigningCredentials( key, SecurityAlgorithms.HmacSha256 );
            var expires = DateTime.Now.AddHours( double.Parse( _configuration[ "Jwt:ExpiresInHours" ]! ) );

            var claims = new[] {
                new Claim( ClaimTypes.NameIdentifier, user.Id.ToString() ),
                new Claim( ClaimTypes.Name, user.Name ),
                new Claim( ClaimTypes.Email, user.Email )
            };

            var token = new JwtSecurityToken(
                claims: claims,
                expires: expires,
                signingCredentials: creds
            );

            return new AuthResponseDTO {
                Token = new JwtSecurityTokenHandler().WriteToken( token ),
                Name = user.Name,
                Email = user.Email,
                ExpiresAt = expires
            };
        }
    }
}
