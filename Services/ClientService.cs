using NailDesignerAPI.DTOs;
using NailDesignerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace NailDesignerAPI.Services {
    public class ClientService {
        private readonly AppDbContext _context;

        public ClientService( AppDbContext context ) {
            _context = context;
        }

        public async Task<List<ClientDTO>> GetAllAsync() {

            return await _context.Clients
                .Select( c => new ClientDTO {
                    Id = c.Id,
                    Name = c.Name,
                    Phone = c.Phone,
                    BirthDay = c.BirthDay,
                    BirthMonth = c.BirthMonth
                } )
                .ToListAsync();
        }

        public async Task<ClientDTO?> GetByIdAsync( int id ) {

            return await _context.Clients
                .Where( c => c.Id == id )
                .Select( c => new ClientDTO {
                    Id = c.Id,
                    Name = c.Name,
                    Phone = c.Phone,
                    BirthDay = c.BirthDay,
                    BirthMonth = c.BirthMonth
                } )
                .FirstOrDefaultAsync();
        }

        public async Task<ClientDTO> CreateAsync( CreateClientDTO dto ) {

            var client = new Client {
                Name = dto.Name,
                Phone = dto.Phone,
                BirthDay = dto.BirthDay,
                BirthMonth = dto.BirthMonth
            };

            _context.Clients.Add( client );
            await _context.SaveChangesAsync();

            return new ClientDTO {
                Id = client.Id,
                Name = client.Name,
                Phone = client.Phone,
                BirthDay = client.BirthDay,
                BirthMonth = client.BirthMonth
            };
        }

        public async Task<ClientDTO?> UpdateAsync( int id, UpdateClientDTO dto ) {

            var client = await _context.Clients.FindAsync( id );

            if( client == null )
                return null;

            client.Name = dto.Name;
            client.Phone = dto.Phone;
            client.BirthDay = dto.BirthDay;
            client.BirthMonth = dto.BirthMonth;
            client.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ClientDTO {
                Id = client.Id,
                Name = client.Name,
                Phone = client.Phone,
                BirthDay = client.BirthDay,
                BirthMonth = client.BirthMonth
            };
        }

        public async Task<bool> DeleteAsync( int id ) {

            var client = await _context.Clients.FindAsync( id );

            if( client == null )
                return false;

            _context.Clients.Remove( client );
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
