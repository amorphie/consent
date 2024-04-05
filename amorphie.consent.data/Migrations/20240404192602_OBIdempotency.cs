using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class OBIdempotency : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             migrationBuilder.InsertData(
                        table: "OBErrorCodeDetails",
                        columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                        values: new object[,]
                        {
                   { new Guid("d6b26fa2-1e43-4f75-b8f9-298ee2501b54"), "TR.OHVPS.Server.InternalError", 152, "By Checking Idempotency, unexpected condition was encountered.", "Idempotency kontrol edilirken beklenmeyen bir durumla karşılaşıldı." }
                        });


            migrationBuilder.AddColumn<DateTime>(
                name: "CheckSumLastValiDateTime",
                table: "OBPaymentOrders",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CheckSumValue",
                table: "OBPaymentOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SaveResponseMessage",
                table: "OBPaymentOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckSumLastValiDateTime",
                table: "OBPaymentConsentDetails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CheckSumValue",
                table: "OBPaymentConsentDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SaveResponseMessage",
                table: "OBPaymentConsentDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CheckSumLastValiDateTime",
                table: "OBAccountConsentDetails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "CheckSumValue",
                table: "OBAccountConsentDetails",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SaveResponseMessage",
                table: "OBAccountConsentDetails",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CheckSumLastValiDateTime",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "CheckSumValue",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "SaveResponseMessage",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "CheckSumLastValiDateTime",
                table: "OBPaymentConsentDetails");

            migrationBuilder.DropColumn(
                name: "CheckSumValue",
                table: "OBPaymentConsentDetails");

            migrationBuilder.DropColumn(
                name: "SaveResponseMessage",
                table: "OBPaymentConsentDetails");

            migrationBuilder.DropColumn(
                name: "CheckSumLastValiDateTime",
                table: "OBAccountConsentDetails");

            migrationBuilder.DropColumn(
                name: "CheckSumValue",
                table: "OBAccountConsentDetails");

            migrationBuilder.DropColumn(
                name: "SaveResponseMessage",
                table: "OBAccountConsentDetails");

        }
    }
}
