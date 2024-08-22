using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class OBOBPaymentConsentDetaiReceiverNullableClms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverTitle",
                table: "OBPaymentConsentDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverAccountNumber",
                table: "OBPaymentConsentDetails",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("1ec5862f-0bb4-419e-8f2a-3fbb73e075c5"), "TR.OHVPS.Field.Invalid", 44, "Gon Unv should not be sent in One Time Payment", "Gon Unv alanı tek seferlik ödeme de gönderilmemelidir/boş olmalıdır." },
                    { new Guid("24636f96-4a2e-4fbf-969d-609828e65a6c"), "TR.OHVPS.Field.Invalid", 42, "GSM/IBAN can only be used in One Time Payment", "GSM/IBAN Ohk Tanım Tipi sadece tek seferlik ödeme de kullanılabilir." },
                    { new Guid("af7233fd-b683-4dca-946a-5593606c1698"), "TR.OHVPS.Field.Invalid", 43, "In One Time Payment, ohkTur must be individual.", "Tek seferlik ödeme de ohkTur sadece bireysel olabilir." },
                    { new Guid("d103cef6-e1d3-4abf-a9c1-ce591ded0161"), "TR.OHVPS.Business.InvalidContent", 328, "Kolas can not be used in one time payment.", "Tek seferlik ödeme işlemlerinde kolas kullanılamaz." },
                    { new Guid("d60b275b-a5e7-4ad9-8920-0524b29b5cb5"), "TR.OHVPS.Field.Invalid", 45, "When ohkTur is individual, institution data should not be in request data.", "Ohk Tur bireysel olan rızalarda, kurumsal kimlik bilgileri gönderilmemelidir." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("1ec5862f-0bb4-419e-8f2a-3fbb73e075c5"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("24636f96-4a2e-4fbf-969d-609828e65a6c"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("af7233fd-b683-4dca-946a-5593606c1698"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("d103cef6-e1d3-4abf-a9c1-ce591ded0161"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("d60b275b-a5e7-4ad9-8920-0524b29b5cb5"));

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverTitle",
                table: "OBPaymentConsentDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ReceiverAccountNumber",
                table: "OBPaymentConsentDetails",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

        }
    }
}
