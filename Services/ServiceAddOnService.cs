using NailDesignerAPI.DTOs;
using NailDesignerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace NailDesignerAPI.Services {
    public class ServiceAddOnService {
        private readonly AppDbContext _context;

        public ServiceAddOnService( AppDbContext context ) {
            _context = context;
        }

        public async Task<ServiceResult<List<ServiceAddOnDTO>>> GetAllAsync() {

            var serviceAddOns = await _context.ServiceAddOns
                .Select( sa => new ServiceAddOnDTO {
                    Id = sa.Id,
                    Name = sa.Name,
                    Description = sa.Description,
                    PricePerUnit = sa.PricePerUnit,
                    PriceAll = sa.PriceAll,
                    DurationMinutes = sa.DurationMinutes,
                    IsActive = sa.IsActive
                } )
                .ToListAsync();

            return ServiceResult<List<ServiceAddOnDTO>>.Ok( serviceAddOns );
        }

        public async Task<ServiceResult<ServiceAddOnDTO>> GetByIdAsync( int id ) {

            var serviceAddOn = await _context.ServiceAddOns
                .Where( sa => sa.Id == id )
                .Select( sa => new ServiceAddOnDTO {
                    Id = sa.Id,
                    Name = sa.Name,
                    Description = sa.Description,
                    PricePerUnit = sa.PricePerUnit,
                    PriceAll = sa.PriceAll,
                    DurationMinutes = sa.DurationMinutes,
                    IsActive = sa.IsActive
                } )
                .FirstOrDefaultAsync();

            if( serviceAddOn == null )
                return ServiceResult<ServiceAddOnDTO>.NotFound( "Serviço adicional não encontrado." );

            return ServiceResult<ServiceAddOnDTO>.Ok( serviceAddOn );
        }

        public async Task<ServiceResult<ServiceAddOnDTO>> CreateAsync( CreateServiceAddOnDTO dto ) {

            var serviceAddOn = new ServiceAddOn {
                Name = dto.Name,
                Description = dto.Description,
                PricePerUnit = dto.PricePerUnit,
                PriceAll = dto.PriceAll,
                DurationMinutes = dto.DurationMinutes,
                IsActive = dto.IsActive
            };

            _context.ServiceAddOns.Add( serviceAddOn );
            await _context.SaveChangesAsync();

            return ServiceResult<ServiceAddOnDTO>.Created( new ServiceAddOnDTO {
                Id = serviceAddOn.Id,
                Name = serviceAddOn.Name,
                Description = serviceAddOn.Description,
                PricePerUnit = serviceAddOn.PricePerUnit,
                PriceAll = serviceAddOn.PriceAll,
                DurationMinutes = serviceAddOn.DurationMinutes,
                IsActive = serviceAddOn.IsActive
            } );
        }

        public async Task<ServiceResult<ServiceAddOnDTO>> UpdateAsync( int id, UpdateServiceAddOnDTO dto ) {

            var serviceAddOn = await _context.ServiceAddOns.FindAsync( id );

            if( serviceAddOn == null )
                return ServiceResult<ServiceAddOnDTO>.NotFound( "Serviço adicional não encontrado." );

            serviceAddOn.Name = dto.Name;
            serviceAddOn.Description = dto.Description;
            serviceAddOn.PricePerUnit = dto.PricePerUnit;
            serviceAddOn.PriceAll = dto.PriceAll;
            serviceAddOn.DurationMinutes = dto.DurationMinutes;
            serviceAddOn.IsActive = dto.IsActive;
            serviceAddOn.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return ServiceResult<ServiceAddOnDTO>.Ok( new ServiceAddOnDTO {
                Id = serviceAddOn.Id,
                Name = serviceAddOn.Name,
                Description = serviceAddOn.Description,
                PricePerUnit = serviceAddOn.PricePerUnit,
                PriceAll = serviceAddOn.PriceAll,
                DurationMinutes = serviceAddOn.DurationMinutes,
                IsActive = serviceAddOn.IsActive
            } );
        }

        public async Task<ServiceResult<bool>> DeleteAsync( int id ) {

            var serviceAddOn = await _context.ServiceAddOns.FindAsync( id );

            if( serviceAddOn == null )
                return ServiceResult<bool>.NotFound( "Serviço adicional não encontrado." );

            _context.ServiceAddOns.Remove( serviceAddOn );
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Ok( true );
        }
    }
}
