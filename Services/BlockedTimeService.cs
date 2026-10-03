using NailDesignerAPI.DTOs;
using NailDesignerAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace NailDesignerAPI.Services {
    public class BlockedTimeService {
        private readonly AppDbContext _context;

        public BlockedTimeService( AppDbContext context ) {
            _context = context;
        }

        public async Task<ServiceResult<List<BlockedTimeDTO>>> GetAllAsync() {

            var blockedTimes = await _context.BlockedTimes
                .Select( b => new BlockedTimeDTO {
                    Id = b.Id,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Reason = b.Reason
                } )
                .ToListAsync();

            return ServiceResult<List<BlockedTimeDTO>>.Ok( blockedTimes );
        }

        public async Task<ServiceResult<BlockedTimeDTO>> GetByIdAsync( int id ) {

            var blockedTimes = await _context.BlockedTimes
                .Where( b => b.Id == id )
                .Select( b => new BlockedTimeDTO {
                    Id = b.Id,
                    StartTime = b.StartTime,
                    EndTime = b.EndTime,
                    Reason = b.Reason
                } )
                .FirstOrDefaultAsync();

            if( blockedTimes == null )
                return ServiceResult<BlockedTimeDTO>.NotFound( "Horário bloqueado não encontrado." );

            return ServiceResult<BlockedTimeDTO>.Ok( blockedTimes );
        }

        public async Task<ServiceResult<BlockedTimeDTO>> CreateAsync( CreateBlockedTimeDTO dto ) {

            var blockedTime = new BlockedTime {
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                Reason = dto.Reason
            };

            _context.BlockedTimes.Add( blockedTime );
            await _context.SaveChangesAsync();

            return ServiceResult<BlockedTimeDTO>.Created( new BlockedTimeDTO {
                Id = blockedTime.Id,
                StartTime = blockedTime.StartTime,
                EndTime = blockedTime.EndTime,
                Reason = blockedTime.Reason
            } );
        }

        public async Task<ServiceResult<bool>> DeleteAsync( int id ) {

            var blockedTime = await _context.BlockedTimes.FindAsync( id );

            if( blockedTime == null )
                return ServiceResult<bool>.NotFound( "Horário bloqueado não encontrado." );

            _context.BlockedTimes.Remove( blockedTime );
            await _context.SaveChangesAsync();

            return ServiceResult<bool>.Ok( true );
        }
    }
}
