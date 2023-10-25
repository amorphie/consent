using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task189061ConsentIdentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OBConsentIdentityInfos",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsentId = table.Column<Guid>(type: "uuid", nullable: false),
                    IdentityType = table.Column<string>(type: "text", nullable: false),
                    IdentityData = table.Column<string>(type: "text", nullable: false),
                    InstitutionIdentityType = table.Column<string>(type: "text", nullable: true),
                    InstitutionIdentityData = table.Column<string>(type: "text", nullable: true),
                    UserType = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBConsentIdentityInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBConsentIdentityInfos_Consents_ConsentId",
                        column: x => x.ConsentId,
                        principalTable: "Consents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OBConsentIdentityInfos_ConsentId",
                table: "OBConsentIdentityInfos",
                column: "ConsentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OBConsentIdentityInfos");
        }
    }
}
