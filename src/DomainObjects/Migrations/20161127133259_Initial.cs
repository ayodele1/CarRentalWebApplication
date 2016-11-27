using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Metadata;

namespace DomainObjects.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    EmailAddress = table.Column<string>(nullable: true),
                    FirstName = table.Column<string>(nullable: true),
                    LastName = table.Column<string>(nullable: true),
                    PhoneNumber = table.Column<string>(nullable: true),
                    RentalId = table.Column<int>(nullable: true),
                    ReservationId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Image = table.Column<byte[]>(nullable: true),
                    MakeYear = table.Column<int>(nullable: false),
                    MyProperty = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    PassengerCapacity = table.Column<int>(nullable: false),
                    PricePerDay = table.Column<double>(nullable: false),
                    Type = table.Column<int>(nullable: false),
                    isAvailable = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Rentals",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CustomerId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    PickupDate = table.Column<DateTime>(nullable: false),
                    PickupTime = table.Column<DateTime>(nullable: false),
                    RentalId = table.Column<int>(nullable: true),
                    ReturnDate = table.Column<DateTime>(nullable: false),
                    ReturnTime = table.Column<DateTime>(nullable: false),
                    TotalCost = table.Column<double>(nullable: false),
                    VehicleId = table.Column<string>(nullable: true),
                    isActive = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rentals", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rentals_Users_RentalId",
                        column: x => x.RentalId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Rentals_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Reservations",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    ConfirmationNumber = table.Column<int>(nullable: false),
                    CustomerId = table.Column<int>(nullable: false),
                    DateCreated = table.Column<DateTime>(nullable: false),
                    DateModified = table.Column<DateTime>(nullable: false),
                    PickupDate = table.Column<DateTime>(nullable: false),
                    PickupTime = table.Column<DateTime>(nullable: false),
                    ReservationId = table.Column<int>(nullable: true),
                    ReturnDate = table.Column<DateTime>(nullable: false),
                    ReturnTime = table.Column<DateTime>(nullable: false),
                    TotalCost = table.Column<double>(nullable: false),
                    VehicleId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Reservations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Reservations_Users_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Reservations_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_RentalId",
                table: "Rentals",
                column: "RentalId");

            migrationBuilder.CreateIndex(
                name: "IX_Rentals_VehicleId",
                table: "Rentals",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_ReservationId",
                table: "Reservations",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_VehicleId",
                table: "Reservations",
                column: "VehicleId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Rentals");

            migrationBuilder.DropTable(
                name: "Reservations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
