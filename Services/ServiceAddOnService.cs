using NailDesignerAPI.DTOs;
using NailDesignerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace NailDesignerAPI.Services {
    public class ServiceAddOnService {
        private readonly AppDbContext _context;

        public ServiceAddOnService( AppDbContext context ) {
            _context = context;
        }

        public async Task<List<ServiceAddOnDTO>> GetAllAsync() {

            return await _context.ServiceAddOns
                .Select( sa => new ServiceAddOnDTO {
                    Id = sa.Id,
                    Name = sa.Name,
                    Description = sa.Description,
                    PricePerUnit = sa.PricePerUnit,
                    PriceAll = sa.PriceAll,
                    IsActive = sa.IsActive
                } )
                .ToListAsync();
        }

        public async Task<ServiceAddOnDTO?> GetByIdAsync( int id ) {

            return await _context.ServiceAddOns
                .Where( sa => sa.Id == id )
                .Select( sa => new ServiceAddOnDTO {
                    Id = sa.Id,
                    Name = sa.Name,
                    Description = sa.Description,
                    PricePerUnit = sa.PricePerUnit,
                    PriceAll = sa.PriceAll,
                    IsActive = sa.IsActive
                } )
                .FirstOrDefaultAsync();
        }

        public async Task<ServiceAddOnDTO> CreateAsync( CreateServiceAddOnDTO dto ) {

            var serviceAddOn = new ServiceAddOn {
                Name = dto.Name,
                Description= dto.Description,
                PricePerUnit = dto.PricePerUnit,
                PriceAll = dto.PriceAll,
                IsActive = dto.IsActive
            };

            _context.ServiceAddOns.Add( serviceAddOn );
            await _context.SaveChangesAsync();

            return new ServiceAddOnDTO {
                Id = serviceAddOn.Id,
                Name = serviceAddOn.Name,
                Description = serviceAddOn.Description,
                PricePerUnit = serviceAddOn.PricePerUnit,
                PriceAll = serviceAddOn.PriceAll,
                IsActive = serviceAddOn.IsActive
            };
        }

        public async Task<ServiceAddOnDTO?> UpdateAsync( int id, UpdateServiceAddOnDTO dto ) {

            var serviceAddOn = await _context.ServiceAddOns.FindAsync( id );

            if( serviceAddOn == null )
                return null;

            serviceAddOn.Name = dto.Name;
            serviceAddOn.Description = dto.Description;
            serviceAddOn.PricePerUnit = dto.PricePerUnit;
            serviceAddOn.PriceAll = dto.PriceAll;
            serviceAddOn.IsActive = dto.IsActive;
            serviceAddOn.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ServiceAddOnDTO {
                Id = serviceAddOn.Id,
                Name = serviceAddOn.Name,
                Description = serviceAddOn.Description,
                PricePerUnit = serviceAddOn.PricePerUnit,
                PriceAll = serviceAddOn.PriceAll,
                IsActive = serviceAddOn.IsActive
            };
        }

        public async Task<bool> DeleteAsync( int id ) {

            var serviceAddOn = await _context.ServiceAddOns.FindAsync( id );

            if( serviceAddOn == null )
                return false;

            _context.ServiceAddOns.Remove( serviceAddOn );
            await _context.SaveChangesAsync();
            
            return true;
        }
    }
}
