using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task200595PaymentOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ConsentDetailType",
                table: "OBPaymentOrders",
                newName: "YosCode");

            migrationBuilder.AddColumn<string>(
                name: "Amount",
                table: "OBPaymentOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "OBPaymentOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "ExpectedPaymentDate",
                table: "OBPaymentOrders",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GeneralWorkplaceNumber",
                table: "OBPaymentOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HhsCode",
                table: "OBPaymentOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "OHKMessage",
                table: "OBPaymentOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PSNDate",
                table: "OBPaymentOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PSNRefNum",
                table: "OBPaymentOrders",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PSNYosCode",
                table: "OBPaymentOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentDescription",
                table: "OBPaymentOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentPurpose",
                table: "OBPaymentOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentServiceUpdateTime",
                table: "OBPaymentOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PaymentSource",
                table: "OBPaymentOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentState",
                table: "OBPaymentOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentSystem",
                table: "OBPaymentOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "PaymentSystemNumber",
                table: "OBPaymentOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ReferenceInformation",
                table: "OBPaymentOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubWorkplaceCategoryCode",
                table: "OBPaymentOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WorkplaceCategoryCode",
                table: "OBPaymentOrders",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "XGroupId",
                table: "OBPaymentOrders",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "XRequestId",
                table: "OBPaymentOrders",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Amount",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "ExpectedPaymentDate",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "GeneralWorkplaceNumber",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "HhsCode",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "OHKMessage",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "PSNDate",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "PSNRefNum",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "PSNYosCode",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "PaymentDescription",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "PaymentPurpose",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "PaymentServiceUpdateTime",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "PaymentSource",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "PaymentState",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "PaymentSystem",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "PaymentSystemNumber",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "ReferenceInformation",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "SubWorkplaceCategoryCode",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "WorkplaceCategoryCode",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "XGroupId",
                table: "OBPaymentOrders");

            migrationBuilder.DropColumn(
                name: "XRequestId",
                table: "OBPaymentOrders");

            migrationBuilder.RenameColumn(
                name: "YosCode",
                table: "OBPaymentOrders",
                newName: "ConsentDetailType");
        }
    }
}
