using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class OBEventErrorCodeDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           
            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("34375630-82c1-492a-95b1-b59534fdf5c7"), "TR.OHVPS.Business.InvalidContent", 208, "Invalid TPP api definition. TPP should have ods api definition.", "Geçersiz TPP api tanımı. TPP, ods api tanımına sahip olmalıdır." },
                    { new Guid("39ad636e-c986-42db-90a5-30e15224c3b7"), "TR.OHVPS.Connection.InvalidTPPRole", 206, "Invalid TPP Role. TPP role must have hbhs.", "Geçersiz Yos rolü. Yosde hbhs rolü olmalı." },
                    { new Guid("53134ce1-8ff8-4d75-8a14-eb24b4d66465"), "TR.OHVPS.Business.InvalidContent", 329, "Header PSU-Session-ID must be empty in one time payment.", "Tek seferlik ödeme gibi ÖHK’nın tanınmadan başlatıldığı işlemlerde PSU-Session-ID başlık değeri boş olarak iletilmelidir." },
                    { new Guid("b4adcd12-b7c4-4168-beff-9ca9beb19f8b"), "TR.OHVPS.Business.InvalidContent", 210, "Olay Abonelik No in parameter and object not match.", "Parametre olarak gelen olay abone no ile olayabonelik nesnesi içerisinde gelen olay abonelik no verileri aynı değil." },
                    { new Guid("ef2e87ce-a18b-48cf-bd09-19a1cfe5fc6b"), "TR.OHVPS.Connection.InvalidTPPRole", 207, "Invalid TPP Role. TPP role must have obhs.", "Geçersiz Yos rolü. Yosde obhs rolü olmalı." },
                    { new Guid("f1184545-2c05-4059-aecf-55c242321808"), "TR.OHVPS.Resource.NotFound", 154, "OlayAbonelikNo not found.", "OlayAbonelikNo bulunamadı." },
                    { new Guid("fb4c280a-01af-4c0c-a1a1-cd5c4bfcee67"), "TR.OHVPS.Business.InvalidContent", 209, "An entity already exists for this YOS.", "1 YÖS'ün 1 HHS'de 1 adet abonelik kaydı olabilir. Kaynak çakışması." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("34375630-82c1-492a-95b1-b59534fdf5c7"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("39ad636e-c986-42db-90a5-30e15224c3b7"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("53134ce1-8ff8-4d75-8a14-eb24b4d66465"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b4adcd12-b7c4-4168-beff-9ca9beb19f8b"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ef2e87ce-a18b-48cf-bd09-19a1cfe5fc6b"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f1184545-2c05-4059-aecf-55c242321808"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("fb4c280a-01af-4c0c-a1a1-cd5c4bfcee67"));
        }
    }
}
