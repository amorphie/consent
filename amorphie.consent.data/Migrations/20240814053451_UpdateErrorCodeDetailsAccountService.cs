using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateErrorCodeDetailsAccountService : Migration
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
                    { new Guid("5570463d-ebda-45af-9829-18be10642b12"), "TR.OHVPS.Resource.InvalidContent", 114, "For system enquiry, last 24 hours can be enquirable.", "sistemsel yapılan sorgulamalarda hem bireysel, hem de kurumsal ÖHK’lar için;son 24 saat sorgulanabilir." },
                    { new Guid("afd1dbf6-c826-499c-a28b-f16788cac967"), "TR.OHVPS.Resource.InvalidContent", 113, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 week.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark kurumsal ÖHK’lar için en fazla 1 hafta olabilir." },
                    { new Guid("cd93adba-643e-4496-b67f-45024cce05a1"), "TR.OHVPS.Resource.InvalidContent", 112, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 month.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark bireysel ÖHK’lar için en fazla 1 ay olabilir." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("5570463d-ebda-45af-9829-18be10642b12"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("afd1dbf6-c826-499c-a28b-f16788cac967"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("cd93adba-643e-4496-b67f-45024cce05a1"));

            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                   { new Guid("610ce78f-79fb-4795-88dd-f2884316e396"), "TR.OHVPS.Resource.InvalidFormat", 114, "For system enquiry, last 24 hours can be enquirable.", "sistemsel yapılan sorgulamalarda hem bireysel, hem de kurumsal ÖHK’lar için;son 24 saat sorgulanabilir." },
                   { new Guid("6f3e094e-c716-4cd6-8372-06298191e402"), "TR.OHVPS.Resource.InvalidFormat", 113, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 week.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark kurumsal ÖHK’lar için en fazla 1 hafta olabilir." },
                   { new Guid("8de16fb2-a131-40ce-b051-8bf48c1d4a42"), "TR.OHVPS.Resource.InvalidFormat", 112, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 month.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark bireysel ÖHK’lar için en fazla 1 ay olabilir." }
                });
        }
    }
}
