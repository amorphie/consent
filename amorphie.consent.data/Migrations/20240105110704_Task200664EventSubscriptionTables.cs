using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task200664EventSubscriptionTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OBEventSubscriptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubscriptionNumber = table.Column<Guid>(type: "uuid", nullable: false),
                    HHSCode = table.Column<string>(type: "text", nullable: false),
                    YOSCode = table.Column<string>(type: "text", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBEventSubscriptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OBEventTypeSourceTypeRelations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    YOSRole = table.Column<string>(type: "text", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    EventCase = table.Column<string>(type: "text", nullable: false),
                    SourceNumber = table.Column<string>(type: "text", nullable: false),
                    APIToGetData = table.Column<string>(type: "text", nullable: false),
                    EventNotificationReporter = table.Column<string>(type: "text", nullable: false),
                    EventNotificationTime = table.Column<string>(type: "text", nullable: false),
                    RetryPolicy = table.Column<string>(type: "text", nullable: true),
                    RetryInMinute = table.Column<int>(type: "integer", nullable: true),
                    RetryCount = table.Column<int>(type: "integer", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBEventTypeSourceTypeRelations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "OBEventSubscriptionTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OBEventSubscriptionId = table.Column<Guid>(type: "uuid", nullable: false),
                    EventType = table.Column<string>(type: "text", nullable: false),
                    SourceType = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    CreatedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true),
                    ModifiedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    ModifiedBy = table.Column<Guid>(type: "uuid", nullable: false),
                    ModifiedByBehalfOf = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OBEventSubscriptionTypes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OBEventSubscriptionTypes_OBEventSubscriptions_OBEventSubscr~",
                        column: x => x.OBEventSubscriptionId,
                        principalTable: "OBEventSubscriptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OBEventSubscriptionTypes_OBEventSubscriptionId",
                table: "OBEventSubscriptionTypes",
                column: "OBEventSubscriptionId");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OBEventSubscriptionTypes");

            migrationBuilder.DropTable(
                name: "OBEventTypeSourceTypeRelations");

            migrationBuilder.DropTable(
                name: "OBEventSubscriptions");
        }
    }
}
