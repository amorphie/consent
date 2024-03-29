using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class OBErrorCodeDetailDataScript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
        DO
        $do$
        BEGIN
            IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'OBErrorCodeDetails') THEN
                TRUNCATE TABLE ""OBErrorCodeDetails"";
            END IF;
        END
        $do$;");
            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("0fcd4b14-e0a4-4806-af7e-51c3d0b5b794"), "TR.OHVPS.Resource.ConsentMismatch", 162, "There is a Authorized / Authorzation Used consent in the system. First cancel the consent.", "Sistemde Yetkilendirildi / Yetki Kullanıldı durumunda rıza olduğu için rıza kabul edilmedi. Öncelikli olarak rızayı iptal ediniz." },
                    { new Guid("194f807a-d685-4d72-88ac-3ab578d235d3"), "TR.OHVPS.Field.Invalid", 7, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("2ba88e80-0efb-49ad-b862-ec1bf062c50e"), "TR.OHVPS.Field.Invalid", 16, "size must be between 1-9", "boyut '1' ile  '9' arasında olmalı" },
                    { new Guid("379d979e-b109-4c9c-8046-ed99819d166d"), "TR.OHVPS.Server.InternalError", 151, "Unexpected condition was encountered.", "Beklenmeyen bir durumla karşılaşıldı." },
                    { new Guid("3917f99d-5a62-4a4b-ba0b-7e1742329888"), "TR.OHVPS.Field.Invalid", 23, "hesapIslemBslZmn hesapIslemBtsZmn fields should only be sent when the Basic Transaction Information/Detailed Transaction Information permission is selected.", "hesapIslemBslZmn hesapIslemBtsZmn alanları sadece Temel İşlem bilgisi/ayrıntlı işlem bilgisi izni seçilmiş olduğu zaman gönderilmelidir." },
                    { new Guid("3a27b20f-c872-4eba-8ff5-5551fe7622dd"), "TR.OHVPS.Field.Invalid", 10, "size must be between 1-9", "boyut '1' ile  '9' arasında olmalı" },
                    { new Guid("3bb28c87-68ca-4d3c-9327-f5bb1972f872"), "TR.OHVPS.Field.Invalid", 5, "size must be between 1-30", "boyut '1' ile  '30' arasında olmalı" },
                    { new Guid("3bd75801-cf89-4451-84be-56cc1c0a6c07"), "TR.OHVPS.Resource.NotFound", 150, "Resource not found", "Kaynak bulunamadı." },
                    { new Guid("449627e3-afa6-48d7-8f62-68428280d7f3"), "TR.OHVPS.Field.Invalid", 22, "When the Basic Transaction Information/Detailed Transaction Information permission is selected, the hesapIslemBslZmn hesapIslemBtsZmn fields must be filled.", "Temel İşlem bilgisi/ayrıntlı işlem bilgisi izni seçilmiş olduğu zaman hesapIslemBslZmn hesapIslemBtsZmn alanlarının doldurulması zorunludur." },
                    { new Guid("475faaee-85bf-477f-be5d-5c8ec85454c3"), "TR.OHVPS.Business.InvalidContent", 200, "PsuInitiated invalid", "PsuInitiated değeri hatalı." },
                    { new Guid("48134e2a-8194-405a-ac34-7ce008941bc5"), "TR.OHVPS.Resource.ConsentMismatch", 160, "Consent not valid to process", "Consent işlem yapılmaya uygun değil." },
                    { new Guid("4a201b66-84a2-4901-8303-4237a047303e"), "TR.OHVPS.Connection.InvalidTPP", 102, "Invalid TPP Code", "Geçersiz Yos kodu." },
                    { new Guid("53aa54cf-668d-43a2-bfa0-81ca3a23fbbf"), "TR.OHVPS.Field.Missing", 2, "must not be null", "boş değer olamaz" },
                    { new Guid("591157e3-439a-4aea-86d7-4e14d0a46a02"), "TR.OHVPS.Resource.ConsentMismatch", 161, "Consent state not valid to delete", "Consent rıza durumu silme işlemine uygun değil." },
                    { new Guid("5db0848b-60e1-480b-9b1a-69344f4875e6"), "TR.OHVPS.Field.Invalid", 15, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("5f77342f-9ed1-43e6-b064-7ee3e792863f"), "TR.OHVPS.Field.Invalid", 6, "size must be 8", "boyut '8' olmalı" },
                    { new Guid("5fc878ae-f4a2-47ab-a923-f7562a378e32"), "TR.OHVPS.Field.Invalid", 11, "size must be 10", "boyut '10' olmalı" },
                    { new Guid("608e4a3e-cbb9-4be7-8a9d-701a756601e1"), "TR.OHVPS.Field.Invalid", 18, "Temel hesap bilgisi izni must.", "Temel hesap bilgisi izni seçimi zorunludur." },
                    { new Guid("64348754-891e-4a3a-afb3-acc14837b1f4"), "TR.OHVPS.Field.Invalid", 4, "size must be 4", "boyut '4' olmalı" },
                    { new Guid("7afe0e99-c77e-4b6b-96f0-7c2a74c08f8c"), "TR.OHVPS.Field.Invalid", 17, "size must be 10", "boyut '10' olmalı" },
                    { new Guid("82d12fc1-f7ee-40a2-9d2a-17b324ace565"), "TR.OHVPS.Field.Invalid", 21, "The minimum value it can take is consent date +1 day, the maximum value it can take is: Consent date + 6 months.", "Alabileceği minimum değer tarihi +1 gün, alabileceği maksimum değer : Rıza tarihi + 6 ay " },
                    { new Guid("9985bed4-e4e3-46fb-9b5c-fe609ee48764"), "TR.OHVPS.Field.Invalid", 25, "hesapIslemBslZmn can not be later than hesapIslemBtsZmn.", "hesapIslemBslZmn, hesapIslemBtsZmn verisinden sonra olamaz. " },
                    { new Guid("a7e259d2-027b-4d65-8b57-6692ef4da22a"), "TR.OHVPS.Field.Invalid", 19, "Detail transactions permission can not be selected without Basic transactions.", "Temelişlem bilgisi izni seçimi yapılmadan ayrıntılı işlem bilgisi seçimi yapılamaz.." },
                    { new Guid("aedab8de-1803-405d-aba7-0bd7780adf64"), "TR.OHVPS.Field.Invalid", 3, "Invalid data.", "Geçersiz veri." },
                    { new Guid("b344c2c1-160b-480e-9ca9-5764e3e28be9"), "TR.OHVPS.Field.Invalid", 24, "Minimum date: Date of consent given – 12 months Maximum date: Date of consent given + 12 months", "Minimum tarih : Rızanın veriliş tarihi – 12 ay Maksimum tarih : Rızanın veriliş tarihi + 12 ay" },
                    { new Guid("b5ac5e4e-8f08-4884-aa33-b9f4fdfff882"), "TR.OHVPS.Field.Invalid", 20, "Instant balance notification cannot be selected without selecting balance information permission.", "Bakiye bilgisi izni seçimi yapılmadan anlık bakiye bildirimi seçimi yapılamaz.." },
                    { new Guid("b6975bb1-c645-4d5d-9f06-822bd0ea4d10"), "TR.OHVPS.Field.Invalid", 14, "size must be between 1-30", "boyut '1' ile  '30' arasında olmalı" },
                    { new Guid("b9396999-2fea-42b2-9161-45bfbcfb6790"), "TR.OHVPS.Field.Invalid", 9, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("d4a8b787-3249-4f8c-b388-0ab01ce89167"), "TR.OHVPS.Business.CustomerInfoMismatch", 104, "kmlk.kmlkVrs - ayrikGkd.ohkTanimDeger must match.", "kmlk.kmlkVrs - ayrikGkd.ohkTanimDeger aynı olmalı." },
                    { new Guid("d6ff87f3-68ff-49d1-86c3-dabe6bc394ac"), "TR.OHVPS.Field.Invalid", 12, "size must be 26", "boyut '26' olmalı" },
                    { new Guid("da7a473a-6d7f-4005-a07e-3756e3ca5d21"), "TR.OHVPS.Business.InvalidContent", 201, "User reference in header is wrong.", "User reference değeri hatalı." },
                    { new Guid("dbe5b92d-f17c-400c-8185-70a4be545260"), "TR.OHVPS.Field.Invalid", 24, "Minimum date: Date of consent given – 12 months Maximum date: Date of consent given + 12 months", "Minimum tarih : Rızanın veriliş tarihi – 12 ay Maksimum tarih : Rızanın veriliş tarihi + 12 ay" },
                    { new Guid("dda9c3d8-4971-4f1c-9e1f-12b28fb2df14"), "TR.OHVPS.Resource.InvalidFormat", 100, "Validation error", "Şema kontrolleri başarısız" },
                    { new Guid("df379553-da4f-4277-b56f-820274f8314c"), "TR.OHVPS.Connection.InvalidASPSP", 101, "Invalid ASPSP Code", "Geçersiz HHS kodu." },
                    { new Guid("e5382130-d481-4978-90c8-2e719684e739"), "TR.OHVPS.Business.EventSubscriptionNotFound", 105, "No evet subscription for AYRIK_GKD_BASARILI and AYRIK_GKD_BASARISIZ.", "AYRIK_GKD_BASARILI ve AYRIK_GKD_BASARISIZ olay tipleri için olay aboneliği yapılmalıdır." },
                    { new Guid("e5b2e5c5-ef4c-49ef-8a5e-6508f2fefb75"), "TR.OHVPS.Field.Invalid", 13, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("edc568f4-ca13-4a6b-a1ba-437505ef6cdc"), "TR.OHVPS.Field.Invalid", 8, "size must be between 1-30", "boyut '1' ile  '30' arasında olmalı" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("0fcd4b14-e0a4-4806-af7e-51c3d0b5b794"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("194f807a-d685-4d72-88ac-3ab578d235d3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("2ba88e80-0efb-49ad-b862-ec1bf062c50e"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("379d979e-b109-4c9c-8046-ed99819d166d"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("3917f99d-5a62-4a4b-ba0b-7e1742329888"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("3a27b20f-c872-4eba-8ff5-5551fe7622dd"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("3bb28c87-68ca-4d3c-9327-f5bb1972f872"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("3bd75801-cf89-4451-84be-56cc1c0a6c07"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("449627e3-afa6-48d7-8f62-68428280d7f3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("475faaee-85bf-477f-be5d-5c8ec85454c3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("48134e2a-8194-405a-ac34-7ce008941bc5"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("4a201b66-84a2-4901-8303-4237a047303e"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("53aa54cf-668d-43a2-bfa0-81ca3a23fbbf"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("591157e3-439a-4aea-86d7-4e14d0a46a02"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("5db0848b-60e1-480b-9b1a-69344f4875e6"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("5f77342f-9ed1-43e6-b064-7ee3e792863f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("5fc878ae-f4a2-47ab-a923-f7562a378e32"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("608e4a3e-cbb9-4be7-8a9d-701a756601e1"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("64348754-891e-4a3a-afb3-acc14837b1f4"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("7afe0e99-c77e-4b6b-96f0-7c2a74c08f8c"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("82d12fc1-f7ee-40a2-9d2a-17b324ace565"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("9985bed4-e4e3-46fb-9b5c-fe609ee48764"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("a7e259d2-027b-4d65-8b57-6692ef4da22a"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("aedab8de-1803-405d-aba7-0bd7780adf64"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b344c2c1-160b-480e-9ca9-5764e3e28be9"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b5ac5e4e-8f08-4884-aa33-b9f4fdfff882"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b6975bb1-c645-4d5d-9f06-822bd0ea4d10"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b9396999-2fea-42b2-9161-45bfbcfb6790"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("d4a8b787-3249-4f8c-b388-0ab01ce89167"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("d6ff87f3-68ff-49d1-86c3-dabe6bc394ac"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("da7a473a-6d7f-4005-a07e-3756e3ca5d21"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("dbe5b92d-f17c-400c-8185-70a4be545260"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("dda9c3d8-4971-4f1c-9e1f-12b28fb2df14"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("df379553-da4f-4277-b56f-820274f8314c"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("e5382130-d481-4978-90c8-2e719684e739"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("e5b2e5c5-ef4c-49ef-8a5e-6508f2fefb75"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("edc568f4-ca13-4a6b-a1ba-437505ef6cdc"));


        }
    }
}
