using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class yos : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_YosInfos_Consents_ConsentId",
                table: "YosInfos");

            migrationBuilder.DropIndex(
                name: "IX_YosInfos_ConsentId",
                table: "YosInfos");

            migrationBuilder.DropColumn(
                name: "ConsentId",
                table: "YosInfos");

            migrationBuilder.DropColumn(
                name: "LastValidAccessDate",
                table: "YosInfos");

            migrationBuilder.DropColumn(
                name: "TransactionInquiryEndTime",
                table: "YosInfos");

            migrationBuilder.DropColumn(
                name: "TransactionInquiryStartTime",
                table: "YosInfos");

            migrationBuilder.RenameColumn(
                name: "PermissionType",
                table: "YosInfos",
                newName: "marka");

            migrationBuilder.RenameColumn(
                name: "AccountReference",
                table: "YosInfos",
                newName: "kod");

            migrationBuilder.AddColumn<string>(
                name: "acikAnahtar",
                table: "YosInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<List<string>>(
                name: "adresler",
                table: "YosInfos",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "logoBilgileri",
                table: "YosInfos",
                type: "text[]",
                nullable: false);

            migrationBuilder.AddColumn<List<string>>(
                name: "roller",
                table: "YosInfos",
                type: "text[]",
                nullable: false);

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

            migrationBuilder.DropColumn(
                name: "acikAnahtar",
                table: "YosInfos");

            migrationBuilder.DropColumn(
                name: "adresler",
                table: "YosInfos");

            migrationBuilder.DropColumn(
                name: "logoBilgileri",
                table: "YosInfos");

            migrationBuilder.DropColumn(
                name: "roller",
                table: "YosInfos");

            migrationBuilder.RenameColumn(
                name: "marka",
                table: "YosInfos",
                newName: "PermissionType");

            migrationBuilder.RenameColumn(
                name: "kod",
                table: "YosInfos",
                newName: "AccountReference");

            migrationBuilder.AddColumn<Guid>(
                name: "ConsentId",
                table: "YosInfos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastValidAccessDate",
                table: "YosInfos",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionInquiryEndTime",
                table: "YosInfos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "TransactionInquiryStartTime",
                table: "YosInfos",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_YosInfos_ConsentId",
                table: "YosInfos",
                column: "ConsentId");

            migrationBuilder.AddForeignKey(
                name: "FK_YosInfos_Consents_ConsentId",
                table: "YosInfos",
                column: "ConsentId",
                principalTable: "Consents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
