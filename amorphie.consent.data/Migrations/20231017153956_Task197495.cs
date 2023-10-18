using System;
using Microsoft.EntityFrameworkCore.Migrations;
using NpgsqlTypes;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task197495 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ConsentDefinitions");

            migrationBuilder.DropTable(
                name: "ConsentPermissions");

            migrationBuilder.DropColumn(
                name: "ConsentDefinitionId",
                table: "Consents");

            migrationBuilder.CreateTable(
                name: "OBAccountReferences",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AccountReference = table.Column<string>(type: "text", nullable: false),
                    PermissionType = table.Column<string>(type: "text", nullable: false),
                    LastValidAccessDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TransactionInquiryStartTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    TransactionInquiryEndTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBAccountReferences", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBAccountReferences_Consents_ConsentId",
                        column: x => x.ConsentId,
                        principalTable: "Consents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OBAccountReferences_ConsentId",
                table: "OBAccountReferences",
                column: "ConsentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OBAccountReferences");

            migrationBuilder.AddColumn<Guid>(
                name: "ConsentDefinitionId",
                table: "Consents",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ConsentDefinitions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ClientId = table.Column<string[]>(type: "text[]", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: false),
                    RoleAssignment = table.Column<string>(type: "text", nullable: false),
                    Scope = table.Column<string[]>(type: "text[]", nullable: false),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false, computedColumnSql: "to_tsvector('english', coalesce(\"Name\", '') || ' ' || coalesce(\"RoleAssignment\", ''))", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsentDefinitions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ConsentPermissions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    Permission = table.Column<string>(type: "text", nullable: false),
                    PermissionLastDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    SearchVector = table.Column<NpgsqlTsVector>(type: "tsvector", nullable: false, computedColumnSql: "to_tsvector('english', coalesce(\"Permission\", ''))", stored: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConsentPermissions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ConsentPermissions_Consents_ConsentId",
                        column: x => x.ConsentId,
                        principalTable: "Consents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ConsentDefinitions_SearchVector",
                table: "ConsentDefinitions",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");

            migrationBuilder.CreateIndex(
                name: "IX_ConsentPermissions_ConsentId",
                table: "ConsentPermissions",
                column: "ConsentId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConsentPermissions_SearchVector",
                table: "ConsentPermissions",
                column: "SearchVector")
                .Annotation("Npgsql:IndexMethod", "GIN");
        }
    }
}
