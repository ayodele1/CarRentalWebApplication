using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DomainObjects;

namespace DomainObjects.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20161201230024_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.1.0-rtm-22752")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DomainObjects.Rental", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("PickupDate");

                    b.Property<DateTime>("PickupTime");

                    b.Property<DateTime>("ReturnDate");

                    b.Property<DateTime>("ReturnTime");

                    b.Property<double>("TotalCost");

                    b.Property<int>("VehicleId");

                    b.Property<int>("isActive");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Rentals");
                });

            modelBuilder.Entity("DomainObjects.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ConfirmationNumber");

                    b.Property<int>("CustomerId");

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<DateTime>("PickupDate");

                    b.Property<DateTime>("PickupTime");

                    b.Property<DateTime>("ReturnDate");

                    b.Property<DateTime>("ReturnTime");

                    b.Property<string>("StoreLocation");

                    b.Property<double>("TotalCost");

                    b.Property<int>("VehicleId");

                    b.HasKey("Id");

                    b.HasIndex("CustomerId");

                    b.HasIndex("VehicleId");

                    b.ToTable("Reservations");
                });

            modelBuilder.Entity("DomainObjects.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("DateCreated");

                    b.Property<DateTime>("DateModified");

                    b.Property<string>("Discriminator")
                        .IsRequired();

                    b.Property<string>("EmailAddress");

                    b.Property<string>("FirstName");

                    b.Property<string>("LastName");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasDiscriminator<string>("Discriminator").HasValue("User");
                });

            modelBuilder.Entity("DomainObjects.Vehicle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ImageName");

                    b.Property<int>("MakeYear");

                    b.Property<int>("ModelType");

                    b.Property<string>("Name");

                    b.Property<int>("PassengerCapacity");

                    b.Property<double>("PricePerDay");

                    b.Property<int>("isAvailable");

                    b.HasKey("Id");

                    b.ToTable("Vehicles");
                });

            modelBuilder.Entity("DomainObjects.Customer", b =>
                {
                    b.HasBaseType("DomainObjects.User");

                    b.Property<string>("PhoneNumber");

                    b.ToTable("Customer");

                    b.HasDiscriminator().HasValue("Customer");
                });

            modelBuilder.Entity("DomainObjects.CustomerRepresentative", b =>
                {
                    b.HasBaseType("DomainObjects.User");


                    b.ToTable("CustomerRepresentative");

                    b.HasDiscriminator().HasValue("CustomerRepresentative");
                });

            modelBuilder.Entity("DomainObjects.Rental", b =>
                {
                    b.HasOne("DomainObjects.Customer", "Customer")
                        .WithMany("Rentals")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DomainObjects.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("DomainObjects.Reservation", b =>
                {
                    b.HasOne("DomainObjects.Customer", "Customer")
                        .WithMany("Reservations")
                        .HasForeignKey("CustomerId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DomainObjects.Vehicle", "Vehicle")
                        .WithMany()
                        .HasForeignKey("VehicleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });
        }
    }
}
