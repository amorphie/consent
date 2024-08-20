using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateErrorCodesTransactions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
             // Delete existing records by InternalCode
            migrationBuilder.Sql("DELETE FROM \"OBErrorCodeDetails\" WHERE \"InternalCode\" IN (112,113,114)");

            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("4ce229a2-e288-4d54-bb9a-45ef0ca6cd07"), "TR.OHVPS.Business.InvalidContent", 114, "For system enquiry, last 24 hours can be enquirable.", "sistemsel yapılan sorgulamalarda hem bireysel, hem de kurumsal ÖHK’lar için;son 24 saat sorgulanabilir." },
                    { new Guid("53c0a141-4e45-422e-a2d3-63e01a9c6937"), "TR.OHVPS.Business.InvalidContent", 113, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 week.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark kurumsal ÖHK’lar için en fazla 1 hafta olabilir." },
                    { new Guid("ac9ba63b-ad41-4377-a731-ee5d87dd32d7"), "TR.OHVPS.Business.InvalidContent", 112, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 month.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark bireysel ÖHK’lar için en fazla 1 ay olabilir." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("4ce229a2-e288-4d54-bb9a-45ef0ca6cd07"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("53c0a141-4e45-422e-a2d3-63e01a9c6937"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ac9ba63b-ad41-4377-a731-ee5d87dd32d7"));

            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("5570463d-ebda-45af-9829-18be10642b12"), "TR.OHVPS.Business.InvalidContent", 114, "For system enquiry, last 24 hours can be enquirable.", "sistemsel yapılan sorgulamalarda hem bireysel, hem de kurumsal ÖHK’lar için;son 24 saat sorgulanabilir." },
                    { new Guid("afd1dbf6-c826-499c-a28b-f16788cac967"), "TR.OHVPS.Business.InvalidContent", 113, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 week.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark kurumsal ÖHK’lar için en fazla 1 hafta olabilir." },
                    { new Guid("cd93adba-643e-4496-b67f-45024cce05a1"), "TR.OHVPS.Business.InvalidContent", 112, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 month.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark bireysel ÖHK’lar için en fazla 1 ay olabilir." }
                });
        }
    }
}
