using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class OBPaymentErrorCodes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("1e84eb29-d627-4230-b810-cb3104ba070d"), "TR.OHVPS.Field.Invalid", 30, "size must be 4", "boyut '4' olmalı" },
                    { new Guid("20e699cd-2a6f-4ea6-899d-eb36459264e4"), "TR.OHVPS.Field.Invalid", 27, "size must be between 1-24", "boyut '1' ile  '24' arasında olmalı" },
                    { new Guid("22a6df75-fcf4-4e73-bc0d-dfefdde48a36"), "TR.OHVPS.Server.InternalError", 153, "By validating header jwt property, body not set.", "Istek başlığında bulunda xjwtsignature alanı kontrol edilirken beklenmedik bir durumla karşılaşıldı. Body değeri set edilmemiş." },
                    { new Guid("3229051f-7110-45f5-88c2-229d9ed01124"), "TR.OHVPS.Resource.InvalidSignature", 327, "X-JWS-Signature header is invalid.", "YOS ten gelen istekteki X-JWS-Signature basligi gecersiz." },
                    { new Guid("6168b013-11ca-4487-b1a4-fe8ea2cfb7b3"), "TR.OHVPS.Field.Invalid", 29, "odmKynk should be O refers to open banking", "odmKynk “O” değeri atanarak iletilmelidir. “O” değeri “Açık bankacılık aracılığı ile gönderilen ödemelerde kullanılır.” anlamını taşımaktadır." },
                    { new Guid("75d017e2-3798-41ba-b168-3b70f27e3070"), "TR.OHVPS.Field.Invalid", 28, "size must be between 1-200", "boyut '1' ile '200' arasında olmalı" },
                    { new Guid("8740c775-d7b7-477c-a221-8c8474b3d6f8"), "TR.OHVPS.Field.Invalid", 31, "size must be 4", "boyut '4' olmalı" },
                    { new Guid("93597342-8767-42ae-8c9e-08067f42babf"), "TR.OHVPS.Field.Invalid", 33, "If payment is not kolas, unv and hspno is required", "Kolas işlemi değilse, unv hspno alanlarının doldu olması gerekmektedir." },
                    { new Guid("9e187f7c-f40b-4b5b-a9e8-d5b3e9f8ebc6"), "TR.OHVPS.Field.Invalid", 34, "size must be between 7-50", "boyut '7' ile '50' arasında olmalı" },
                    { new Guid("a741de55-e5e3-4396-b662-d648151b949d"), "TR.OHVPS.Resource.InvalidSignature", 326, "PSU-Fraud-Check header is invalid.", "YOS ten gelen istekteki PSU-Fraud-Check basligi gecersiz." },
                    { new Guid("ab39c236-266e-46b0-ad24-d89330f0a884"), "TR.OHVPS.Field.Invalid", 26, "size must be 3", "boyut '3' olmalı" },
                    { new Guid("ae3c5ffd-ec0d-4bcf-94a2-5c69930392f4"), "TR.OHVPS.Field.Invalid", 35, "size must be 4", "boyut '4' olmalı" },
                    { new Guid("ef04705c-1f0a-4c9d-b3c1-3c1fbb8b9fcb"), "TR.OHVPS.Field.Invalid", 32, "size must be 8", "boyut '8' olmalı" },
                    { new Guid("f6e503bb-6ddc-4fc1-923e-d67a67e75846"), "TR.OHVPS.Resource.InvalidFormat", 119, "karekod and kolas can not be used together.", "kkod kolas aynı mesajda dolu olarak gönderilemez." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("1e84eb29-d627-4230-b810-cb3104ba070d"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("20e699cd-2a6f-4ea6-899d-eb36459264e4"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("22a6df75-fcf4-4e73-bc0d-dfefdde48a36"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("3229051f-7110-45f5-88c2-229d9ed01124"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("6168b013-11ca-4487-b1a4-fe8ea2cfb7b3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("75d017e2-3798-41ba-b168-3b70f27e3070"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("8740c775-d7b7-477c-a221-8c8474b3d6f8"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("93597342-8767-42ae-8c9e-08067f42babf"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("9e187f7c-f40b-4b5b-a9e8-d5b3e9f8ebc6"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("a741de55-e5e3-4396-b662-d648151b949d"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ab39c236-266e-46b0-ad24-d89330f0a884"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ae3c5ffd-ec0d-4bcf-94a2-5c69930392f4"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ef04705c-1f0a-4c9d-b3c1-3c1fbb8b9fcb"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f6e503bb-6ddc-4fc1-923e-d67a67e75846"));
        }
    }
}
