using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class UpdateErrorCodeDetailsFraud : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           // Delete existing records by InternalCode
            migrationBuilder.Sql("DELETE FROM \"OBErrorCodeDetails\" WHERE \"InternalCode\" IN (317, 311, 320, 318, 319, 316, 10, 322, 314, 321, 315)");


            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("1a4abf88-69bb-4225-9f01-338430dcb285"), "TR.OHVPS.Resource.InvalidFormat", 317, "PSU-Fraud-Check header in the TPP request LastPasswordChangeFlag is missing.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki LastPasswordChangeFlag eksik." },
                    { new Guid("38a147be-4774-4e3f-9528-b6a5f7ef75a4"), "TR.OHVPS.Resource.InvalidFormat", 311, "PSU-Fraud-Check header in the TPP request FirstLoginFlag is missing.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki FirstLoginFlag eksik." },
                    { new Guid("44ce5cb9-de46-454e-be11-155d11b71cca"), "TR.OHVPS.Resource.InvalidFormat", 320, "PSU-Fraud-Check header in the TPP request UnsafeAccountFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki UnsafeAccountFlag hatalı." },
                    { new Guid("492b70a9-b776-4bbe-b3cd-aabc89d80b39"), "TR.OHVPS.Resource.InvalidFormat", 318, "PSU-Fraud-Check header in the TPP request LastPasswordChangeFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki LastPasswordChangeFlag hatalı." },
                    { new Guid("5b15f78c-652e-4d63-bd57-43c3a4d6a6d8"), "TR.OHVPS.Resource.InvalidFormat", 319, "PSU-Fraud-Check header in the TPP request BlacklistFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki BlacklistFlag hatalı." },
                    { new Guid("6ab9d72f-0f18-474e-babe-f45d6bb66de8"), "TR.OHVPS.Resource.InvalidFormat", 316, "PSU-Fraud-Check header in the TPP request DeviceFirstLoginFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki DeviceFirstLoginFlag hatalı." },
                    { new Guid("8aa5323a-7e22-4fbf-a870-bd1dce017247"), "TR.OHVPS.Field.Invalid", 10, "size must be between 7-9", "boyut '7' ile  '9' arasında olmalı" },
                    { new Guid("abaeb7b9-9674-4f3e-a8e5-c4c70cb5131f"), "TR.OHVPS.Resource.InvalidFormat", 322, "PSU-Fraud-Check header in the TPP request MalwareFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki MalwareFlag hatalı." },
                    { new Guid("bf6b5d44-afdb-41d6-b956-841ff687d938"), "TR.OHVPS.Resource.InvalidFormat", 314, "PSU-Fraud-Check header in the TPP request FirstLoginFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki FirstLoginFlag hatalı." },
                    { new Guid("ea5f785f-5663-47c9-a209-e4eaf57ffb8f"), "TR.OHVPS.Resource.InvalidFormat", 321, "PSU-Fraud-Check header in the TPP request AnomalyFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki AnomalyFlag hatalı." },
                    { new Guid("efc2a37f-8761-4632-87e9-a27819d55542"), "TR.OHVPS.Resource.InvalidFormat", 315, "PSU-Fraud-Check header in the TPP request DeviceFirstLoginFlag is missing.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki DeviceFirstLoginFlag eksik." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("1a4abf88-69bb-4225-9f01-338430dcb285"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("38a147be-4774-4e3f-9528-b6a5f7ef75a4"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("44ce5cb9-de46-454e-be11-155d11b71cca"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("492b70a9-b776-4bbe-b3cd-aabc89d80b39"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("5b15f78c-652e-4d63-bd57-43c3a4d6a6d8"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("6ab9d72f-0f18-474e-babe-f45d6bb66de8"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("8aa5323a-7e22-4fbf-a870-bd1dce017247"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("abaeb7b9-9674-4f3e-a8e5-c4c70cb5131f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("bf6b5d44-afdb-41d6-b956-841ff687d938"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ea5f785f-5663-47c9-a209-e4eaf57ffb8f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("efc2a37f-8761-4632-87e9-a27819d55542"));

        }
    }
}
