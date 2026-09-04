using NailDesignerAPI.DTOs;
using NailDesignerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace NailDesignerAPI.Services {
    public class ServiceTypeService {
        private readonly AppDbContext _context;

        public ServiceTypeService( AppDbContext context ) {
            _context = context;
        }

        public async Task<ServiceResult<List<ServiceTypeDTO>>> GetAllAsync() {

            var serviceTypes = await _context.ServiceTypes
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

            return ServiceResult<List<ServiceTypeDTO>>.Ok( serviceTypes );
        }

        public async Task<ServiceResult<ServiceTypeDTO>> GetByIdAsync( int id ) {

            var serviceType = await _context.ServiceTypes
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

            if( serviceType == null )
                return ServiceResult<ServiceTypeDTO>.NotFound( "Serviço não encontrado." );

            return ServiceResult<ServiceTypeDTO>.Ok( serviceType );
        }

        public async Task<ServiceResult<ServiceTypeDTO>> CreateAsync( CreateServiceTypeDTO dto ) {

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

            return ServiceResult<ServiceTypeDTO>.Created( new ServiceTypeDTO {
                Id = serviceType.Id,
                Name = serviceType.Name,
                Description = serviceType.Description,
                PricingType = serviceType.PricingType,
                Price = serviceType.Price,
                PricePerUnit = serviceType.PricePerUnit,
                DurationMinutes = serviceType.DurationMinutes,
                IsActive = serviceType.IsActive
            } );
        }

        public async Task<ServiceResult<ServiceTypeDTO>> UpdateAsync( int id, UpdateServiceTypeDTO dto ) {

            var serviceType = await _context.ServiceTypes.FindAsync( id );

            if( serviceType == null )
                return ServiceResult<ServiceTypeDTO>.NotFound( "Serviço não encontrado." );

            serviceType.Name = dto.Name;
            serviceType.Description = dto.Description;
            serviceType.PricingType = dto.PricingType;
            serviceType.Price = dto.Price;
            serviceType.PricePerUnit = dto.PricePerUnit;
            serviceType.DurationMinutes = dto.DurationMinutes;
            serviceType.IsActive = dto.IsActive;
            serviceType.UpdatedAt = DateTime.Now;

            await _context.SaveChangesAsync();

            return ServiceResult<ServiceTypeDTO>.Ok( new ServiceTypeDTO {
                Id = serviceType.Id,
                Name = serviceType.Name,
                Description = serviceType.Description,
                PricingType = serviceType.PricingType,
                Price = serviceType.Price,
                PricePerUnit = serviceType.PricePerUnit,
                DurationMinutes = serviceType.DurationMinutes,
                IsActive = serviceType.IsActive
            } );
        }

        public async Task<ServiceResult<bool>> DeleteAsync( int id ) {
            var serviceType = await _context.ServiceTypes.FindAsync( id );

            if( serviceType == null )
                return ServiceResult<bool>.NotFound( "Serviço não encontrado." );

            _context.ServiceTypes.Remove( serviceType );
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Ok( true );
        }
    }
}
