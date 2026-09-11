using Microsoft.EntityFrameworkCore;
using NailDesignerAPI.Models;

namespace NailDesignerAPI.Services {
    public class AppointmentReminderService : BackgroundService {

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AppointmentReminderService> _logger;

        public AppointmentReminderService( IServiceProvider serviceProvider, ILogger<AppointmentReminderService> logger ) {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync( CancellationToken stoppingToken ) {

            _logger.LogInformation( "Appointment Reminder started." );

            while( !stoppingToken.IsCancellationRequested ) {

                var now = DateTime.Now;

                // send reminders at 18:00
                if( now.Hour == 18 && now.Minute == 0 )
                    await SendRemindersAsync();

                // send birthday messages at 10:00
                if( now.Hour == 10 && now.Minute == 0 )
                    await SendBirthdayMessagesAsync();

                // check every minute
                await Task.Delay( TimeSpan.FromMinutes( 1 ), stoppingToken );
            }
        }

        private async Task SendRemindersAsync() {

            _logger.LogInformation( "Sending appointment reminders..." );

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var whatsApp = scope.ServiceProvider.GetRequiredService<WhatsAppService>();

            var tomorrow = DateOnly.FromDateTime( DateTime.Now.AddDays( 1 ) );

            var appointments = await context.Appointments
                .Include( a => a.Client )
                .Include( a => a.ServiceType )
                .Where( a =>
                    a.Status == AppointmentStatus.Scheduled ||
                    a.Status == AppointmentStatus.Confirmed )
                .Where( a => DateOnly.FromDateTime( a.StartTime ) == tomorrow )
                .ToListAsync();

            foreach( var appointment in appointments ) {

                await whatsApp.SendAppointmentReminderAsync(
                    appointment.Client.Phone,
                    appointment.Client.Name,
                    appointment.StartTime,
                    appointment.ServiceType.Name
                    );

                _logger.LogInformation( $"Reminder sent to {appointment.Client.Name}" );
            }
        }

        private async Task SendBirthdayMessagesAsync() {

            _logger.LogInformation( "Sending birthday messages..." );

            using var scope = _serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            var whatsApp = scope.ServiceProvider.GetRequiredService<WhatsAppService>();

            var today = DateTime.Now;

            var clients = await context.Clients
                .Where( c => c.BirthDay == today.Day && c.BirthMonth == today.Month )
                .ToListAsync();

            foreach( var client in clients ) {

                await whatsApp.SendBirthdayMessageAsync(
                    client.Phone,
                    client.Name
                    );

                _logger.LogInformation( $"Birthday message sent to {client.Name}" );
            }
        }

        public async Task TriggerRemindersAsync() {
            await SendRemindersAsync();
        }

        public async Task TriggerBirthdayMessagesAsync() {
            await SendBirthdayMessagesAsync();
        }
    }
}
