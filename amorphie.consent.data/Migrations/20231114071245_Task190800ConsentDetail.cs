using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task190800ConsentDetail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ConsentDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsentId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    State = table.Column<string>(type: "text", nullable: false),
                    ConsentDetailType = table.Column<string>(type: "text", nullable: false),
                    AdditionalData = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsentDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsentDetails_Consents_ConsentId",
                        column: x => x.ConsentId,
                        principalTable: "Consents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsentDetails_ConsentId",
                table: "ConsentDetails",
                column: "ConsentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsentDetails");
        }
    }
}
