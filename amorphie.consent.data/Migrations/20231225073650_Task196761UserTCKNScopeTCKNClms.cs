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
            migrationBuilder.AddColumn<string>(
                name: "ScopeTCKN",
                table: "Consents",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserTCKN",
                table: "Consents",
                type: "text",
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
