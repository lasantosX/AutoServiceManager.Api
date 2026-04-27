using AutoServiceManager.Api.Common;
using AutoServiceManager.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace AutoServiceManager.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
    public DbSet<Vehicle> Vehicles => Set<Vehicle>();
    public DbSet<Technician> Technicians => Set<Technician>();
    public DbSet<ServiceOrder> ServiceOrders => Set<ServiceOrder>();
    public DbSet<ServiceOrderOperation> ServiceOrderOperations => Set<ServiceOrderOperation>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        ConfigureCustomer(modelBuilder);
        ConfigureVehicle(modelBuilder);
        ConfigureTechnician(modelBuilder);
        ConfigureServiceOrder(modelBuilder);
        ConfigureServiceOrderOperation(modelBuilder);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    public override int SaveChanges()
    {
        ApplyAuditFields();
        return base.SaveChanges();
    }

    private void ApplyAuditFields()
    {
        var entries = ChangeTracker
            .Entries<AuditableEntity>()
            .Where(entry => entry.State == EntityState.Added || entry.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAtUtc = DateTime.UtcNow;
                entry.Entity.CreatedBy = "system";
            }

            if (entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedAtUtc = DateTime.UtcNow;
                entry.Entity.ModifiedBy = "system";
            }
        }
    }

    private static void ConfigureCustomer(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(x => x.CustomerId);

            entity.Property(x => x.FirstName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.LastName)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(150);

            entity.Property(x => x.Phone)
                .HasMaxLength(30);
        });
    }

    private static void ConfigureVehicle(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Vehicle>(entity =>
        {
            entity.HasKey(x => x.VehicleId);

            entity.Property(x => x.VIN)
                .HasMaxLength(17)
                .IsRequired();

            entity.Property(x => x.Make)
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(x => x.Model)
                .HasMaxLength(80)
                .IsRequired();

            entity.Property(x => x.PlateNumber)
                .HasMaxLength(20);

            entity.Property(x => x.UnitNumber)
                .HasMaxLength(20);

            entity.HasOne(x => x.Customer)
                .WithMany(x => x.Vehicles)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureTechnician(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Technician>(entity =>
        {
            entity.HasKey(x => x.TechnicianId);

            entity.Property(x => x.FullName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(150);
        });
    }

    private static void ConfigureServiceOrder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceOrder>(entity =>
        {
            entity.HasKey(x => x.ServiceOrderId);

            entity.Property(x => x.OrderNumber)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.TotalLaborAmount)
                .HasPrecision(18, 2);

            entity.Property(x => x.TotalPartsAmount)
                .HasPrecision(18, 2);

            entity.Property(x => x.TotalAmount)
                .HasPrecision(18, 2);

            entity.HasOne(x => x.Vehicle)
                .WithMany(x => x.ServiceOrders)
                .HasForeignKey(x => x.VehicleId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }

    private static void ConfigureServiceOrderOperation(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ServiceOrderOperation>(entity =>
        {
            entity.HasKey(x => x.ServiceOrderOperationId);

            entity.Property(x => x.OpCode)
                .HasMaxLength(30)
                .IsRequired();

            entity.Property(x => x.Description)
                .HasMaxLength(500)
                .IsRequired();

            entity.Property(x => x.LaborHours)
                .HasPrecision(18, 2);

            entity.Property(x => x.LaborRate)
                .HasPrecision(18, 2);

            entity.Property(x => x.LaborAmount)
                .HasPrecision(18, 2);

            entity.HasOne(x => x.ServiceOrder)
                .WithMany(x => x.Operations)
                .HasForeignKey(x => x.ServiceOrderId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.Technician)
                .WithMany(x => x.Operations)
                .HasForeignKey(x => x.TechnicianId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}