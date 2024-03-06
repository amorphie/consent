using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task209367Event : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OBEventItems");

            migrationBuilder.DropColumn(
                name: "IsUndeliverable",
                table: "OBEvents");

            migrationBuilder.AddColumn<int>(
                name: "DeliveryStatus",
                table: "OBEvents",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "EventDate",
                table: "OBEvents",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EventNumber",
                table: "OBEvents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "EventType",
                table: "OBEvents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceNumber",
                table: "OBEvents",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SourceType",
                table: "OBEvents",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DeliveryStatus",
                table: "OBEvents");

            migrationBuilder.DropColumn(
                name: "EventDate",
                table: "OBEvents");

            migrationBuilder.DropColumn(
                name: "EventNumber",
                table: "OBEvents");

            migrationBuilder.DropColumn(
                name: "EventType",
                table: "OBEvents");

            migrationBuilder.DropColumn(
                name: "SourceNumber",
                table: "OBEvents");

            migrationBuilder.DropColumn(
                name: "SourceType",
                table: "OBEvents");

            migrationBuilder.AddColumn<bool>(
                name: "IsUndeliverable",
                table: "OBEvents",
                type: "boolean",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OBEventItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OBEventId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    EventDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    EventNumber = table.Column<string>(type: "text", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    SourceNumber = table.Column<string>(type: "text", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBEventItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBEventItems_OBEvents_OBEventId",
                        column: x => x.OBEventId,
                        principalTable: "OBEvents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "OBPermissionTypes",
                columns: new[] { "Id", "Code", "Description", "Language" },
                values: new object[,]
                {
                    { new Guid("1f087662-8852-4861-8177-3e08a578edc9"), "01", "Temel Hesap Bilgisi", "tr-TR" },
                    { new Guid("58377714-1a5c-48ba-b05a-dc367c7a3abe"), "02", "Ayrıntılı Hesap Bilgisi", "tr-TR" },
                    { new Guid("6d64db7b-eea7-46c5-b6a9-d1550057daf6"), "03", "Bakiye Bilgisi", "tr-TR" },
                    { new Guid("9d06d0b8-485c-429e-9ea8-62ba9556665b"), "04", "Temel İşlem (Hesap Hareketleri) Bilgisi", "tr-TR" },
                    { new Guid("a00598a9-ed10-4912-890e-5d34843b9656"), "06", "Anlık Bakiye Bildirimi", "tr-TR" },
                    { new Guid("c346aa62-4e11-4b99-9ded-7cd1242ccc0b"), "05", "Ayrıntılı İşlem Bilgisi", "tr-TR" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_OBEventItems_OBEventId",
                table: "OBEventItems",
                column: "OBEventId");
        }
    }
}
