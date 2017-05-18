using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Vans_SRMS_API.Database;
using Vans_SRMS_API.Utils;

namespace Vans_SRMS_API.Migrations
{
    [DbContext(typeof(SRMS_DbContext))]
    [Migration("20170424184427_LocationAuditChanges")]
    partial class LocationAuditChanges
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasDefaultSchema("SRMS")
                .HasAnnotation("Npgsql:PostgresExtension:uuid-ossp", "'uuid-ossp', '', ''")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn)
                .HasAnnotation("ProductVersion", "1.1.1");

            modelBuilder.Entity("Vans_SRMS_API.Models.Color", b =>
                {
                    b.Property<int>("ColorId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Hex")
                        .IsRequired();

                    b.Property<string>("Name")
                        .IsRequired();

                    b.HasKey("ColorId");

                    b.ToTable("Colors");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.CycleCount", b =>
                {
                    b.Property<long>("CycleCountId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("DeviceId");

                    b.Property<int>("LocationId");

                    b.Property<int>("StoreId");

                    b.HasKey("CycleCountId");

                    b.HasIndex("DeviceId");

                    b.HasIndex("LocationId");

                    b.HasIndex("StoreId");

                    b.ToTable("CycleCounts");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.CycleCountItem", b =>
                {
                    b.Property<long>("CycleCountItemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<long>("CycleCountId");

                    b.Property<int>("ProductId");

                    b.Property<DateTime>("Timestamp");

                    b.HasKey("CycleCountItemId");

                    b.HasIndex("CycleCountId");

                    b.HasIndex("ProductId");

                    b.ToTable("CycleCountItems");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.Device", b =>
                {
                    b.Property<int>("DeviceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("DeviceNumber")
                        .IsRequired();

                    b.HasKey("DeviceId");

                    b.ToTable("Devices");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.Location", b =>
                {
                    b.Property<int>("LocationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Barcode")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<bool>("FlaggedForAudit")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("FlaggedForCycleCount")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired();

                    b.Property<int>("StoreId");

                    b.HasKey("LocationId");

                    b.HasIndex("StoreId");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.LocationAudit", b =>
                {
                    b.Property<long>("LocationAuditId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("DeviceId");

                    b.Property<int>("LocationId");

                    b.Property<int>("StoreId");

                    b.HasKey("LocationAuditId");

                    b.HasIndex("DeviceId");

                    b.HasIndex("LocationId");

                    b.HasIndex("StoreId");

                    b.ToTable("LocationAudits");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.LocationAuditItem", b =>
                {
                    b.Property<long>("LocationAuditItemId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<long>("LocationAuditId");

                    b.Property<int>("OnRecordQuantity");

                    b.Property<int>("ProductId");

                    b.Property<int>("ScannedQuantity");

                    b.HasKey("LocationAuditItemId");

                    b.HasIndex("LocationAuditId");

                    b.HasIndex("ProductId");

                    b.ToTable("LocationAuditItems");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.Logs", b =>
                {
                    b.Property<long>("LogId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Application");

                    b.Property<string>("Callsite");

                    b.Property<string>("Exception");

                    b.Property<string>("Level");

                    b.Property<string>("Logger");

                    b.Property<string>("Message");

                    b.Property<string>("Timestamp");

                    b.HasKey("LogId");

                    b.ToTable("Logs");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.Pick", b =>
                {
                    b.Property<long>("PickId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("DeviceId");

                    b.Property<int>("LocationId");

                    b.Property<int>("ProductId");

                    b.Property<long>("RequestId");

                    b.Property<DateTime>("Timestamp")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.HasKey("PickId");

                    b.HasIndex("DeviceId");

                    b.HasIndex("LocationId");

                    b.HasIndex("ProductId");

                    b.HasIndex("RequestId");

                    b.ToTable("Picks");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.Product", b =>
                {
                    b.Property<int>("ProductId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Color")
                        .IsRequired();

                    b.Property<string>("Description");

                    b.Property<string>("GTIN")
                        .IsRequired();

                    b.Property<bool>("InStock")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValue(false);

                    b.Property<bool>("IsYouthSize");

                    b.Property<string>("ItemNumber")
                        .IsRequired();

                    b.Property<DateTime>("LastUpdate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<decimal>("Retail");

                    b.Property<string>("SKU");

                    b.Property<float>("Size");

                    b.Property<string>("Style")
                        .IsRequired();

                    b.Property<string>("VendorStyle")
                        .IsRequired();

                    b.HasKey("ProductId");

                    b.HasIndex("GTIN");

                    b.HasIndex("IsYouthSize");

                    b.HasIndex("VendorStyle");

                    b.ToTable("Products");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.ProductLocation", b =>
                {
                    b.Property<long>("ProductLocationId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<DateTime>("LastUpdate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<int>("LocationId");

                    b.Property<int>("ProductId");

                    b.Property<int>("Quantity");

                    b.HasKey("ProductLocationId");

                    b.HasIndex("LocationId");

                    b.HasIndex("ProductId");

                    b.ToTable("ProductLocations");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.Putaway", b =>
                {
                    b.Property<long>("PutawayId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<int>("DeviceId");

                    b.Property<int>("LocationId");

                    b.Property<int>("ProductId");

                    b.Property<DateTime>("Timestamp")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.HasKey("PutawayId");

                    b.HasIndex("DeviceId");

                    b.HasIndex("LocationId");

                    b.HasIndex("ProductId");

                    b.ToTable("Putaways");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.Request", b =>
                {
                    b.Property<long>("RequestId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<bool>("Active");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<int>("DeviceId");

                    b.Property<DateTime?>("FilledAt");

                    b.Property<int>("InStockQuantity");

                    b.Property<DateTime>("LastUpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<bool>("Picked");

                    b.Property<float>("SizeConverted");

                    b.Property<float>("SizeRequested");

                    b.Property<int>("SizeType");

                    b.Property<int>("StoreId");

                    b.Property<int>("SuggestedLocationId");

                    b.Property<string>("VendorStyle")
                        .IsRequired();

                    b.HasKey("RequestId");

                    b.HasIndex("DeviceId");

                    b.HasIndex("StoreId");

                    b.ToTable("Requests");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.Size", b =>
                {
                    b.Property<int>("SizeId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<float>("EURSize");

                    b.Property<bool>("Kids");

                    b.Property<float>("MEXSize");

                    b.Property<float>("UKSize");

                    b.Property<float>("USSize");

                    b.Property<float>("WomenSize");

                    b.HasKey("SizeId");

                    b.ToTable("Sizes");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.Store", b =>
                {
                    b.Property<int>("StoreId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<string>("Address1");

                    b.Property<string>("Address2");

                    b.Property<string>("City");

                    b.Property<string>("DistrictManager");

                    b.Property<string>("DistrictNumber");

                    b.Property<string>("Fax");

                    b.Property<bool>("IsDefault");

                    b.Property<DateTime>("LastUpdate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<string>("Name");

                    b.Property<string>("Phone");

                    b.Property<string>("Region");

                    b.Property<string>("RegionalDirector");

                    b.Property<string>("State");

                    b.Property<string>("StoreNumber");

                    b.Property<string>("Type");

                    b.Property<string>("Zip");

                    b.HasKey("StoreId");

                    b.ToTable("Stores");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.StoreDevice", b =>
                {
                    b.Property<int>("StoreDeviceId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.SerialColumn);

                    b.Property<bool>("Active");

                    b.Property<int>("ColorId");

                    b.Property<DateTime>("CreatedAt");

                    b.Property<int>("DeviceId");

                    b.Property<Guid>("DeviceKey")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTime>("LastUpdate")
                        .ValueGeneratedOnAdd()
                        .HasDefaultValueSql("now()");

                    b.Property<int>("StoreId");

                    b.HasKey("StoreDeviceId");

                    b.HasIndex("ColorId");

                    b.HasIndex("DeviceId");

                    b.HasIndex("StoreId");

                    b.ToTable("StoreDevices");
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.CycleCount", b =>
                {
                    b.HasOne("Vans_SRMS_API.Models.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.CycleCountItem", b =>
                {
                    b.HasOne("Vans_SRMS_API.Models.CycleCount", "CycleCount")
                        .WithMany()
                        .HasForeignKey("CycleCountId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.Location", b =>
                {
                    b.HasOne("Vans_SRMS_API.Models.Store", "Store")
                        .WithMany("Locations")
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.LocationAudit", b =>
                {
                    b.HasOne("Vans_SRMS_API.Models.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.LocationAuditItem", b =>
                {
                    b.HasOne("Vans_SRMS_API.Models.LocationAudit", "LocationAudit")
                        .WithMany()
                        .HasForeignKey("LocationAuditId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.Pick", b =>
                {
                    b.HasOne("Vans_SRMS_API.Models.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Request", "Request")
                        .WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.ProductLocation", b =>
                {
                    b.HasOne("Vans_SRMS_API.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.Putaway", b =>
                {
                    b.HasOne("Vans_SRMS_API.Models.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Location", "Location")
                        .WithMany()
                        .HasForeignKey("LocationId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Product", "Product")
                        .WithMany()
                        .HasForeignKey("ProductId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.Request", b =>
                {
                    b.HasOne("Vans_SRMS_API.Models.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("Vans_SRMS_API.Models.StoreDevice", b =>
                {
                    b.HasOne("Vans_SRMS_API.Models.Color", "Color")
                        .WithMany()
                        .HasForeignKey("ColorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Device", "Device")
                        .WithMany()
                        .HasForeignKey("DeviceId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("Vans_SRMS_API.Models.Store", "Store")
                        .WithMany()
                        .HasForeignKey("StoreId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
