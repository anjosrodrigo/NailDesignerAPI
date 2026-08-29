using Microsoft.EntityFrameworkCore;
using NailDesignerAPI.Models;

namespace NailDesignerAPI {
    public class AppDbContext : DbContext {
        public AppDbContext( DbContextOptions<AppDbContext> options ) : base( options ) { }

        // Tables
        public DbSet<Client> Clients { get; set; }
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<ServiceAddOn> ServiceAddOns { get; set; }
        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<AppointmentAddOn> AppointmentAddOns { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating( ModelBuilder modelBuilder ) {
            // Client
            modelBuilder.Entity<Client>(
                entity => {
                    entity.HasKey( c => c.Id );
                    entity.Property( c => c.Name ).IsRequired().HasMaxLength( 100 );
                    entity.Property( c => c.Phone ).IsRequired().HasMaxLength( 20 );
                }
            );

            // ServiceType
            modelBuilder.Entity<ServiceType>(
                entity => {
                    entity.HasKey( s => s.Id );
                    entity.Property( s => s.Name ).IsRequired().HasMaxLength( 100 );
                    entity.Property( s => s.Price ).IsRequired();
                    entity.Property( s => s.DurationMinutes ).IsRequired();
                }
            );

            // ServiceAddOn
            modelBuilder.Entity<ServiceAddOn>(
                entity => {
                    entity.HasKey( s => s.Id );
                    entity.Property( s => s.Name ).IsRequired().HasMaxLength( 100 );
                    entity.Property( s => s.PricePerUnit ).IsRequired();
                    entity.Property( s => s.PriceAll ).IsRequired();
                }
            );

            // Appointment
            modelBuilder.Entity<Appointment>(
                entity => {
                    entity.HasKey( a => a.Id );
                    entity.HasOne( a => a.Client )
                          .WithMany()
                          .HasForeignKey( a => a.ClientId );
                    entity.HasOne( a => a.ServiceType )
                          .WithMany()
                          .HasForeignKey( a => a.ServiceTypeId );
                }
            );

            // AppointmentAddOn
            modelBuilder.Entity<AppointmentAddOn>(
                entity => {
                    entity.HasKey( a => a.Id );
                    entity.HasOne( a => a.Appointment )
                          .WithMany( a => a.AddOns )
                          .HasForeignKey( a => a.AppointmentId )
                          .OnDelete( DeleteBehavior.Cascade );
                    entity.HasOne( a => a.ServiceAddOn )
                          .WithMany()
                          .HasForeignKey( a => a.ServiceAddOnId )
                          .OnDelete( DeleteBehavior.Cascade );
                }
            );

            base.OnModelCreating( modelBuilder );
        }
    }
}
