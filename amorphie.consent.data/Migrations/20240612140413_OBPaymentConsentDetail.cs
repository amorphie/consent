using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class OBPaymentConsentDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AddColumn<string>(
                name: "CustomerNumber",
                table: "OBPaymentConsentDetails",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InstitutionCustomerNumber",
                table: "OBPaymentConsentDetails",
                type: "text",
                nullable: true);

            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("a4fe0111-a842-4a02-aa81-41e8c0eb622d"), "TR.OHVPS.Business.InvalidContent", 330, "There is no customer in the system with given kmlk data", "Kmlk alanında girilen bilgiler ile müşteri bulunamamıştır." },
                    { new Guid("bd68012f-0ca9-4ed2-9704-7212938896bc"), "TR.OHVPS.Server.InternalError", 155, "By checking customer information from customer service, service error occured.", "Müşteri bilgilerinin sorgulandığı servis sürecinde hata oluştu." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("a4fe0111-a842-4a02-aa81-41e8c0eb622d"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("bd68012f-0ca9-4ed2-9704-7212938896bc"));

            migrationBuilder.DropColumn(
                name: "CustomerNumber",
                table: "OBPaymentConsentDetails");

            migrationBuilder.DropColumn(
                name: "InstitutionCustomerNumber",
                table: "OBPaymentConsentDetails");

        }
    }
}
