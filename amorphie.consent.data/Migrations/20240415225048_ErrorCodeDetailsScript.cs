using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class ErrorCodeDetailsScript : Migration
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
                    { new Guid("0d50c0c1-ed01-4b67-9e49-dbc419f5b8db"), "TR.OHVPS.Resource.InvalidFormat", 112, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 month.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark bireysel ÖHK’lar için en fazla 1 ay olabilir." },
                    { new Guid("13697d97-4d10-4c02-bf90-4b2da7f0e6c3"), "TR.OHVPS.Resource.ConsentMismatch", 162, "There is a Authorized / Authorzation Used consent in the system. First cancel the consent.", "Sistemde Yetkilendirildi / Yetki Kullanıldı durumunda rıza olduğu için rıza kabul edilmedi. Öncelikli olarak rızayı iptal ediniz." },
                    { new Guid("14b1b63c-e87d-4601-8129-9d5711c8c5c5"), "TR.OHVPS.Resource.InvalidFormat", 109, "hesapIslemBtsTrh,hesapIslemBslTrh values not valid", "hesapIslemBtsTrh,hesapIslemBslTrh değerleri geçersiz." },
                    { new Guid("16bcbc09-3641-4e7b-8f49-70e7a66257c1"), "TR.OHVPS.Resource.InvalidFormat", 107, "srlmKrtr value is not valid. it should be hspRef", "srlmKrtr değeri geçersiz. Olması gereken değer hspRef." },
                    { new Guid("196d6149-1a87-4df8-a70e-bf8b02dc2821"), "TR.OHVPS.Resource.InvalidFormat", 117, "minIslTtr value is not valid.", "minIslTtr değeri geçersiz." },
                    { new Guid("1ea3312f-f6b2-49d6-a97d-6258ce17ed71"), "TR.OHVPS.Field.Invalid", 17, "size must be 10", "boyut '10' olmalı" },
                    { new Guid("2a563a1d-808a-44ac-97f6-b48a195f60fb"), "TR.OHVPS.Resource.NotFound", 150, "Resource not found", "Kaynak bulunamadı." },
                    { new Guid("2cfdc525-0b8b-4722-ab48-0fba83a2b895"), "TR.OHVPS.Field.Invalid", 16, "size must be between 1-9", "boyut '1' ile  '9' arasında olmalı" },
                    { new Guid("346c4eeb-cf85-4e85-a786-567388056461"), "TR.OHVPS.Field.Missing", 2, "must not be null", "boş değer olamaz" },
                    { new Guid("34a0750e-7fda-4e4a-bdc7-c0b4977280ec"), "TR.OHVPS.Resource.InvalidFormat", 106, "syfKytSayi value is not valid. syfKytSayi can be between 1-100", "syfKytSayi değeri geçersiz. 1-100 aralığında olabilir." },
                    { new Guid("351cc3db-9972-489b-9f30-e628cdfe0bc3"), "TR.OHVPS.Field.Invalid", 13, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("41055406-4c99-4eef-b4fb-6e01dd6a864f"), "TR.OHVPS.Server.InternalError", 151, "Unexpected condition was encountered.", "Beklenmeyen bir durumla karşılaşıldı." },
                    { new Guid("44060e83-dadd-455a-8629-693cc9e2a05a"), "TR.OHVPS.Resource.InvalidFormat", 108, "srlmYon value is not valid.", "srlmYon değeri geçersiz." },
                    { new Guid("48161774-f8fd-46c1-ad40-f67ac51b32e0"), "TR.OHVPS.Field.Invalid", 24, "Minimum date: Date of consent given – 12 months Maximum date: Date of consent given + 12 months", "Minimum tarih : Rızanın veriliş tarihi – 12 ay Maksimum tarih : Rızanın veriliş tarihi + 12 ay" },
                    { new Guid("4a380958-f79d-45d4-a000-99718c9a3e46"), "TR.OHVPS.Field.Invalid", 15, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("568a9ff8-bb99-4b97-bc10-296714697379"), "TR.OHVPS.Resource.InvalidFormat", 100, "Validation error", "Şema kontrolleri başarısız" },
                    { new Guid("5a48ae18-d4c7-4725-a2ef-48f1950ae93c"), "TR.OHVPS.Field.Invalid", 20, "Instant balance notification cannot be selected without selecting balance information permission.", "Bakiye bilgisi izni seçimi yapılmadan anlık bakiye bildirimi seçimi yapılamaz.." },
                    { new Guid("6172522a-c516-46fe-941f-412cb36529ad"), "TR.OHVPS.Field.Invalid", 4, "size must be 4", "boyut '4' olmalı" },
                    { new Guid("66242d5b-1beb-4ea2-9faf-3eefc93129a8"), "TR.OHVPS.Resource.ConsentMismatch", 160, "Consent not valid to process", "Consent işlem yapılmaya uygun değil." },
                    { new Guid("6748e925-21b2-40fe-9431-87a2e54a2de6"), "TR.OHVPS.Field.Invalid", 6, "size must be between 1-8", "boyut '1' ile '8' arasında olmalı" },
                    { new Guid("6baf2a76-69b9-40f9-9ec7-9a714331fe00"), "TR.OHVPS.Resource.InvalidFormat", 111, "hesapIslemBtsTrh can not be early than hesapIslemBslTrh.", "hesapIslemBtsTrh hesapIslemBslTrh den önce olamaz." },
                    { new Guid("6c7839af-9e1c-4548-95db-95025f9a59b3"), "TR.OHVPS.Field.Invalid", 14, "size must be between 1-30", "boyut '1' ile  '30' arasında olmalı" },
                    { new Guid("70858d02-6763-4a37-9f0e-ebd3c2a3dc41"), "TR.OHVPS.Connection.InvalidTPP", 102, "Invalid TPP Code", "Geçersiz Yos kodu." },
                    { new Guid("7098aeb3-7f1f-4c2e-bc6d-4b8367bd2168"), "TR.OHVPS.Field.Invalid", 22, "When the Basic Transaction Information/Detailed Transaction Information permission is selected, the hesapIslemBslZmn hesapIslemBtsZmn fields must be filled.", "Temel İşlem bilgisi/ayrıntlı işlem bilgisi izni seçilmiş olduğu zaman hesapIslemBslZmn hesapIslemBtsZmn alanlarının doldurulması zorunludur." },
                    { new Guid("7d18d600-41be-4c09-8fd1-fcbfb155b27a"), "TR.OHVPS.Field.Invalid", 19, "Detail transactions permission can not be selected without Basic transactions.", "Temelişlem bilgisi izni seçimi yapılmadan ayrıntılı işlem bilgisi seçimi yapılamaz.." },
                    { new Guid("84e96832-b0b6-495f-87f2-c1a5dfb1aecc"), "TR.OHVPS.Field.Invalid", 3, "Invalid data.", "Geçersiz veri." },
                    { new Guid("852ca27f-4193-4d7e-8f37-bb0f53c91dab"), "TR.OHVPS.Field.Invalid", 25, "hesapIslemBslZmn can not be later than hesapIslemBtsZmn.", "hesapIslemBslZmn, hesapIslemBtsZmn verisinden sonra olamaz. " },
                    { new Guid("87b9ab37-d8e2-4abd-b531-ed478123331d"), "TR.OHVPS.Field.Invalid", 10, "size must be between 1-9", "boyut '1' ile  '9' arasında olmalı" },
                    { new Guid("8b8d1442-f5f7-48b0-b521-8d271b6c463f"), "TR.OHVPS.Connection.InvalidASPSP", 101, "Invalid ASPSP Code", "Geçersiz HHS kodu." },
                    { new Guid("943206c5-1c0f-4c65-b808-4e713b86be52"), "TR.OHVPS.Field.Invalid", 9, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("95d08f9f-21d2-4000-90ae-0b6683137f51"), "TR.OHVPS.Field.Invalid", 24, "Minimum date: Date of consent given – 12 months Maximum date: Date of consent given + 12 months", "Minimum tarih : Rızanın veriliş tarihi – 12 ay Maksimum tarih : Rızanın veriliş tarihi + 12 ay" },
                    { new Guid("961ee3ba-2657-4c85-a657-a0396fe618ab"), "TR.OHVPS.Field.Invalid", 5, "size must be between 1-30", "boyut '1' ile  '30' arasında olmalı" },
                    { new Guid("a52fdb22-7222-41dd-8503-cdd626495bf0"), "TR.OHVPS.Resource.InvalidFormat", 114, "For system enquiry, last 24 hours can be enquirable.", "sistemsel yapılan sorgulamalarda hem bireysel, hem de kurumsal ÖHK’lar için;son 24 saat sorgulanabilir." },
                    { new Guid("a5a31e98-fe46-4085-aa6b-c6ddd5559c20"), "TR.OHVPS.Resource.InvalidFormat", 110, "hesapIslemBtsTrh can not be later than enquiry datetime.", "hesapIslemBtsTrh sorgulama zamanından sonra olamaz." },
                    { new Guid("b3264a29-9abf-4605-9930-276b17b0e6bf"), "TR.OHVPS.Field.Invalid", 23, "hesapIslemBslZmn hesapIslemBtsZmn fields should only be sent when the Basic Transaction Information/Detailed Transaction Information permission is selected.", "hesapIslemBslZmn hesapIslemBtsZmn alanları sadece Temel İşlem bilgisi/ayrıntlı işlem bilgisi izni seçilmiş olduğu zaman gönderilmelidir." },
                    { new Guid("b8c32564-60c5-4946-bd62-183b3ea01df0"), "TR.OHVPS.Business.InvalidContent", 201, "User reference in header is wrong.", "User reference değeri hatalı." },
                    { new Guid("b8e367a4-4b63-40a5-ad32-0f04e70c9e93"), "TR.OHVPS.Business.CustomerInfoMismatch", 104, "kmlk.kmlkVrs - ayrikGkd.ohkTanimDeger must match.", "kmlk.kmlkVrs - ayrikGkd.ohkTanimDeger aynı olmalı." },
                    { new Guid("b9318747-0068-4ca3-830a-3b2d133a5ff3"), "TR.OHVPS.Business.InvalidContent", 200, "PsuInitiated invalid", "PsuInitiated değeri hatalı." },
                    { new Guid("bf2e98e1-20eb-453c-beca-dbeebc2e21f4"), "TR.OHVPS.Resource.ConsentMismatch", 161, "Consent state not valid to delete", "Consent rıza durumu silme işlemine uygun değil." },
                    { new Guid("c0e8ca89-60be-48cf-937b-4cd03b427bb8"), "TR.OHVPS.Field.Invalid", 11, "size must be 10", "boyut '10' olmalı" },
                    { new Guid("cadbbd74-431f-4f8a-b55e-fc55d2fdb25e"), "TR.OHVPS.Field.Invalid", 8, "size must be between 1-30", "boyut '1' ile  '30' arasında olmalı" },
                    { new Guid("ceb914d8-0c66-4ce2-ac03-a1e5ef650b2b"), "TR.OHVPS.Server.InternalError", 152, "By Checking Idempotency Unexpected condition was encountered.", "Idempotency kontrol edilirken beklenmeyen bir durumla karşılaşıldı." },
                    { new Guid("d6ee560c-7189-43f8-8f4c-075fdbee0ef9"), "TR.OHVPS.Field.Invalid", 18, "Temel hesap bilgisi izni must.", "Temel hesap bilgisi izni seçimi zorunludur." },
                    { new Guid("d74b07c8-4e51-4667-99b1-0ba2f6d0ac21"), "TR.OHVPS.Resource.InvalidFormat", 118, "mksIslTtr value is not valid.", "mksIslTtr değeri geçersiz." },
                    { new Guid("d75ad513-a612-4409-8bea-eb5c9b127321"), "TR.OHVPS.Field.Invalid", 12, "size must be 26", "boyut '26' olmalı" },
                    { new Guid("d8ebd86a-a2f8-49d2-aecf-0498894463db"), "TR.OHVPS.Field.Invalid", 7, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("ddd17f2d-b85f-4e03-aca5-2baa240a8334"), "TR.OHVPS.Business.EventSubscriptionNotFound", 105, "No evet subscription for AYRIK_GKD_BASARILI and AYRIK_GKD_BASARISIZ.", "AYRIK_GKD_BASARILI ve AYRIK_GKD_BASARISIZ olay tipleri için olay aboneliği yapılmalıdır." },
                    { new Guid("dfcc500a-b22b-44f9-a35d-0475cda2c78a"), "TR.OHVPS.Resource.InvalidFormat", 113, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 week.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark kurumsal ÖHK’lar için en fazla 1 hafta olabilir." },
                    { new Guid("e35192bb-2490-4166-8c86-1084231e0898"), "TR.OHVPS.Field.Invalid", 21, "The minimum value it can take is consent date +1 day, the maximum value it can take is: Consent date + 6 months.", "Alabileceği minimum değer tarihi +1 gün, alabileceği maksimum değer : Rıza tarihi + 6 ay " },
                    { new Guid("ec09f1f9-bffd-48e4-8a89-526b6abf4faf"), "TR.OHVPS.Resource.InvalidFormat", 116, "srlmKrtr value is not valid. it should be islGrckZaman", "srlmKrtr değeri geçersiz. Olması gereken değer islGrckZaman." },
                    { new Guid("ef07e888-175b-46f9-bd3e-e2581c13bef2"), "TR.OHVPS.Resource.InvalidFormat", 115, "brcAlc value is not valid.", "brcAlc değeri geçersiz." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("0d50c0c1-ed01-4b67-9e49-dbc419f5b8db"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("13697d97-4d10-4c02-bf90-4b2da7f0e6c3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("14b1b63c-e87d-4601-8129-9d5711c8c5c5"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("16bcbc09-3641-4e7b-8f49-70e7a66257c1"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("196d6149-1a87-4df8-a70e-bf8b02dc2821"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("1ea3312f-f6b2-49d6-a97d-6258ce17ed71"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("2a563a1d-808a-44ac-97f6-b48a195f60fb"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("2cfdc525-0b8b-4722-ab48-0fba83a2b895"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("346c4eeb-cf85-4e85-a786-567388056461"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("34a0750e-7fda-4e4a-bdc7-c0b4977280ec"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("351cc3db-9972-489b-9f30-e628cdfe0bc3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("41055406-4c99-4eef-b4fb-6e01dd6a864f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("44060e83-dadd-455a-8629-693cc9e2a05a"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("48161774-f8fd-46c1-ad40-f67ac51b32e0"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("4a380958-f79d-45d4-a000-99718c9a3e46"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("568a9ff8-bb99-4b97-bc10-296714697379"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("5a48ae18-d4c7-4725-a2ef-48f1950ae93c"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("6172522a-c516-46fe-941f-412cb36529ad"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("66242d5b-1beb-4ea2-9faf-3eefc93129a8"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("6748e925-21b2-40fe-9431-87a2e54a2de6"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("6baf2a76-69b9-40f9-9ec7-9a714331fe00"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("6c7839af-9e1c-4548-95db-95025f9a59b3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("70858d02-6763-4a37-9f0e-ebd3c2a3dc41"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("7098aeb3-7f1f-4c2e-bc6d-4b8367bd2168"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("7d18d600-41be-4c09-8fd1-fcbfb155b27a"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("84e96832-b0b6-495f-87f2-c1a5dfb1aecc"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("852ca27f-4193-4d7e-8f37-bb0f53c91dab"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("87b9ab37-d8e2-4abd-b531-ed478123331d"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("8b8d1442-f5f7-48b0-b521-8d271b6c463f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("943206c5-1c0f-4c65-b808-4e713b86be52"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("95d08f9f-21d2-4000-90ae-0b6683137f51"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("961ee3ba-2657-4c85-a657-a0396fe618ab"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("a52fdb22-7222-41dd-8503-cdd626495bf0"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("a5a31e98-fe46-4085-aa6b-c6ddd5559c20"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b3264a29-9abf-4605-9930-276b17b0e6bf"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b8c32564-60c5-4946-bd62-183b3ea01df0"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b8e367a4-4b63-40a5-ad32-0f04e70c9e93"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b9318747-0068-4ca3-830a-3b2d133a5ff3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("bf2e98e1-20eb-453c-beca-dbeebc2e21f4"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("c0e8ca89-60be-48cf-937b-4cd03b427bb8"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("cadbbd74-431f-4f8a-b55e-fc55d2fdb25e"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ceb914d8-0c66-4ce2-ac03-a1e5ef650b2b"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("d6ee560c-7189-43f8-8f4c-075fdbee0ef9"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("d74b07c8-4e51-4667-99b1-0ba2f6d0ac21"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("d75ad513-a612-4409-8bea-eb5c9b127321"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("d8ebd86a-a2f8-49d2-aecf-0498894463db"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ddd17f2d-b85f-4e03-aca5-2baa240a8334"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("dfcc500a-b22b-44f9-a35d-0475cda2c78a"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("e35192bb-2490-4166-8c86-1084231e0898"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec09f1f9-bffd-48e4-8a89-526b6abf4faf"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ef07e888-175b-46f9-bd3e-e2581c13bef2"));
        }
    }
}
