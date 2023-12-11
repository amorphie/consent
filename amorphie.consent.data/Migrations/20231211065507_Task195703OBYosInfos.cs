using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task195703OBYosInfos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "YosInfos");

            migrationBuilder.CreateTable(
                name: "OBYosInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    roller = table.Column<List<string>>(type: "text[]", nullable: false),
                    kod = table.Column<string>(type: "text", nullable: false),
                    unv = table.Column<string>(type: "text", nullable: false),
                    marka = table.Column<string>(type: "text", nullable: false),
                    acikAnahtar = table.Column<string>(type: "text", nullable: false),
                    adresler = table.Column<List<string>>(type: "text[]", nullable: false),
                    logoBilgileri = table.Column<List<string>>(type: "text[]", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBYosInfos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OBYosInfos");

            migrationBuilder.CreateTable(
                name: "YosInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    acikAnahtar = table.Column<string>(type: "text", nullable: false),
                    adresler = table.Column<List<string>>(type: "text[]", nullable: false),
                    kod = table.Column<string>(type: "text", nullable: false),
                    logoBilgileri = table.Column<List<string>>(type: "text[]", nullable: false),
                    marka = table.Column<string>(type: "text", nullable: false),
                    roller = table.Column<List<string>>(type: "text[]", nullable: false),
                    unv = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_YosInfos", x => x.Id);
                });
        }
    }
}
