using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DomainObjects.Migrations
{
    public partial class UpdateReservationTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ConfirmationNumber",
                table: "Reservations",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ConfirmationNumber",
                table: "Reservations",
                nullable: false,
                oldClrType: typeof(long));
        }
    }
}
