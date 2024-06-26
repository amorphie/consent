using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class ErrorCodeDetailsScriptCheckCustomer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("1915df4a-4bd5-43ed-97f0-a9002171a524"), "TR.OHVPS.Business.InvalidCustomerInfo", 213, "Unique customer information can not be found by given gsm/iban", "Verilmiş olan ohkTanimTip = GSM-IBAN ile sistemde kayıtlı tekil bir kullanıcıya erişilemedi." },
                    { new Guid("27dfa95e-e421-4041-b8bf-aae085304d2e"), "TR.OHVPS.Server.InternalError", 156, "By checking is unique customer by iban/gsm from customer service, service error occured.", "GSM/IBAN ile tekil müşteri bilgilerinin sorgulandığı servis sürecinde hata oluştu." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("1915df4a-4bd5-43ed-97f0-a9002171a524"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("27dfa95e-e421-4041-b8bf-aae085304d2e"));

        }
    }
}
