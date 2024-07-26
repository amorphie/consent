using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class MissingErrorCodeDetailsPR189 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
    
            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("83901c32-4b67-477e-9f7f-0735c168ad58"), "TR.OHVPS.Resource.InvalidSignature", 327, "X-JWS-Signature header is invalid.", "YOS ten gelen istekteki X-JWS-Signature basligi gecersiz." },
                    { new Guid("9e0041fc-af7d-4d01-873b-e9c9dd5a5656"), "TR.OHVPS.Server.InternalError", 153, "By validating header jwt property, body not set.", "Istek başlığında bulunda xjwtsignature alanı kontrol edilirken beklenmedik bir durumla karşılaşıldı. Body değeri set edilmemiş." },
                    { new Guid("c374640a-60f5-44ec-a919-e2856bdef3b5"), "TR.OHVPS.Resource.InvalidSignature", 326, "PSU-Fraud-Check header is invalid.", "YOS ten gelen istekteki PSU-Fraud-Check basligi gecersiz." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("83901c32-4b67-477e-9f7f-0735c168ad58"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("9e0041fc-af7d-4d01-873b-e9c9dd5a5656"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("c374640a-60f5-44ec-a919-e2856bdef3b5"));

        }
    }
}
