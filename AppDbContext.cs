using Microsoft.EntityFrameworkCore;
using NailDesignerAPI.Models;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.ChangeTracking;

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

        public override async Task<int> SaveChangesAsync( CancellationToken cancellationToken = default ) {

            var auditEntries = new List<AuditLog>();
            var entries = ChangeTracker.Entries()
                .Where( e => e.Entity is not AuditLog &&
                             e.State is EntityState.Added or
                                        EntityState.Modified or
                                        EntityState.Deleted );

            foreach( var entry in entries ) {

                var audit = new AuditLog {
                    TableName = entry.Entity.GetType().Name,
                    RecordId = 0,
                    Action = entry.State switch {
                        EntityState.Added => AuditAction.Create,
                        EntityState.Modified => AuditAction.Update,
                        EntityState.Deleted => AuditAction.Delete,
                        _ => AuditAction.Update
                    },
                    OldValues = entry.State == EntityState.Modified ||
                                entry.State == EntityState.Deleted ?
                                JsonSerializer.Serialize( entry.OriginalValues.ToObject() ) : null,
                    NewValues = entry.State == EntityState.Added ||
                                entry.State == EntityState.Modified ?
                                JsonSerializer.Serialize( entry.CurrentValues.ToObject() ) : null,
                    ChangedAt = DateTime.Now,
                    ChangedBy = "System"
                };

                auditEntries.Add( audit );
            }

            var result = await base.SaveChangesAsync( cancellationToken );

            foreach( var audit in auditEntries ) {
                var entry = ChangeTracker.Entries()
                    .FirstOrDefault( e => e.Entity.GetType().Name == audit.TableName );

                if( entry != null )
                    audit.RecordId = (int)entry.Property( "Id" ).CurrentValue!;

                AuditLogs.Add( audit );
            }

            await base.SaveChangesAsync( cancellationToken );

            return result;
        }

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
