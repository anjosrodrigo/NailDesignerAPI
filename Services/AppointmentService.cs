using Microsoft.EntityFrameworkCore;
using NailDesignerAPI.DTOs;
using NailDesignerAPI.Models;

namespace NailDesignerAPI.Services {
    public class AppointmentService {
        private readonly AppDbContext _context;

        public AppointmentService( AppDbContext context ) {
            _context = context;
        }

        public async Task<List<AppointmentDTO>> GetAllAsync() {

            return await _context.Appointments
                .Include( a => a.Client )                   // loads the client data
                .Include( a => a.ServiceType )              // loads the service data
                .Include( a => a.AddOns )                   // loads the add-ons
                    .ThenInclude( ao => ao.ServiceAddOn )   // loads the data of each add-on
                .Select( a => new AppointmentDTO {
                    Id = a.Id,
                    ClientName = a.Client.Name,
                    ServiceTypeName = a.ServiceType.Name,
                    ServicePrice = a.ServiceType.Price,
                    Discount = a.Discount,
                    TotalPrice = a.TotalPrice,
                    Status = a.Status,
                    CancellationReason = a.CancellationReason,
                    CancellationNotes = a.CancellationNotes,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    AddOns = a.AddOns
                        .Select( ao => new AppointmentAddOnItemDTO {
                            Name = ao.ServiceAddOn.Name,
                            Quantity = ao.Quantity,
                            UnitPrice = ao.UnitPrice,
                            TotalPrice = ao.TotalPrice
                        } ).ToList()
                } )
                .ToListAsync();
        }

        public async Task<AppointmentDTO?> GetByIdAsync( int id ) {

            return await _context.Appointments
                .Include( a => a.Client )                   // loads the client data
                .Include( a => a.ServiceType )              // loads the service data
                .Include( a => a.AddOns )                   // loads the add-ons
                    .ThenInclude( ao => ao.ServiceAddOn )   // loads the data of each add-on
                .Where( a => a.Id == id )
                .Select( a => new AppointmentDTO {
                    Id = a.Id,
                    ClientName = a.Client.Name,
                    ServiceTypeName = a.ServiceType.Name,
                    ServicePrice = a.ServiceType.Price,
                    Discount = a.Discount,
                    TotalPrice = a.TotalPrice,
                    Status = a.Status,
                    CancellationReason = a.CancellationReason,
                    CancellationNotes = a.CancellationNotes,
                    StartTime = a.StartTime,
                    EndTime = a.EndTime,
                    AddOns = a.AddOns
                        .Select( ao => new AppointmentAddOnItemDTO {
                            Name = ao.ServiceAddOn.Name,
                            Quantity = ao.Quantity,
                            UnitPrice = ao.UnitPrice,
                            TotalPrice = ao.TotalPrice,
                        } ).ToList()
                } )
                .FirstOrDefaultAsync();
        }

        public async Task<AppointmentDTO?> CreateAsync( CreateAppointmentDTO dto ) {

            // step 1 - take the client from the database
            var client = await _context.Clients.FindAsync( dto.ClientId );

            if( client == null )
                return null;

            // step 2 — take the service type from the database
            var serviceType = await _context.ServiceTypes.FindAsync( dto.ServiceTypeId );

            if( serviceType == null )
                return null;

            // step 3 — take the add-ons from the database
            var addOnIds = dto.AddOns.Select( a => a.ServiceAddOnId ).ToList();
            var serviceAddOns = await _context.ServiceAddOns
                .Where( sa => addOnIds.Contains( sa.Id ) )
                .ToListAsync();

            // step 4 - calculate the EndTime
            int totalMinutes = serviceType.DurationMinutes;

            foreach( var addOn in serviceAddOns ) {
                totalMinutes += addOn.DurationMinutes;
            }

            DateTime endTime = dto.StartTime.AddMinutes( totalMinutes );

            // step 5 - validate overlaps
            bool hasConflict = await _context.Appointments
                .Where( a => a.Status != AppointmentStatus.Cancelled )
                .AnyAsync( a =>
                    dto.StartTime < a.EndTime &&
                    endTime > a.StartTime
                );

            if( hasConflict )
                return null;

            // step 6 - calculate the totalPrice
            double totalPrice = serviceType.Price;

            foreach( var dtoAddOn in dto.AddOns ) {
                var serviceAddOn = serviceAddOns.First( sa => sa.Id == dtoAddOn.ServiceAddOnId );
                double addOnTotal = serviceAddOn.PricePerUnit * dtoAddOn.Quantity;
                totalPrice += addOnTotal;
            }

            totalPrice -= dto.Discount;

            // step 7 - save the appointment
            var appointment = new Appointment {
                ClientId = dto.ClientId,
                ServiceTypeId = dto.ServiceTypeId,
                StartTime = dto.StartTime,
                EndTime = endTime,
                TotalPrice = totalPrice,
                Discount = dto.Discount,
                Status = AppointmentStatus.Scheduled
            };

            _context.Appointments.Add( appointment );
            await _context.SaveChangesAsync();

            // step 8 - save the add-ons
            foreach( var dtoAddOn in dto.AddOns ) {
                var serviceAddOn = serviceAddOns.First( sa => sa.Id == dtoAddOn.ServiceAddOnId );
                var appointmentAddOn = new AppointmentAddOn {
                    AppointmentId = appointment.Id,
                    ServiceAddOnId = serviceAddOn.Id,
                    Quantity = dtoAddOn.Quantity,
                    UnitPrice = serviceAddOn.PricePerUnit,
                    TotalPrice = serviceAddOn.PricePerUnit * dtoAddOn.Quantity
                };

                _context.AppointmentAddOns.Add( appointmentAddOn );
            }

            await _context.SaveChangesAsync();

            // step 9 - return the AppointmentDTO
            return new AppointmentDTO {
                Id = appointment.Id,
                ClientName = client.Name,
                ServiceTypeName = serviceType.Name,
                ServicePrice = serviceType.Price,
                Discount = appointment.Discount,
                TotalPrice = appointment.TotalPrice,
                Status = appointment.Status,
                CancellationReason = appointment.CancellationReason,
                CancellationNotes = appointment.CancellationNotes,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                AddOns = dto.AddOns.Select( dtoAddOn => {
                    var serviceAddOn = serviceAddOns.First( sa => sa.Id == dtoAddOn.ServiceAddOnId );
                    return new AppointmentAddOnItemDTO {
                        Name = serviceAddOn.Name,
                        Quantity = dtoAddOn.Quantity,
                        UnitPrice = serviceAddOn.PricePerUnit,
                        TotalPrice = serviceAddOn.PricePerUnit * dtoAddOn.Quantity
                    };
                } ).ToList()
            };
        }

