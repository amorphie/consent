using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class hhsyostable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "unv",
                table: "OBYosInfos",
                newName: "Unv");

            migrationBuilder.RenameColumn(
                name: "roller",
                table: "OBYosInfos",
                newName: "Roller");

            migrationBuilder.RenameColumn(
                name: "marka",
                table: "OBYosInfos",
                newName: "Marka");

            migrationBuilder.RenameColumn(
                name: "logoBilgileri",
                table: "OBYosInfos",
                newName: "LogoBilgileri");

            migrationBuilder.RenameColumn(
                name: "kod",
                table: "OBYosInfos",
                newName: "Kod");

            migrationBuilder.RenameColumn(
                name: "adresler",
                table: "OBYosInfos",
                newName: "Adresler");

            migrationBuilder.RenameColumn(
                name: "acikAnahtar",
                table: "OBYosInfos",
                newName: "AcikAnahtar");

            migrationBuilder.AlterColumn<string>(
                name: "LogoBilgileri",
                table: "OBYosInfos",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AlterColumn<string>(
                name: "Adresler",
                table: "OBYosInfos",
                type: "text",
                nullable: false,
                oldClrType: typeof(List<string>),
                oldType: "text[]");

            migrationBuilder.AddColumn<string>(
                name: "ApiBilgileri",
                table: "OBYosInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Durum",
                table: "OBYosInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OBHhsInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Kod = table.Column<string>(type: "text", nullable: false),
                    Unv = table.Column<string>(type: "text", nullable: false),
                    Marka = table.Column<string>(type: "text", nullable: false),
                    AcikAnahtar = table.Column<string>(type: "text", nullable: false),
                    ApiBilgileri = table.Column<string>(type: "text", nullable: false),
                    LogoBilgileri = table.Column<string>(type: "text", nullable: false),
                    AyrikGKD = table.Column<string>(type: "text", nullable: true),
                    Durum = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBHhsInfos", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OBHhsInfos");

            migrationBuilder.DropColumn(
                name: "ApiBilgileri",
                table: "OBYosInfos");

            migrationBuilder.DropColumn(
                name: "Durum",
                table: "OBYosInfos");

            migrationBuilder.RenameColumn(
                name: "Unv",
                table: "OBYosInfos",
                newName: "unv");

            migrationBuilder.RenameColumn(
                name: "Roller",
                table: "OBYosInfos",
                newName: "roller");

            migrationBuilder.RenameColumn(
                name: "Marka",
                table: "OBYosInfos",
                newName: "marka");

            migrationBuilder.RenameColumn(
                name: "LogoBilgileri",
                table: "OBYosInfos",
                newName: "logoBilgileri");

            migrationBuilder.RenameColumn(
                name: "Kod",
                table: "OBYosInfos",
                newName: "kod");

            migrationBuilder.RenameColumn(
                name: "Adresler",
                table: "OBYosInfos",
                newName: "adresler");

            migrationBuilder.RenameColumn(
                name: "AcikAnahtar",
                table: "OBYosInfos",
                newName: "acikAnahtar");

            migrationBuilder.AlterColumn<List<string>>(
                name: "logoBilgileri",
                table: "OBYosInfos",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<List<string>>(
                name: "adresler",
                table: "OBYosInfos",
                type: "text[]",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }
    }
}
