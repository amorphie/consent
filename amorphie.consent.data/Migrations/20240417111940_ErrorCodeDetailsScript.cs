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
                    { new Guid("00d5f845-2c6e-488f-bb89-ce587bceb519"), "TR.OHVPS.Server.InternalError", 151, "Unexpected condition was encountered.", "Beklenmeyen bir durumla karşılaşıldı." },
                    { new Guid("075ebaae-d065-4ce6-94cb-fcd819145cc4"), "TR.OHVPS.Field.Invalid", 19, "Detail transactions permission can not be selected without Basic transactions.", "Temelişlem bilgisi izni seçimi yapılmadan ayrıntılı işlem bilgisi seçimi yapılamaz.." },
                    { new Guid("0e1d4db6-03c9-4015-bcff-cc30ccad9566"), "TR.OHVPS.Field.Invalid", 6, "size must be between 1-8", "boyut '1' ile '8' arasında olmalı" },
                    { new Guid("16f73fe3-92f2-4b97-9b7a-f2ea0bedef71"), "TR.OHVPS.Resource.ConsentMismatch", 161, "Consent state not valid to delete", "Consent rıza durumu silme işlemine uygun değil." },
                    { new Guid("1a3334a3-c17a-4d48-aae8-23631e494595"), "TR.OHVPS.Resource.ConsentRevoked", 163, "Consent ended. Not valid to process.", "Sonlandırılmış rıza için işlem yapılamaz." },
                    { new Guid("2d6f4351-6994-42c7-a260-b9539254febd"), "TR.OHVPS.Resource.MissingSignature", 300, "Header x-jws-signature property is empty.", "İstek başlığında x-jws-signature alanı eksik." },
                    { new Guid("3209a533-a7a3-4f9c-a2d9-cf2f41733d3c"), "TR.OHVPS.Resource.InvalidFormat", 108, "srlmYon value is not valid.", "srlmYon değeri geçersiz." },
                    { new Guid("34716ab5-b839-4a34-8200-a904589ec549"), "TR.OHVPS.Resource.InvalidFormat", 115, "brcAlc value is not valid.", "brcAlc değeri geçersiz." },
                    { new Guid("376dee57-c554-40d2-8c33-4ea3bdd09860"), "TR.OHVPS.Resource.InvalidFormat", 116, "srlmKrtr value is not valid. it should be islGrckZaman", "srlmKrtr değeri geçersiz. Olması gereken değer islGrckZaman." },
                    { new Guid("4c52e931-63af-49b7-b5ef-09843181c7f8"), "TR.OHVPS.Field.Invalid", 18, "Temel hesap bilgisi izni must.", "Temel hesap bilgisi izni seçimi zorunludur." },
                    { new Guid("4eb9ecd7-5076-45fe-a2c7-ccc1bdac710c"), "TR.OHVPS.Resource.InvalidFormat", 109, "hesapIslemBtsTrh,hesapIslemBslTrh values not valid", "hesapIslemBtsTrh,hesapIslemBslTrh değerleri geçersiz." },
                    { new Guid("53057c89-eff3-4444-9762-2f64789c46bf"), "TR.OHVPS.Field.Invalid", 11, "size must be 10", "boyut '10' olmalı" },
                    { new Guid("56446392-e762-4f50-ab93-ae1243cd4fbb"), "TR.OHVPS.Field.Invalid", 24, "Minimum date: Date of consent given – 12 months Maximum date: Date of consent given + 12 months", "Minimum tarih : Rızanın veriliş tarihi – 12 ay Maksimum tarih : Rızanın veriliş tarihi + 12 ay" },
                    { new Guid("57e78d29-b372-425c-9df9-6b0a3c8afd82"), "TR.OHVPS.Field.Invalid", 7, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("5949a56a-d55d-454f-9fbd-6b74f19243a3"), "TR.OHVPS.Field.Invalid", 13, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("5acaa8dc-8611-4e0a-9ce1-5f44d6b29d9a"), "TR.OHVPS.Field.Invalid", 23, "hesapIslemBslZmn hesapIslemBtsZmn fields should only be sent when the Basic Transaction Information/Detailed Transaction Information permission is selected.", "hesapIslemBslZmn hesapIslemBtsZmn alanları sadece Temel İşlem bilgisi/ayrıntlı işlem bilgisi izni seçilmiş olduğu zaman gönderilmelidir." },
                    { new Guid("5fbeb808-9241-4fa7-bfc9-bbfc63e380e0"), "TR.OHVPS.Business.InvalidContent", 202, "OpenBanking Consent Id in header is invalid.", "OpenBanking Consent Id değeri geçersiz." },
                    { new Guid("610ce78f-79fb-4795-88dd-f2884316e396"), "TR.OHVPS.Resource.InvalidFormat", 114, "For system enquiry, last 24 hours can be enquirable.", "sistemsel yapılan sorgulamalarda hem bireysel, hem de kurumsal ÖHK’lar için;son 24 saat sorgulanabilir." },
                    { new Guid("6dc3924c-0064-4c84-b965-a744acbcf4ec"), "TR.OHVPS.Field.Invalid", 16, "size must be between 1-9", "boyut '1' ile  '9' arasında olmalı" },
                    { new Guid("6f3e094e-c716-4cd6-8372-06298191e402"), "TR.OHVPS.Resource.InvalidFormat", 113, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 week.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark kurumsal ÖHK’lar için en fazla 1 hafta olabilir." },
                    { new Guid("703a06c9-b8ae-4a0f-be90-80e4a9b14188"), "TR.OHVPS.Resource.ConsentMismatch", 162, "There is a Authorized / Authorzation Used consent in the system. First cancel the consent.", "Sistemde Yetkilendirildi / Yetki Kullanıldı durumunda rıza olduğu için rıza kabul edilmedi. Öncelikli olarak rızayı iptal ediniz." },
                    { new Guid("7c8a625a-c465-4f18-bc76-7d618574dc58"), "TR.OHVPS.Server.InternalError", 152, "By Checking Idempotency Unexpected condition was encountered.", "Idempotency kontrol edilirken beklenmeyen bir durumla karşılaşıldı." },
                    { new Guid("7f8aa21a-93ad-4ea4-904b-1f08f05a03a7"), "TR.OHVPS.Business.InvalidContent", 201, "User reference in header is wrong.", "User reference değeri geçersiz." },
                    { new Guid("8a8d027a-087e-494c-b1d4-436ded807ce6"), "TR.OHVPS.Resource.InvalidFormat", 107, "srlmKrtr value is not valid. it should be hspRef", "srlmKrtr değeri geçersiz. Olması gereken değer hspRef." },
                    { new Guid("8de16fb2-a131-40ce-b051-8bf48c1d4a42"), "TR.OHVPS.Resource.InvalidFormat", 112, "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 month.", "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark bireysel ÖHK’lar için en fazla 1 ay olabilir." },
                    { new Guid("94ceb714-eb17-4eeb-bfb4-16716042f211"), "TR.OHVPS.Field.Invalid", 3, "Invalid data.", "Geçersiz veri." },
                    { new Guid("95afed77-f15e-4d5b-bb98-81a3666ca8e2"), "TR.OHVPS.Field.Invalid", 14, "size must be between 1-30", "boyut '1' ile  '30' arasında olmalı" },
                    { new Guid("95bf32a6-b6a9-4259-8eb7-217bd8735d63"), "TR.OHVPS.Field.Invalid", 9, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("9e480a47-d008-4391-b82d-c2125f07f9f8"), "TR.OHVPS.Resource.InvalidFormat", 106, "syfKytSayi value is not valid. syfKytSayi can be between 1-100", "syfKytSayi değeri geçersiz. 1-100 aralığında olabilir." },
                    { new Guid("9fca1229-a83b-4a9d-a323-1aba5d0cc31a"), "TR.OHVPS.Field.Invalid", 25, "hesapIslemBslZmn can not be later than hesapIslemBtsZmn.", "hesapIslemBslZmn, hesapIslemBtsZmn verisinden sonra olamaz. " },
                    { new Guid("a9c9655c-86ef-4784-9258-d1b1df0fab03"), "TR.OHVPS.Field.Invalid", 8, "size must be between 1-30", "boyut '1' ile  '30' arasında olmalı" },
                    { new Guid("aff24378-9a4f-4613-b100-5242015fff1b"), "TR.OHVPS.Business.CustomerInfoMismatch", 104, "kmlk.kmlkVrs - ayrikGkd.ohkTanimDeger must match.", "kmlk.kmlkVrs - ayrikGkd.ohkTanimDeger aynı olmalı." },
                    { new Guid("b1496b62-c8c4-4146-a1f2-35cd56f80bc2"), "TR.OHVPS.Field.Invalid", 21, "The minimum value it can take is consent date +1 day, the maximum value it can take is: Consent date + 6 months.", "Alabileceği minimum değer tarihi +1 gün, alabileceği maksimum değer : Rıza tarihi + 6 ay " },
                    { new Guid("b1e86ea3-2fe0-4c23-8395-21f8aaa53c08"), "TR.OHVPS.Resource.NotFound", 150, "Resource not found", "Kaynak bulunamadı." },
                    { new Guid("b30ad3e9-a1e2-4cdc-80b1-f18b8f214e8d"), "TR.OHVPS.Field.Invalid", 15, "size must be 11", "boyut '11' olmalı" },
                    { new Guid("b57b6eb2-d656-41ac-b499-8d3a860ab3db"), "TR.OHVPS.Resource.InvalidFormat", 117, "minIslTtr value is not valid.", "minIslTtr değeri geçersiz." },
                    { new Guid("b67b9adc-d382-468a-946d-ff19955022b2"), "TR.OHVPS.Field.Invalid", 5, "size must be between 1-30", "boyut '1' ile  '30' arasında olmalı" },
                    { new Guid("b9b627f7-2b86-40fd-9119-b2a851ab6d04"), "TR.OHVPS.Resource.InvalidFormat", 111, "hesapIslemBtsTrh can not be early than hesapIslemBslTrh.", "hesapIslemBtsTrh hesapIslemBslTrh den önce olamaz." },
                    { new Guid("b9e97485-5e54-496a-8cf1-706774268569"), "TR.OHVPS.Business.EventSubscriptionNotFound", 105, "No evet subscription for AYRIK_GKD_BASARILI and AYRIK_GKD_BASARISIZ.", "AYRIK_GKD_BASARILI ve AYRIK_GKD_BASARISIZ olay tipleri için olay aboneliği yapılmalıdır." },
                    { new Guid("bfef3ca1-8d4f-44d7-8112-5cf553654a97"), "TR.OHVPS.Resource.ConsentRevoked", 164, "Consent state is not authorization used. Not valid to process.", "Rıza durumu yetki kullanıldı olmadığı için işlem yapılamaz." },
                    { new Guid("c087390a-8c3e-49fd-93de-64ac5258a294"), "TR.OHVPS.Connection.InvalidTPP", 102, "Invalid TPP Code", "Geçersiz Yos kodu." },
                    { new Guid("c1fe7cda-68be-4bd3-b5f7-c439c3efff74"), "TR.OHVPS.Field.Missing", 2, "must not be null", "boş değer olamaz" },
                    { new Guid("d1339d35-f367-4876-890a-06ee9e888594"), "TR.OHVPS.Field.Invalid", 22, "When the Basic Transaction Information/Detailed Transaction Information permission is selected, the hesapIslemBslZmn hesapIslemBtsZmn fields must be filled.", "Temel İşlem bilgisi/ayrıntlı işlem bilgisi izni seçilmiş olduğu zaman hesapIslemBslZmn hesapIslemBtsZmn alanlarının doldurulması zorunludur." },
                    { new Guid("db7a0741-30c6-4ea2-88be-39c593cd9146"), "TR.OHVPS.Resource.InvalidFormat", 100, "Validation error", "Şema kontrolleri başarısız" },
                    { new Guid("ddfdb41c-ee40-4090-9e55-e38b98b19151"), "TR.OHVPS.Field.Invalid", 12, "size must be 26", "boyut '26' olmalı" },
                    { new Guid("e5aac2b0-d4d6-4023-8a16-89d75c42c150"), "TR.OHVPS.Field.Invalid", 10, "size must be between 1-9", "boyut '1' ile  '9' arasında olmalı" },
                    { new Guid("eb8c91fd-1663-47c6-b143-4ac964ed2ff5"), "TR.OHVPS.Field.Invalid", 4, "size must be 4", "boyut '4' olmalı" },
                    { new Guid("eba499b9-8271-40b2-82e5-624a9067603d"), "TR.OHVPS.Resource.InvalidFormat", 118, "mksIslTtr value is not valid.", "mksIslTtr değeri geçersiz." },
                    { new Guid("ec5c9910-00f5-4f64-b9e2-6d0ef4efdb7f"), "TR.OHVPS.Business.InvalidContent", 200, "PsuInitiated invalid", "PsuInitiated değeri hatalı." },
                    { new Guid("ed354dea-c822-48bd-8acd-e56276444a67"), "TR.OHVPS.Resource.InvalidFormat", 110, "hesapIslemBtsTrh can not be later than enquiry datetime.", "hesapIslemBtsTrh sorgulama zamanından sonra olamaz." },
                    { new Guid("ef42dcfe-47db-43a5-ae92-f16e830a716c"), "TR.OHVPS.Field.Invalid", 20, "Instant balance notification cannot be selected without selecting balance information permission.", "Bakiye bilgisi izni seçimi yapılmadan anlık bakiye bildirimi seçimi yapılamaz.." },
                    { new Guid("ef63595d-e335-4678-9580-b5a8f2205075"), "TR.OHVPS.Resource.ConsentMismatch", 160, "Consent not valid to process", "Consent işlem yapılmaya uygun değil." },
                    { new Guid("f3a5651d-1e73-4308-8ebc-88a73e436645"), "TR.OHVPS.Connection.InvalidASPSP", 101, "Invalid ASPSP Code", "Geçersiz HHS kodu." },
                    { new Guid("f868e2ef-9863-42df-8603-87347de8d0d6"), "TR.OHVPS.Field.Invalid", 24, "Minimum date: Date of consent given – 12 months Maximum date: Date of consent given + 12 months", "Minimum tarih : Rızanın veriliş tarihi – 12 ay Maksimum tarih : Rızanın veriliş tarihi + 12 ay" },
                    { new Guid("f955fb07-f9ba-4c31-97f9-fab7437790f8"), "TR.OHVPS.Field.Invalid", 17, "size must be 10", "boyut '10' olmalı" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("00d5f845-2c6e-488f-bb89-ce587bceb519"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("075ebaae-d065-4ce6-94cb-fcd819145cc4"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("0e1d4db6-03c9-4015-bcff-cc30ccad9566"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("16f73fe3-92f2-4b97-9b7a-f2ea0bedef71"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("1a3334a3-c17a-4d48-aae8-23631e494595"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("2d6f4351-6994-42c7-a260-b9539254febd"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("3209a533-a7a3-4f9c-a2d9-cf2f41733d3c"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("34716ab5-b839-4a34-8200-a904589ec549"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("376dee57-c554-40d2-8c33-4ea3bdd09860"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("4c52e931-63af-49b7-b5ef-09843181c7f8"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("4eb9ecd7-5076-45fe-a2c7-ccc1bdac710c"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("53057c89-eff3-4444-9762-2f64789c46bf"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("56446392-e762-4f50-ab93-ae1243cd4fbb"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("57e78d29-b372-425c-9df9-6b0a3c8afd82"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("5949a56a-d55d-454f-9fbd-6b74f19243a3"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("5acaa8dc-8611-4e0a-9ce1-5f44d6b29d9a"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("5fbeb808-9241-4fa7-bfc9-bbfc63e380e0"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("610ce78f-79fb-4795-88dd-f2884316e396"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("6dc3924c-0064-4c84-b965-a744acbcf4ec"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("6f3e094e-c716-4cd6-8372-06298191e402"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("703a06c9-b8ae-4a0f-be90-80e4a9b14188"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("7c8a625a-c465-4f18-bc76-7d618574dc58"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("7f8aa21a-93ad-4ea4-904b-1f08f05a03a7"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("8a8d027a-087e-494c-b1d4-436ded807ce6"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("8de16fb2-a131-40ce-b051-8bf48c1d4a42"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("94ceb714-eb17-4eeb-bfb4-16716042f211"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("95afed77-f15e-4d5b-bb98-81a3666ca8e2"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("95bf32a6-b6a9-4259-8eb7-217bd8735d63"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("9e480a47-d008-4391-b82d-c2125f07f9f8"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("9fca1229-a83b-4a9d-a323-1aba5d0cc31a"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("a9c9655c-86ef-4784-9258-d1b1df0fab03"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("aff24378-9a4f-4613-b100-5242015fff1b"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b1496b62-c8c4-4146-a1f2-35cd56f80bc2"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b1e86ea3-2fe0-4c23-8395-21f8aaa53c08"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b30ad3e9-a1e2-4cdc-80b1-f18b8f214e8d"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b57b6eb2-d656-41ac-b499-8d3a860ab3db"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b67b9adc-d382-468a-946d-ff19955022b2"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b9b627f7-2b86-40fd-9119-b2a851ab6d04"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("b9e97485-5e54-496a-8cf1-706774268569"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("bfef3ca1-8d4f-44d7-8112-5cf553654a97"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("c087390a-8c3e-49fd-93de-64ac5258a294"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("c1fe7cda-68be-4bd3-b5f7-c439c3efff74"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("d1339d35-f367-4876-890a-06ee9e888594"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("db7a0741-30c6-4ea2-88be-39c593cd9146"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ddfdb41c-ee40-4090-9e55-e38b98b19151"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("e5aac2b0-d4d6-4023-8a16-89d75c42c150"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("eb8c91fd-1663-47c6-b143-4ac964ed2ff5"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("eba499b9-8271-40b2-82e5-624a9067603d"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ec5c9910-00f5-4f64-b9e2-6d0ef4efdb7f"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ed354dea-c822-48bd-8acd-e56276444a67"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ef42dcfe-47db-43a5-ae92-f16e830a716c"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ef63595d-e335-4678-9580-b5a8f2205075"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f3a5651d-1e73-4308-8ebc-88a73e436645"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f868e2ef-9863-42df-8603-87347de8d0d6"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("f955fb07-f9ba-4c31-97f9-fab7437790f8"));

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
    }
}
