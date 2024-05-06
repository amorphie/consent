using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class ErrorCodeDetailsHeaderScript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("091c5ad5-e3ad-473a-b8b3-2ab010295eb3"), "TR.OHVPS.Resource.InvalidSignature", 301, "X-JWS-Signature header in the TPP request is algorithm is wrong.", "YOS ten gelen istekteki X-JWS-Signature basligindaki alg geçersiz." },
                    { new Guid("123ab5ab-ee48-4b4f-9161-b1bbee7c8379"), "TR.OHVPS.Resource.InvalidSignature", 308, "PSU-Fraud-Check header in the TPP request is algorithm is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligindaki alg geçersiz." },
                    { new Guid("145dba06-7600-4b69-a56d-7fc042707ec4"), "TR.OHVPS.Resource.Forbidden", 325, "There is no transaction permission for this consent.", "İzin türü kontrolü başarısız. İşlem yetkisi olmayan rıza no ile işlem bilgisi sorgulanamaz." },
                    { new Guid("33af5a1a-ba25-47cd-a3d4-66e27f234f25"), "TR.OHVPS.Resource.InvalidSignature", 313, "PSU-Fraud-Check header in the TPP request ex is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki ex hatalı." },
                    { new Guid("34b8db4c-6d7f-41bb-83cc-7cad0d58464e"), "TR.OHVPS.Resource.InvalidSignature", 303, "X-JWS-Signature header in the TPP request is body claim is missing.", "YOS ten gelen istekteki X-JWS-Signature basliginda body claim bulunmamaktadır." },
                    { new Guid("37000d9a-aa46-4d5b-8b22-8fd926d458b7"), "TR.OHVPS.Resource.InvalidSignature", 314, "PSU-Fraud-Check header in the TPP request FirstLoginFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki FirstLoginFlag hatalı." },
                    { new Guid("3780e5f0-1ca1-4625-8dbb-99f36da05970"), "TR.OHVPS.Resource.InvalidSignature", 315, "PSU-Fraud-Check header in the TPP request DeviceFirstLoginFlag is missing.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki DeviceFirstLoginFlag eksik." },
                    { new Guid("44cad478-f9e7-4ca3-9275-d8e7347e3deb"), "TR.OHVPS.Resource.InvalidSignature", 305, "X-JWS-Signature header in the TPP request ex is missing.", "YOS ten gelen istekteki X-JWS-Signature basligi içerisindeki ex eksik." },
                    { new Guid("4a7ae714-c068-4afa-9bac-2b436185b0e8"), "TR.OHVPS.Resource.Forbidden", 323, "There is no account permission for this consent.", "İzin türü kontrolü başarısız. Hesap yetkisi olmayan rıza no ile hesap sorgulanamaz." },
                    { new Guid("4bedce71-1fe3-47a1-8120-05c4b6f34330"), "TR.OHVPS.Resource.InvalidSignature", 322, "PSU-Fraud-Check header in the TPP request MalwareFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki MalwareFlag hatalı." },
                    { new Guid("4c820c3d-c298-4acb-9520-1deeb4aacb1b"), "TR.OHVPS.Resource.Forbidden", 324, "There is no balance permission for this consent.", "İzin türü kontrolü başarısız. Bakiye yetkisi olmayan rıza no ile bakiye sorgulanamaz." },
                    { new Guid("52901069-762b-49fd-bdec-94f76609faa2"), "TR.OHVPS.Resource.InvalidSignature", 318, "PSU-Fraud-Check header in the TPP request LastPasswordChangeFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki LastPasswordChangeFlag hatalı." },
                    { new Guid("5cf95caf-fefb-4062-93bd-ca9661be9cf4"), "TR.OHVPS.Resource.InvalidSignature", 317, "PSU-Fraud-Check header in the TPP request LastPasswordChangeFlag is missing.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki LastPasswordChangeFlag eksik." },
                    { new Guid("60f44090-8480-42d8-9e61-974853862991"), "TR.OHVPS.Resource.InvalidSignature", 311, "PSU-Fraud-Check header in the TPP request FirstLoginFlag is missing.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki FirstLoginFlag eksik." },
                    { new Guid("6b43dca7-359e-4075-8f0a-940b25235262"), "TR.OHVPS.Resource.MissingSignature", 307, "Header PSU-Fraud-Check property is empty.", "İstek başlığında PSU-Fraud-Check alanı eksik." },
                    { new Guid("7e20161a-81ce-404b-8916-b38f43c02984"), "TR.OHVPS.Resource.InvalidSignature", 316, "PSU-Fraud-Check header in the TPP request DeviceFirstLoginFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki DeviceFirstLoginFlag hatalı." },
                    { new Guid("8e042fbd-b912-426e-ba10-11a3006f7d3f"), "TR.OHVPS.Resource.InvalidSignature", 321, "PSU-Fraud-Check header in the TPP request AnomalyFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki AnomalyFlag hatalı." },
                    { new Guid("b2a96301-c54b-4712-8d95-3d60bc5a42a8"), "TR.OHVPS.Resource.InvalidSignature", 302, "X-JWS-Signature header in the TPP request is expired.", "YOS ten gelen istekteki X-JWS-Signature basligi zaman asimina ugramis." },
                    { new Guid("b374e743-6222-476d-88af-99143a49ff7c"), "TR.OHVPS.Resource.InvalidSignature", 304, "X-JWS-Signature signature does not match locally computed signature.", "YOS ten gelen istekteki X-JWS-Signature kayitli public key ile dogrulanamadi." },
                    { new Guid("c81e12ac-5b40-4ab3-82ba-c7064d4ce3fd"), "TR.OHVPS.Resource.InvalidSignature", 306, "X-JWS-Signature header in the TPP request ex is wrong.", "YOS ten gelen istekteki X-JWS-Signature basligi içerisindeki ex hatalı." },
                    { new Guid("ec143d8c-7cd0-40f6-ae0a-88c75dc24899"), "TR.OHVPS.Resource.InvalidSignature", 310, "PSU-Fraud-Check signature does not match locally computed signature.", "YOS ten gelen istekteki PSU-Fraud-Check kayitli public key ile dogrulanamadi." },
                    { new Guid("f064254b-19e2-4af5-9fa5-128122d39f5a"), "TR.OHVPS.Resource.InvalidSignature", 319, "PSU-Fraud-Check header in the TPP request BlacklistFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki BlacklistFlag hatalı." },
                    { new Guid("f2a9adb3-acac-47be-9737-acdd15d16c6f"), "TR.OHVPS.Resource.InvalidSignature", 312, "PSU-Fraud-Check header in the TPP request ex is missing.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki ex eksik." },
                    { new Guid("f7ce3250-5eb9-43bc-b6c8-e0e333c3baa6"), "TR.OHVPS.Resource.InvalidSignature", 320, "PSU-Fraud-Check header in the TPP request UnsafeAccountFlag is wrong.", "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki UnsafeAccountFlag hatalı." },
                    { new Guid("feb74fbb-d1bd-479e-a800-bd228bc7b5f7"), "TR.OHVPS.Resource.InvalidSignature", 309, "PSU-Fraud-Check expired.", "YOS ten gelen istekteki PSU-Fraud-Check tarihi gecmistir." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("091c5ad5-e3ad-473a-b8b3-2ab010295eb3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("123ab5ab-ee48-4b4f-9161-b1bbee7c8379"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("145dba06-7600-4b69-a56d-7fc042707ec4"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("33af5a1a-ba25-47cd-a3d4-66e27f234f25"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("34b8db4c-6d7f-41bb-83cc-7cad0d58464e"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("37000d9a-aa46-4d5b-8b22-8fd926d458b7"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("3780e5f0-1ca1-4625-8dbb-99f36da05970"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("44cad478-f9e7-4ca3-9275-d8e7347e3deb"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("4a7ae714-c068-4afa-9bac-2b436185b0e8"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("4bedce71-1fe3-47a1-8120-05c4b6f34330"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("4c820c3d-c298-4acb-9520-1deeb4aacb1b"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("52901069-762b-49fd-bdec-94f76609faa2"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("5cf95caf-fefb-4062-93bd-ca9661be9cf4"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("60f44090-8480-42d8-9e61-974853862991"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("6b43dca7-359e-4075-8f0a-940b25235262"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("7e20161a-81ce-404b-8916-b38f43c02984"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("8e042fbd-b912-426e-ba10-11a3006f7d3f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b2a96301-c54b-4712-8d95-3d60bc5a42a8"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b374e743-6222-476d-88af-99143a49ff7c"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("c81e12ac-5b40-4ab3-82ba-c7064d4ce3fd"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec143d8c-7cd0-40f6-ae0a-88c75dc24899"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f064254b-19e2-4af5-9fa5-128122d39f5a"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f2a9adb3-acac-47be-9737-acdd15d16c6f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f7ce3250-5eb9-43bc-b6c8-e0e333c3baa6"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("feb74fbb-d1bd-479e-a800-bd228bc7b5f7"));

        }
    }
}
