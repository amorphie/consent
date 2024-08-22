using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class ConsentScopeClm : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.RenameColumn(
                        name: "ScopeTCKN",
                        table: "Consents",
                        newName: "Scope");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
           migrationBuilder.RenameColumn(
                    name: "Scope",
                    table: "Consents",
                    newName: "ScopeTCKN");

        }
    }
}
