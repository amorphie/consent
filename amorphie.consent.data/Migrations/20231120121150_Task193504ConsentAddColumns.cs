using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task193504ConsentAddColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClientId",
                table: "Consents",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "RoleId",
                table: "Consents",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ScopeId",
                table: "Consents",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Variant",
                table: "Consents",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClientId",
                table: "Consents");

            migrationBuilder.DropColumn(
                name: "RoleId",
                table: "Consents");

            migrationBuilder.DropColumn(
                name: "ScopeId",
                table: "Consents");

            migrationBuilder.DropColumn(
                name: "Variant",
                table: "Consents");
        }
    }
}
