using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class MyFirstMigration1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Consents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                table: "Consents");
        }
    }
}