        public async Task<AppointmentDTO?> UpdateAsync( int id, UpdateAppointmentDTO dto ) {

            // step 1 - take the appointment from the database
            var appointment = await _context.Appointments
                .Include( a => a.Client )
                .FirstOrDefaultAsync( appointment => appointment.Id == id );

            if( appointment == null )
                return null;

            // step 2 - take the service type from the database
            var serviceType = await _context.ServiceTypes.FindAsync( dto.ServiceTypeId );

            if( serviceType == null )
                return null;

            // step 3 - take the add-ons from the database
            var addOnsId = dto.AddOns.Select( a => a.ServiceAddOnId ).ToList();
            var serviceAddOns = await _context.ServiceAddOns
                .Where( sa => addOnsId.Contains( sa.Id ) )
                .ToListAsync();

            // step 4 - calculate the EndTime
            int totalMinutes = serviceType.DurationMinutes;

            foreach( var addOn in serviceAddOns ) {
                totalMinutes += addOn.DurationMinutes;
            }

            DateTime endTime = dto.StartTime.AddMinutes( totalMinutes );

            // step 5 - validate overlaps
            bool hasConflict = await _context.Appointments
                .Where( a => a.Status != AppointmentStatus.Cancelled && a.Id != id )
                .AnyAsync( a =>
                    dto.StartTime < a.EndTime &&
                    endTime > a.StartTime
                );

            if( hasConflict )
                return null;

            // step 6 - calculate the totalPrice
            double totalPrice = serviceType.Price;

            foreach( var dtoAddOn in dto.AddOns ) {
                var serviceAddOn = serviceAddOns.First( sa => sa.Id == dtoAddOn.ServiceAddOnId );
                double addOnTotal = serviceAddOn.PricePerUnit * dtoAddOn.Quantity;
                totalPrice += addOnTotal;
            }

            totalPrice -= dto.Discount;

            // step 7 - update the appointment
            appointment.TotalPrice = totalPrice;
            appointment.Discount = dto.Discount;
            appointment.StartTime = dto.StartTime;
            appointment.EndTime = endTime;
            appointment.Status = dto.Status;
            appointment.CancellationReason = dto.CancellationReason;
            appointment.CancellationNotes = dto.CancellationNotes;
            appointment.ServiceTypeId = dto.ServiceTypeId;
            appointment.UpdatedAt = DateTime.Now;

            // step 8 - delete old add-ons e save new add-ons
            var oldAddOns = await _context.AppointmentAddOns
                .Where( ao => ao.AppointmentId == id )
                .ToListAsync();

            _context.AppointmentAddOns.RemoveRange( oldAddOns );

            foreach( var dtoAddOn in dto.AddOns ) {
                var serviceAddOn = serviceAddOns.First( sa => sa.Id == dtoAddOn.ServiceAddOnId );
                var appointmentAddOn = new AppointmentAddOn {
                    AppointmentId = appointment.Id,
                    ServiceAddOnId = dtoAddOn.ServiceAddOnId,
                    Quantity = dtoAddOn.Quantity,
                    UnitPrice = serviceAddOn.PricePerUnit,
                    TotalPrice = serviceAddOn.PricePerUnit * dtoAddOn.Quantity
                };

                _context.AppointmentAddOns.Add( appointmentAddOn );
            }

            await _context.SaveChangesAsync();

            // step 9 - return the AppointmentDTO
            return new AppointmentDTO {
                Id = appointment.Id,
                ClientName = appointment.Client.Name,
                ServiceTypeName = serviceType.Name,
                ServicePrice = serviceType.Price,
                Discount = appointment.Discount,
                TotalPrice = appointment.TotalPrice,
                Status = appointment.Status,
                CancellationReason = appointment.CancellationReason,
                CancellationNotes = appointment.CancellationNotes,
                StartTime = appointment.StartTime,
                EndTime = appointment.EndTime,
                AddOns = dto.AddOns.Select( dtoAddOn => {
                    var serviceAddOn = serviceAddOns.First( sa => sa.Id == dtoAddOn.ServiceAddOnId );
                    return new AppointmentAddOnItemDTO {
                        Name = serviceAddOn.Name,
                        Quantity = dtoAddOn.Quantity,
                        UnitPrice = serviceAddOn.PricePerUnit,
                        TotalPrice = serviceAddOn.PricePerUnit * dtoAddOn.Quantity
                    };
                } ).ToList()
            };
        }

        public async Task<bool> DeleteAsync( int id ) {
            var appointment = await _context.Appointments.FindAsync( id );

            if(appointment == null)
                return false;

            _context.Appointments.Remove( appointment );
            await _context.SaveChangesAsync();

            return true;
        }
    }
}
