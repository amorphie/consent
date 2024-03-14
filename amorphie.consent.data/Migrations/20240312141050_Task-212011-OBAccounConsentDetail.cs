using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task212011OBAccounConsentDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SendToServiceDeliveryStatus",
                table: "OBAccountConsentDetails",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "SendToServiceLastTryTime",
                table: "OBAccountConsentDetails",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SendToServiceTryCount",
                table: "OBAccountConsentDetails",
                type: "integer",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SendToServiceDeliveryStatus",
                table: "OBAccountConsentDetails");

            migrationBuilder.DropColumn(
                name: "SendToServiceLastTryTime",
                table: "OBAccountConsentDetails");

            migrationBuilder.DropColumn(
                name: "SendToServiceTryCount",
                table: "OBAccountConsentDetails");
        }
    }
}
