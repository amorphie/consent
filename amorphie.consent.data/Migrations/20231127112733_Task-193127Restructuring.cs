using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task193127Restructuring : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TABLE IF EXISTS public.\"ConsentDefinitions\"");
            migrationBuilder.Sql("DROP TABLE IF EXISTS public.\"ConsentPermissions\"");
            migrationBuilder.Sql("ALTER TABLE public.\"Consents\" DROP COLUMN IF EXISTS \"ConsentDefinitionId\"");

            migrationBuilder.DropTable(
                name: "ConsentDetails");

            migrationBuilder.DropColumn(
                name: "AccountReference",
                table: "OBAccountReferences");

            migrationBuilder.DropColumn(
                name: "PermissionType",
                table: "OBAccountReferences");

            migrationBuilder.AddColumn<List<string>>(
                name: "AccountReferences",
                table: "OBAccountReferences",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "PermissionTypes",
                table: "OBAccountReferences",
                type: "text[]",
                nullable: false);

            migrationBuilder.CreateTable(
                name: "OBPaymentOrders",
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
                    table.PrimaryKey("PK_OBPaymentOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBPaymentOrders_Consents_ConsentId",
                        column: x => x.ConsentId,
                        principalTable: "Consents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OBPaymentOrders_ConsentId",
                table: "OBPaymentOrders",
                column: "ConsentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "AccountReferences",
                table: "OBAccountReferences");

            migrationBuilder.DropColumn(
                name: "PermissionTypes",
                table: "OBAccountReferences");

            migrationBuilder.AddColumn<string>(
                name: "AccountReference",
                table: "OBAccountReferences",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PermissionType",
                table: "OBAccountReferences",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "ConsentDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ConsentId = table.Column<Guid>(type: "uuid", nullable: false),
                    AdditionalData = table.Column<string>(type: "text", nullable: false),
                    ConsentDetailType = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    State = table.Column<string>(type: "text", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true)
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
    }
}
