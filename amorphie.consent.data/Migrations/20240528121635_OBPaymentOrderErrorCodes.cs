using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class OBPaymentOrderErrorCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("32d7bc60-4ad3-4dc8-b1f3-3038d338748f"), "TR.OHVPS.Business.InvalidContent", 204, "Gkd yonadr does not match yos api yos addresses.", "YonAdr değeri hatalı. Yos api içerisinde belirtilen temel addresler ile uyuşmamaktadır." },
                    { new Guid("548f940e-300a-42f8-9347-542c4539d14f"), "TR.OHVPS.Field.Invalid", 36, "size must be between 3-140", "boyut '3' ile  '140' arasında olmalı" },
                    { new Guid("5fe3c8c2-8782-47bc-b22a-3491eb981ca9"), "TR.OHVPS.Field.Invalid", 39, "size must be between 1-200", "boyut '1' ile  '200' arasında olmalı" },
                    { new Guid("66a09286-7861-49b3-a6f0-1323ff5436a3"), "TR.OHVPS.Resource.NotFound", 120, "Related Payment consent can not found to process payment order.", "Ödeme emri yapılmak istenilen ilişkili ödeme emri rızası kaydı bulunamadı." },
                    { new Guid("6dc0633e-c857-4309-8b8e-86b2cf479e68"), "TR.OHVPS.Field.Invalid", 40, "size must be between 3-140", "boyut '3' ile  '140' arasında olmalı" },
                    { new Guid("704009c8-8057-49b5-a3eb-8e0b1402b4ce"), "TR.OHVPS.Field.Invalid", 41, "Incorrect or missing data.", "Alan verisi eksik ya da hatalı." },
                    { new Guid("77a67556-bf51-4c10-b70b-76f2ddd87d0b"), "TR.OHVPS.Field.Invalid", 38, "Numeric 12 length data", "boyur '12' sayısal karakter olmalı" },
                    { new Guid("e0a5e6cf-662d-4cc7-b171-131206ceb4c6"), "TR.OHVPS.Business.InvalidContent", 205, "kmlk.kmlkVrs does not match processing user tckn.", "İşlem yapan kullanıcının tckn si, rıza içerisindeki kimlik verisi ile uyuşmamaktadır." },
                    { new Guid("f20c8ab6-0642-4133-9a55-9399ac46807f"), "TR.OHVPS.Field.Invalid", 37, "size must be 26", "boyut '26' olmalı" },
                    { new Guid("f71142f9-e36d-4a65-8eb5-6116a4286aac"), "TR.OHVPS.Resource.ConsentMismatch", 165, "Consent state not valid to process", "Odeme emri rıza durumu, ödeme emri işlemi yapılmaya uygun değil." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("32d7bc60-4ad3-4dc8-b1f3-3038d338748f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("548f940e-300a-42f8-9347-542c4539d14f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("5fe3c8c2-8782-47bc-b22a-3491eb981ca9"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("66a09286-7861-49b3-a6f0-1323ff5436a3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("6dc0633e-c857-4309-8b8e-86b2cf479e68"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("704009c8-8057-49b5-a3eb-8e0b1402b4ce"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("77a67556-bf51-4c10-b70b-76f2ddd87d0b"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("e0a5e6cf-662d-4cc7-b171-131206ceb4c6"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f20c8ab6-0642-4133-9a55-9399ac46807f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f71142f9-e36d-4a65-8eb5-6116a4286aac"));
        }
    }
}
