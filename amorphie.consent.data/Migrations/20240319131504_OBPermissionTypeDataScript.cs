using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class OBPermissionTypeDataScript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.Sql("TRUNCATE TABLE \"OBPermissionTypes\";");

            migrationBuilder.InsertData(
                table: "OBPermissionTypes",
                columns: new[] { "Id", "Code", "Description", "GroupId", "GroupName", "Language" },
                values: new object[,]
                {
                    { new Guid("4c1d6d12-572e-4d73-b9a9-b4bd8debf104"), "02", "Ayrıntılı Hesap Bilgisi", 1, "Hesap Bilgileri", "tr-TR" },
                    { new Guid("562e1b35-a1de-4ac7-8a23-379bfbd8cd7e"), "06", "Anlık Bakiye Bildirimi", 2, "Hesap Bakiyesi", "tr-TR" },
                    { new Guid("5b833776-34d5-4b32-9f82-6c5cb701e064"), "01", "Temel Hesap Bilgisi", 1, "Hesap Bilgileri", "tr-TR" },
                    { new Guid("75fc6e81-c293-4107-92eb-31b6bd507405"), "03", "Bakiye Bilgisi", 2, "Hesap Bakiyesi", "tr-TR" },
                    { new Guid("7caa7d28-587a-4922-86f9-f897c3b9cd07"), "05", "Ayrıntılı İşlem Bilgisi", 3, "Hesap Hareketleri", "tr-TR" },
                    { new Guid("9c98a0b7-e568-4acc-9bb2-a15ecb3c2129"), "04", "Temel İşlem (Hesap Hareketleri) Bilgisi", 3, "Hesap Hareketleri", "tr-TR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBPermissionTypes",
                keyColumn: "Id",
                keyValue: new Guid("4c1d6d12-572e-4d73-b9a9-b4bd8debf104"));

            migrationBuilder.DeleteData(
                table: "OBPermissionTypes",
                keyColumn: "Id",
                keyValue: new Guid("562e1b35-a1de-4ac7-8a23-379bfbd8cd7e"));

            migrationBuilder.DeleteData(
                table: "OBPermissionTypes",
                keyColumn: "Id",
                keyValue: new Guid("5b833776-34d5-4b32-9f82-6c5cb701e064"));

            migrationBuilder.DeleteData(
                table: "OBPermissionTypes",
                keyColumn: "Id",
                keyValue: new Guid("75fc6e81-c293-4107-92eb-31b6bd507405"));

            migrationBuilder.DeleteData(
                table: "OBPermissionTypes",
                keyColumn: "Id",
                keyValue: new Guid("7caa7d28-587a-4922-86f9-f897c3b9cd07"));

            migrationBuilder.DeleteData(
                table: "OBPermissionTypes",
                keyColumn: "Id",
                keyValue: new Guid("9c98a0b7-e568-4acc-9bb2-a15ecb3c2129"));
        }
    }
}
