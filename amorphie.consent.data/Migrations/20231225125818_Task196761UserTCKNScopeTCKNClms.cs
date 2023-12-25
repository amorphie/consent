using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task196761UserTCKNScopeTCKNClms : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ScopeTCKN",
                table: "Consents",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "UserTCKN",
                table: "Consents",
                type: "bigint",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ScopeTCKN",
                table: "Consents");

            migrationBuilder.DropColumn(
                name: "UserTCKN",
                table: "Consents");
        }
    }
}
