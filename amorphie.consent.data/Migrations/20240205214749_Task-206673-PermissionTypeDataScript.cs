using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task206673PermissionTypeDataScript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OBPermissionTypes",
                columns: new[] { "Id", "Code", "Description", "Language" },
                values: new object[,]
                {
                    { new Guid("1f087662-8852-4861-8177-3e08a578edc9"), "01", "Temel Hesap Bilgisi", "tr-TR" },
                    { new Guid("58377714-1a5c-48ba-b05a-dc367c7a3abe"), "02", "Ayrıntılı Hesap Bilgisi", "tr-TR" },
                    { new Guid("6d64db7b-eea7-46c5-b6a9-d1550057daf6"), "03", "Bakiye Bilgisi", "tr-TR" },
                    { new Guid("9d06d0b8-485c-429e-9ea8-62ba9556665b"), "04", "Temel İşlem (Hesap Hareketleri) Bilgisi", "tr-TR" },      
                    { new Guid("c346aa62-4e11-4b99-9ded-7cd1242ccc0b"), "05", "Ayrıntılı İşlem Bilgisi", "tr-TR" },
                    { new Guid("a00598a9-ed10-4912-890e-5d34843b9656"), "06", "Anlık Bakiye Bildirimi", "tr-TR" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBPermissionTypes",
                keyColumn: "Id",
                keyValue: new Guid("1f087662-8852-4861-8177-3e08a578edc9"));

            migrationBuilder.DeleteData(
                table: "OBPermissionTypes",
                keyColumn: "Id",
                keyValue: new Guid("58377714-1a5c-48ba-b05a-dc367c7a3abe"));

            migrationBuilder.DeleteData(
                table: "OBPermissionTypes",
                keyColumn: "Id",
                keyValue: new Guid("6d64db7b-eea7-46c5-b6a9-d1550057daf6"));

            migrationBuilder.DeleteData(
                table: "OBPermissionTypes",
                keyColumn: "Id",
                keyValue: new Guid("9d06d0b8-485c-429e-9ea8-62ba9556665b"));

            migrationBuilder.DeleteData(
                table: "OBPermissionTypes",
                keyColumn: "Id",
                keyValue: new Guid("a00598a9-ed10-4912-890e-5d34843b9656"));

            migrationBuilder.DeleteData(
                table: "OBPermissionTypes",
                keyColumn: "Id",
                keyValue: new Guid("c346aa62-4e11-4b99-9ded-7cd1242ccc0b"));
        }
    }
}
