using Microsoft.EntityFrameworkCore;
using Npgsql.EntityFrameworkCore.PostgreSQL;
using Vans_SRMS_API.Models;

namespace Vans_SRMS_API.Database
{
    public class SRMS_DbContext : DbContext
    {
        public SRMS_DbContext(DbContextOptions<SRMS_DbContext> options)
            : base(options)
        {
        }

        public DbSet<Color> Colors { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<Pick> Picks { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductLocation> ProductLocations { get; set; }
        public DbSet<Putaway> Putaways { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Size> Sizes { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<StoreDevice> StoreDevices { get; set; }
        public DbSet<Logs> Logs { get; set; }
        public DbSet<CycleCount> CycleCounts { get; set; }
        public DbSet<CycleCountItem>  CycleCountItems { get; set; }
        public DbSet<LocationAudit> LocationAudits { get; set; }
        public DbSet<LocationAuditItem> LocationAuditItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ItemMasterImportLog> ItemMasterImports { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema("SRMS");
            modelBuilder.HasPostgresExtension("uuid-ossp");

            modelBuilder.Entity<CycleCountItem>()
                .Property(c => c.Timestamp)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Device>()
                .Property(d => d.CreatedAt)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Location>()
                .Property(l => l.FlaggedForCycleCount)
                .HasDefaultValue(false);
            modelBuilder.Entity<Location>()
                .Property(l => l.FlaggedForAudit)
                .HasDefaultValue(false);

            modelBuilder.Entity<Order>()
                .Property(d => d.LastUpdatedAt)
                .HasDefaultValueSql("now()");
            modelBuilder.Entity<Order>()
                .Property(d => d.OrderedAt)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Product>()
                .Property(d => d.LastUpdate)
                .HasDefaultValueSql("now()");
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.GTIN);
            modelBuilder.Entity<Product>()
                .HasIndex(p => p.VendorStyle);
            modelBuilder.Entity<Product>()
                .Property(p => p.InStock)
                .HasDefaultValue(false);
            
            modelBuilder.Entity<Pick>()
                .Property(p => p.Timestamp)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Product>()
                .Property(p => p.LastUpdate)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<ProductLocation>()
                .Property(p => p.LastUpdatedAt)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Putaway>()
                .Property(p => p.Timestamp)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Request>()
                .Property(p => p.RequestedAt)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<Store>()
                .Property(s => s.LastUpdate)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<StoreDevice>()
                .Property(sd => sd.DeviceKey)
                .HasDefaultValueSql("uuid_generate_v4()");
            modelBuilder.Entity<StoreDevice>()
                .Property(sd => sd.LastUpdate)
                .HasDefaultValueSql("now()");

            modelBuilder.Entity<ItemMasterImportLog>()
                .Property(l => l.StartedAt)
                .HasDefaultValueSql("now()");
        }

    }
}
