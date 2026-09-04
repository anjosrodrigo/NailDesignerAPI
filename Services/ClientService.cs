using NailDesignerAPI.DTOs;
using NailDesignerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace NailDesignerAPI.Services {
    public class ClientService {
        private readonly AppDbContext _context;

        public ClientService( AppDbContext context ) {
            _context = context;
        }

        public async Task<ServiceResult<List<ClientDTO>>> GetAllAsync() {

            var clients = await _context.Clients
                .Select( c => new ClientDTO {
                    Id = c.Id,
                    Name = c.Name,
                    Phone = c.Phone,
                    BirthDay = c.BirthDay,
                    BirthMonth = c.BirthMonth
                } )
                .ToListAsync();

            return ServiceResult<List<ClientDTO>>.Ok( clients );
        }

        public async Task<ServiceResult<ClientDTO>> GetByIdAsync( int id ) {

            var client = await _context.Clients
                .Where( c => c.Id == id )
                .Select( c => new ClientDTO {
                    Id = c.Id,
                    Name = c.Name,
                    Phone = c.Phone,
                    BirthDay = c.BirthDay,
                    BirthMonth = c.BirthMonth
                } )
                .FirstOrDefaultAsync();

            if( client == null )
                return ServiceResult<ClientDTO>.NotFound( "Cliente não encontrado." );

            return ServiceResult<ClientDTO>.Ok( client );
        }

        public async Task<ServiceResult<ClientDTO>> CreateAsync( CreateClientDTO dto ) {

            var client = new Client {
                Name = dto.Name,
                Phone = dto.Phone,
                BirthDay = dto.BirthDay,
                BirthMonth = dto.BirthMonth
            };

            _context.Clients.Add( client );
            await _context.SaveChangesAsync();

            return ServiceResult<ClientDTO>.Created( new ClientDTO {
                Id = client.Id,
                Name = client.Name,
                Phone = client.Phone,
                BirthDay = client.BirthDay,
                BirthMonth = client.BirthMonth
            } );
        }

        public async Task<ServiceResult<ClientDTO>> UpdateAsync( int id, UpdateClientDTO dto ) {

            var client = await _context.Clients.FindAsync( id );

            if( client == null )
                return ServiceResult<ClientDTO>.NotFound( "Cliente não encontrado." );

            client.Name = dto.Name;
            client.Phone = dto.Phone;
            client.BirthDay = dto.BirthDay;
            client.BirthMonth = dto.BirthMonth;
            client.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return ServiceResult<ClientDTO>.Ok( new ClientDTO {
                Id = client.Id,
                Name = client.Name,
                Phone = client.Phone,
                BirthDay = client.BirthDay,
                BirthMonth = client.BirthMonth
            } );
        }

        public async Task<ServiceResult<bool>> DeleteAsync( int id ) {

            var client = await _context.Clients.FindAsync( id );

            if( client == null )
                return ServiceResult<bool>.NotFound( "Cliente não encontrado." );

            _context.Clients.Remove( client );
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Ok( true );
        }
    }
}
