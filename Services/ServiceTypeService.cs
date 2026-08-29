using NailDesignerAPI.DTOs;
using NailDesignerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace NailDesignerAPI.Services {
    public class ServiceTypeService {
        private readonly AppDbContext _context;

        public ServiceTypeService( AppDbContext context ) {
            _context = context;
        }

        public async Task<List<ServiceTypeDTO>> GetAllAsync() {

            return await _context.ServiceTypes
                .Select( st => new ServiceTypeDTO {
                    Id = st.Id,
                    Name = st.Name,
                    Description = st.Description,
                    PricingType = st.PricingType,
                    Price = st.Price,
                    PricePerUnit = st.PricePerUnit,
                    DurationMinutes = st.DurationMinutes,
                    IsActive = st.IsActive
                } )
                .ToListAsync();
        }

        public async Task<ServiceTypeDTO?> GetByIdAsync( int id ) {

            return await _context.ServiceTypes
                .Where( st => st.Id == id )
                .Select( st => new ServiceTypeDTO {
                    Id = st.Id,
                    Name = st.Name,
                    Description = st.Description,
                    PricingType = st.PricingType,
                    Price = st.Price,
                    PricePerUnit = st.PricePerUnit,
                    DurationMinutes = st.DurationMinutes,
                    IsActive = st.IsActive
                } )
                .FirstOrDefaultAsync();
        }

        public async Task<ServiceTypeDTO> CreateAsync( CreateServiceTypeDTO dto ) {

            var serviceType = new ServiceType {
                Name = dto.Name,
                Description = dto.Description,
                PricingType = dto.PricingType,
                Price = dto.Price,
                PricePerUnit = dto.PricePerUnit,
                DurationMinutes = dto.DurationMinutes,
                IsActive = dto.IsActive
            };

            _context.Add( serviceType );
            await _context.SaveChangesAsync();

            return new ServiceTypeDTO {
                Id = serviceType.Id,
                Name = serviceType.Name,
                Description = serviceType.Description,
                PricingType = serviceType.PricingType,
                Price = serviceType.Price,
                PricePerUnit = serviceType.PricePerUnit,
                DurationMinutes = serviceType.DurationMinutes,
                IsActive = serviceType.IsActive
            };
        }

        public async Task<ServiceTypeDTO?> UpdateAsync( int id, UpdateServiceTypeDTO dto ) {

            var serviceType = await _context.ServiceTypes.FindAsync( id );

            if( serviceType == null )
                return null;

            serviceType.Name = dto.Name;
            serviceType.Description = dto.Description;
            serviceType.PricingType = dto.PricingType;
            serviceType.Price = dto.Price;
            serviceType.PricePerUnit = dto.PricePerUnit;
            serviceType.DurationMinutes = dto.DurationMinutes;
            serviceType.IsActive = dto.IsActive;
            serviceType.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return new ServiceTypeDTO {
                Id = serviceType.Id,
                Name = serviceType.Name,
                Description = serviceType.Description,
                PricingType = serviceType.PricingType,
                Price = serviceType.Price,
                PricePerUnit = serviceType.PricePerUnit,
                DurationMinutes = serviceType.DurationMinutes,
                IsActive = serviceType.IsActive
            };
        }

        public async Task<bool> DeleteAsync( int id ) {
            var serviceType = await _context.ServiceTypes.FindAsync( id );

            if( serviceType == null )
                return false;

            _context.ServiceTypes.Remove( serviceType );
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
