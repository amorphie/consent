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
                    { new Guid("02ac284d-169a-4ba2-90fc-a1c767f5c6a3"), "TR.OHVPS.Resource.InvalidFormat", 110, "hesapIslemBtsTrh can not be later than enquiry datetime.", "hesapIslemBtsTrh sorgulama zamanından sonra olamaz." },
                    { new Guid("07871055-f172-4853-b369-42e582b0aa4d"), "TR.OHVPS.Resource.NotFound", 150, "Resource not found", "Kaynak bulunamadı." },
                    { new Guid("07c20c1a-cbe6-414e-ab0b-f2b3a39154be"), "TR.OHVPS.Field.Invalid", 12, "size must be 26", "boyut '26' olmalı" },
                    { new Guid("0f6af59f-3695-4599-a724-e20077ad1ed1"), "TR.OHVPS.Field.Invalid", 8, "size must be between 1-30", "boyut '1' ile  '30' arasında olmalı" },
                    { new Guid("156e8aed-b41d-444f-af59-de557a70fb6a"), "TR.OHVPS.Field.Invalid", 9, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("22758959-655b-4136-9a6d-400e27160eaf"), "TR.OHVPS.Field.Invalid", 19, "Detail transactions permission can not be selected without Basic transactions.", "Temelişlem bilgisi izni seçimi yapılmadan ayrıntılı işlem bilgisi seçimi yapılamaz.." },
                    { new Guid("24113782-4116-42b0-9b34-40ec3e76c7f7"), "TR.OHVPS.Field.Invalid", 21, "The minimum value it can take is consent date +1 day, the maximum value it can take is: Consent date + 6 months.", "Alabileceği minimum değer tarihi +1 gün, alabileceği maksimum değer : Rıza tarihi + 6 ay " },
                    { new Guid("34e053e3-9a70-4082-9040-3f4efd33e649"), "TR.OHVPS.Business.InvalidContent", 200, "PsuInitiated invalid", "PsuInitiated değeri hatalı." },
                    { new Guid("35b0e4ff-f937-4ba6-a727-d4ed707c8265"), "TR.OHVPS.Field.Invalid", 5, "size must be between 1-30", "boyut '1' ile  '30' arasında olmalı" },
                    { new Guid("361c5a8d-837f-4807-a6a4-dc8d61337cd3"), "TR.OHVPS.Resource.ConsentRevoked", 164, "Consent state is not authorization used. Not valid to process.", "Rıza durumu yetki kullanıldı olmadığı için işlem yapılamaz." },
                    { new Guid("5351a02e-2a59-45e7-9509-8e6a37051d4f"), "TR.OHVPS.Resource.InvalidFormat", 108, "srlmYon value is not valid.", "srlmYon değeri geçersiz." },
                    { new Guid("579047dd-e9b4-48ba-bcf6-f7b94cdd3a22"), "TR.OHVPS.Field.Invalid", 17, "size must be 10", "boyut '10' olmalı" },
                    { new Guid("5a4450ae-8e80-4405-9557-bc6cff791b40"), "TR.OHVPS.Resource.InvalidFormat", 115, "brcAlc value is not valid.", "brcAlc değeri geçersiz." },
                    { new Guid("60f86d3f-4c2d-4c9b-92f0-1e0c553ab03a"), "TR.OHVPS.Field.Invalid", 6, "size must be between 1-8", "boyut '1' ile '8' arasında olmalı" },
                    { new Guid("6ef3e43d-3313-4c9f-8e58-83ba1942575e"), "TR.OHVPS.Server.InternalError", 152, "By Checking Idempotency Unexpected condition was encountered.", "Idempotency kontrol edilirken beklenmeyen bir durumla karşılaşıldı." },
                    { new Guid("725f5be5-4fa6-49ea-b831-467008f9ebd7"), "TR.OHVPS.Business.EventSubscriptionNotFound", 105, "No evet subscription for AYRIK_GKD_BASARILI and AYRIK_GKD_BASARISIZ.", "AYRIK_GKD_BASARILI ve AYRIK_GKD_BASARISIZ olay tipleri için olay aboneliği yapılmalıdır." },
                    { new Guid("841bf229-a9ac-4fb1-9e20-1ee6fdd223d8"), "TR.OHVPS.Field.Missing", 2, "must not be null", "boş değer olamaz" },
                    { new Guid("87b8e90f-43b9-4c53-bb6e-128f2fee17a3"), "TR.OHVPS.Resource.InvalidFormat", 106, "syfKytSayi value is not valid. syfKytSayi can be between 1-100", "syfKytSayi değeri geçersiz. 1-100 aralığında olabilir." },
                    { new Guid("88e2b05a-1e69-47fd-bbf9-c26e82aadddc"), "TR.OHVPS.Resource.InvalidFormat", 111, "hesapIslemBtsTrh can not be early than hesapIslemBslTrh.", "hesapIslemBtsTrh hesapIslemBslTrh den önce olamaz." },
                    { new Guid("8e32e9e1-ea0d-4b71-a61a-82f049e2abad"), "TR.OHVPS.Resource.InvalidFormat", 100, "Validation error", "Şema kontrolleri başarısız" },
                    { new Guid("9744a249-d6b7-4404-af8e-1bffb14d944a"), "TR.OHVPS.Resource.InvalidFormat", 114, "For system enquiry, last 24 hours can be enquirable.", "sistemsel yapılan sorgulamalarda hem bireysel, hem de kurumsal ÖHK’lar için;son 24 saat sorgulanabilir." },
                    { new Guid("992e3262-fe09-4f68-8cf4-e0ba3dc0f90c"), "TR.OHVPS.Resource.InvalidFormat", 112, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 month.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark bireysel ÖHK’lar için en fazla 1 ay olabilir." },
                    { new Guid("9cea46aa-7b58-4ef8-a697-8d959685fc4c"), "TR.OHVPS.Resource.InvalidFormat", 117, "minIslTtr value is not valid.", "minIslTtr değeri geçersiz." },
                    { new Guid("a6da080d-4cc4-4e72-8042-d7d8f9fc26b8"), "TR.OHVPS.Business.InvalidContent", 202, "OpenBanking Consent Id in header is invalid.", "OpenBanking Consent Id değeri geçersiz." },
                    { new Guid("afa15290-1e59-49f3-a468-2106591239c9"), "TR.OHVPS.Field.Invalid", 22, "When the Basic Transaction Information/Detailed Transaction Information permission is selected, the hesapIslemBslZmn hesapIslemBtsZmn fields must be filled.", "Temel İşlem bilgisi/ayrıntlı işlem bilgisi izni seçilmiş olduğu zaman hesapIslemBslZmn hesapIslemBtsZmn alanlarının doldurulması zorunludur." },
                    { new Guid("b5710db9-4d09-486c-adc4-4e925596e201"), "TR.OHVPS.Field.Invalid", 3, "Invalid data.", "Geçersiz veri." },
                    { new Guid("b7fef618-dd1e-40d6-8822-0eb73801b4b7"), "TR.OHVPS.Field.Invalid", 13, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("c0425ded-7786-4f4f-ad15-ff899dd44a1d"), "TR.OHVPS.Resource.ConsentRevoked", 163, "Consent ended. Not valid to process.", "Sonlandırılmış rıza için işlem yapılamaz." },
                    { new Guid("c06ed343-0360-48a9-a166-552036cd8b0c"), "TR.OHVPS.Resource.ConsentMismatch", 160, "Consent not valid to process", "Consent işlem yapılmaya uygun değil." },
                    { new Guid("c2f14416-e3c3-44d9-bdcb-8d1570479d82"), "TR.OHVPS.Resource.InvalidFormat", 118, "mksIslTtr value is not valid.", "mksIslTtr değeri geçersiz." },
                    { new Guid("c6ac0347-c639-486e-b7e5-99fa5230264d"), "TR.OHVPS.Field.Invalid", 16, "size must be between 1-9", "boyut '1' ile  '9' arasında olmalı" },
                    { new Guid("c6c61977-dc31-4e7e-b075-23bea1a5c7a2"), "TR.OHVPS.Field.Invalid", 10, "size must be between 1-9", "boyut '1' ile  '9' arasında olmalı" },
                    { new Guid("c86ab026-813a-4c04-a7af-8e7ac1fc8e82"), "TR.OHVPS.Connection.InvalidTPP", 102, "Invalid TPP Code", "Geçersiz Yos kodu." },
                    { new Guid("cc536003-e51e-48eb-a26a-24cd09ef1e25"), "TR.OHVPS.Field.Invalid", 14, "size must be between 1-30", "boyut '1' ile  '30' arasında olmalı" },
                    { new Guid("cd238a9b-0a8d-40c9-908f-491f51638865"), "TR.OHVPS.Field.Invalid", 24, "Minimum date: Date of consent given – 12 months Maximum date: Date of consent given + 12 months", "Minimum tarih : Rızanın veriliş tarihi – 12 ay Maksimum tarih : Rızanın veriliş tarihi + 12 ay" },
                    { new Guid("d475571b-0308-4f96-b733-91f51e99340f"), "TR.OHVPS.Field.Invalid", 11, "size must be 10", "boyut '10' olmalı" },
                    { new Guid("d6905ef6-9d21-4c82-a2ab-ec18b8860341"), "TR.OHVPS.Field.Invalid", 23, "hesapIslemBslZmn hesapIslemBtsZmn fields should only be sent when the Basic Transaction Information/Detailed Transaction Information permission is selected.", "hesapIslemBslZmn hesapIslemBtsZmn alanları sadece Temel İşlem bilgisi/ayrıntlı işlem bilgisi izni seçilmiş olduğu zaman gönderilmelidir." },
                    { new Guid("d6f908c8-b580-4525-9cab-4e84b9b3f321"), "TR.OHVPS.Resource.InvalidFormat", 107, "srlmKrtr value is not valid. it should be hspRef", "srlmKrtr değeri geçersiz. Olması gereken değer hspRef." },
                    { new Guid("d815a9bb-2f04-4cb7-a7e9-a27c13fda62e"), "TR.OHVPS.Field.Invalid", 24, "Minimum date: Date of consent given – 12 months Maximum date: Date of consent given + 12 months", "Minimum tarih : Rızanın veriliş tarihi – 12 ay Maksimum tarih : Rızanın veriliş tarihi + 12 ay" },
                    { new Guid("dd1033b1-7fe1-4e7b-bb29-d9e6cf351fe7"), "TR.OHVPS.Field.Invalid", 4, "size must be 4", "boyut '4' olmalı" },
                    { new Guid("deb787b8-54b5-4142-aed5-0faaa4a01274"), "TR.OHVPS.Resource.ConsentMismatch", 161, "Consent state not valid to delete", "Consent rıza durumu silme işlemine uygun değil." },
                    { new Guid("e333d225-6f95-4970-b449-366b791b588f"), "TR.OHVPS.Resource.ConsentMismatch", 162, "There is a Authorized / Authorzation Used consent in the system. First cancel the consent.", "Sistemde Yetkilendirildi / Yetki Kullanıldı durumunda rıza olduğu için rıza kabul edilmedi. Öncelikli olarak rızayı iptal ediniz." },
                    { new Guid("e8c3ff38-8175-4695-a22e-e1c676105af5"), "TR.OHVPS.Server.InternalError", 151, "Unexpected condition was encountered.", "Beklenmeyen bir durumla karşılaşıldı." },
                    { new Guid("ea93d68c-db26-4dd7-bca7-24598cace8b0"), "TR.OHVPS.Resource.InvalidFormat", 116, "srlmKrtr value is not valid. it should be islGrckZaman", "srlmKrtr değeri geçersiz. Olması gereken değer islGrckZaman." },
                    { new Guid("efe0e6db-bb22-4803-8713-72ec0139fa55"), "TR.OHVPS.Field.Invalid", 7, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("f07ec1e2-b90e-4095-9bd4-92335da4ed4d"), "TR.OHVPS.Field.Invalid", 20, "Instant balance notification cannot be selected without selecting balance information permission.", "Bakiye bilgisi izni seçimi yapılmadan anlık bakiye bildirimi seçimi yapılamaz.." },
                    { new Guid("f733643d-fb83-440a-a2c5-66809aa30acf"), "TR.OHVPS.Resource.InvalidFormat", 109, "hesapIslemBtsTrh,hesapIslemBslTrh values not valid", "hesapIslemBtsTrh,hesapIslemBslTrh değerleri geçersiz." },
                    { new Guid("f76a86ee-ef8c-43b9-996e-e8c6d258e158"), "TR.OHVPS.Resource.InvalidFormat", 113, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 week.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark kurumsal ÖHK’lar için en fazla 1 hafta olabilir." },
                    { new Guid("f76bec1e-f329-4767-86a9-0c5747002354"), "TR.OHVPS.Field.Invalid", 15, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("f7b66a7d-d2bf-4686-a8ea-bf4314da810b"), "TR.OHVPS.Field.Invalid", 25, "hesapIslemBslZmn can not be later than hesapIslemBtsZmn.", "hesapIslemBslZmn, hesapIslemBtsZmn verisinden sonra olamaz. " },
                    { new Guid("fb8f8ad6-dd0b-44d4-8508-bc9285567ba7"), "TR.OHVPS.Field.Invalid", 18, "Temel hesap bilgisi izni must.", "Temel hesap bilgisi izni seçimi zorunludur." },
                    { new Guid("fce8b4e0-133a-4abd-b1a5-8e541abf9e51"), "TR.OHVPS.Business.InvalidContent", 201, "User reference in header is wrong.", "User reference değeri geçersiz." },
                    { new Guid("fdfb334f-6839-44bd-a811-fcda2dd3dba8"), "TR.OHVPS.Business.CustomerInfoMismatch", 104, "kmlk.kmlkVrs - ayrikGkd.ohkTanimDeger must match.", "kmlk.kmlkVrs - ayrikGkd.ohkTanimDeger aynı olmalı." },
                    { new Guid("fe36bb90-8d1e-45ac-a817-7ed57599352f"), "TR.OHVPS.Connection.InvalidASPSP", 101, "Invalid ASPSP Code", "Geçersiz HHS kodu." }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("02ac284d-169a-4ba2-90fc-a1c767f5c6a3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("07871055-f172-4853-b369-42e582b0aa4d"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("07c20c1a-cbe6-414e-ab0b-f2b3a39154be"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("0f6af59f-3695-4599-a724-e20077ad1ed1"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("156e8aed-b41d-444f-af59-de557a70fb6a"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("22758959-655b-4136-9a6d-400e27160eaf"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("24113782-4116-42b0-9b34-40ec3e76c7f7"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("34e053e3-9a70-4082-9040-3f4efd33e649"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("35b0e4ff-f937-4ba6-a727-d4ed707c8265"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("361c5a8d-837f-4807-a6a4-dc8d61337cd3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("5351a02e-2a59-45e7-9509-8e6a37051d4f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("579047dd-e9b4-48ba-bcf6-f7b94cdd3a22"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("5a4450ae-8e80-4405-9557-bc6cff791b40"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("60f86d3f-4c2d-4c9b-92f0-1e0c553ab03a"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("6ef3e43d-3313-4c9f-8e58-83ba1942575e"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("725f5be5-4fa6-49ea-b831-467008f9ebd7"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("841bf229-a9ac-4fb1-9e20-1ee6fdd223d8"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("87b8e90f-43b9-4c53-bb6e-128f2fee17a3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("88e2b05a-1e69-47fd-bbf9-c26e82aadddc"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("8e32e9e1-ea0d-4b71-a61a-82f049e2abad"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("9744a249-d6b7-4404-af8e-1bffb14d944a"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("992e3262-fe09-4f68-8cf4-e0ba3dc0f90c"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("9cea46aa-7b58-4ef8-a697-8d959685fc4c"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("a6da080d-4cc4-4e72-8042-d7d8f9fc26b8"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("afa15290-1e59-49f3-a468-2106591239c9"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b5710db9-4d09-486c-adc4-4e925596e201"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b7fef618-dd1e-40d6-8822-0eb73801b4b7"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("c0425ded-7786-4f4f-ad15-ff899dd44a1d"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("c06ed343-0360-48a9-a166-552036cd8b0c"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("c2f14416-e3c3-44d9-bdcb-8d1570479d82"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("c6ac0347-c639-486e-b7e5-99fa5230264d"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("c6c61977-dc31-4e7e-b075-23bea1a5c7a2"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("c86ab026-813a-4c04-a7af-8e7ac1fc8e82"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("cc536003-e51e-48eb-a26a-24cd09ef1e25"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("cd238a9b-0a8d-40c9-908f-491f51638865"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("d475571b-0308-4f96-b733-91f51e99340f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("d6905ef6-9d21-4c82-a2ab-ec18b8860341"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("d6f908c8-b580-4525-9cab-4e84b9b3f321"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("d815a9bb-2f04-4cb7-a7e9-a27c13fda62e"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("dd1033b1-7fe1-4e7b-bb29-d9e6cf351fe7"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("deb787b8-54b5-4142-aed5-0faaa4a01274"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("e333d225-6f95-4970-b449-366b791b588f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("e8c3ff38-8175-4695-a22e-e1c676105af5"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ea93d68c-db26-4dd7-bca7-24598cace8b0"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("efe0e6db-bb22-4803-8713-72ec0139fa55"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f07ec1e2-b90e-4095-9bd4-92335da4ed4d"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f733643d-fb83-440a-a2c5-66809aa30acf"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f76a86ee-ef8c-43b9-996e-e8c6d258e158"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f76bec1e-f329-4767-86a9-0c5747002354"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f7b66a7d-d2bf-4686-a8ea-bf4314da810b"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("fb8f8ad6-dd0b-44d4-8508-bc9285567ba7"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("fce8b4e0-133a-4abd-b1a5-8e541abf9e51"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("fdfb334f-6839-44bd-a811-fcda2dd3dba8"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("fe36bb90-8d1e-45ac-a817-7ed57599352f"));

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
    }
}
