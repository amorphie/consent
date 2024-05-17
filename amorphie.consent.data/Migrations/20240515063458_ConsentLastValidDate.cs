using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class ConsentLastValidDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                 name: "LastValidAccessDate",
                 table: "OBAccountConsentDetails");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastValidAccessDate",
                table: "Consents",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.InsertData(
                  table: "OBErrorCodeDetails",
                  columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                  values: new object[,]
                  {
                            { new Guid("b6799e97-3a74-4e59-98e4-6c8230a0ec7e"), "TR.OHVPS.Business.InvalidContent", 203, "Invalid post message.", "İstek mesajı geçersiz." }
                  });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastValidAccessDate",
                table: "Consents");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastValidAccessDate",
                table: "OBAccountConsentDetails",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
            migrationBuilder.DeleteData(
                        table: "OBErrorCodeDetails",
                        keyColumn: "Id",
                        keyValue: new Guid("b6799e97-3a74-4e59-98e4-6c8230a0ec7e"));

        }
    }
}




