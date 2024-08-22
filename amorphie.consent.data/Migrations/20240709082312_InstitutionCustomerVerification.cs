using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class InstitutionCustomerVerification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
        
            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("1a91d250-60df-4348-8866-994386ab4cb2"), "TR.OHVPS.Server.InternalError", 400, "By checking is institution customer has authorization for open banking from verification service, handled service error occured.", "Kurumsal müşterinin açık bankacılık işlemi yapabilir mi yetkilerinin sorgulandığı servis sürecinde servis hatası oluştu." },
                    { new Guid("6991ee4e-fdcf-44f9-babf-86e36ce4fb52"), "TR.OHVPS.Business.InvalidAccount", 401, "Institution customer validation error. {0}", "Kurumsal müşteri yetkisiz işlem. Hata detayı: {0}" },
                    { new Guid("b4916a66-401f-4f1f-846c-367989385461"), "TR.OHVPS.Server.InternalError", 157, "By checking is institution customer has authorization for open banking from verification service, service error occured.", "Kurumsal müşterinin açık bankacılık işlemi yapabilir mi yetkilerinin sorgulandığı servis sürecinde hata oluştu." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("1a91d250-60df-4348-8866-994386ab4cb2"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("6991ee4e-fdcf-44f9-babf-86e36ce4fb52"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b4916a66-401f-4f1f-846c-367989385461"));
        }
    }
}
