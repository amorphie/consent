using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task200574EventSubscriptionModuleName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("0eb17791-f3a0-4b2c-b953-2454f90b80a3"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("1bc7080e-ea78-4b7b-8cb4-72006a22d78d"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("249d199d-9365-4eff-8932-7be46247cb5b"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("308e1243-207f-48e8-82fb-19f68659f248"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("3e56c5b6-b779-4961-8870-ce79ed226b79"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("439bb1e1-2796-4685-a0dc-634e8646d655"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("4f39cb46-a78e-4da8-84a2-ad01af3ef702"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("651b1893-076e-4298-8b9e-742a2cf03af2"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("c1dc6049-4879-4378-9c41-c56641abd408"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("fa46a41f-7f62-4fe5-b95e-542b3c9cc422"));

            migrationBuilder.AddColumn<string>(
                name: "ModuleName",
                table: "OBEventSubscriptions",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "OBEventTypeSourceTypeRelations",
                columns: new[] { "Id", "APIToGetData", "CreatedAt", "CreatedBy", "CreatedByBehalfOf", "EventCase", "EventNotificationReporter", "EventNotificationTime", "EventType", "ModifiedAt", "ModifiedBy", "ModifiedByBehalfOf", "RetryCount", "RetryInMinute", "RetryPolicy", "SourceNumber", "SourceType", "YOSRole" },
                values: new object[,]
                {
                    { new Guid("00181d7e-d3ad-4b41-98dd-4cd35bdef03d"), "GET /yetkilendirme-kodu?rizaNo={rizaNo}}&rizaTip=H", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7430), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladığında rıza oluşturulur. YÖS'e rıza oluşturulduğuna dair bildirim yapılır. YÖS yetkod değerini sorgulama sonucunda elde eder.", "HHS", "Anlık", "AYRIK_GKD_BASARILI", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7430), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "HESAP_BILGISI_RIZASI", "HBH" },
                    { new Guid("535613ef-3ca6-456a-bfe1-3e48b98f0ae0"), "GET /odeme-emri/{odemeEmriNo}", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7300), new Guid("00000000-0000-0000-0000-000000000000"), null, "Tüm ödeme durum değişikliklerinde", "HHS", "Anlık", "KAYNAK_GUNCELLENDI", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7300), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 30, "30 Dakika - 3 Deneme", "odemeEmriNo", "ODEME_EMRI", "ÖBH" },
                    { new Guid("603efc6d-dd8a-4bff-91b8-d876fe41616d"), "", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7380), new Guid("00000000-0000-0000-0000-000000000000"), null, "İlgili API İlke ve kurallarına eklendiğinde güncellenecektir.", "HHS", "", "KAYNAK_GUNCELLENDI", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7380), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 30, "30 Dakika - 3 Deneme", "", "COKLU_ISLEM_TALEBI ( bulk-data)", "HBH" },
                    { new Guid("6b478182-360d-4796-9fc1-ab5a31152767"), "GET /hesaplar/{hspRef}/bakiye", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7360), new Guid("00000000-0000-0000-0000-000000000000"), null, "Bakiye nesnesindeki tutarla ilgili bir bilgi değiştiğinde ve HBH rızası içerisinde \"06-Anlık Bakiye Bildirimi\" izin türü varsa oluşturulur.\n\nMevcutta alınmış rızalar için bakiye kaynak tipi özelinde 06 izin türü gerektiğinden; mevcut rızanın yenilenmesine dair müşteriye bilgilendirme yapılarak 06 izin türünü kapsayan yeni rıza alınması süreci YÖS tarafından gerçekleştirilebilir.\n\nBloke tutar değişikliği için olay oluşturma ve bildirimi HHS inisiyatifindedir.\n\nKrdHsp içerisinde yer alan kulKrdTtr değerinin değiştiği durumda olay bildirim gönderilmesi gerekmektedir.", "HHS", "Maksimum 10 dakika içerisinde", "KAYNAK_GUNCELLENDI", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7360), new Guid("00000000-0000-0000-0000-000000000000"), null, null, null, "Retry policy uygulanmamalıdır. İlk istek gönderilemediği durumda İletilemeyen Olaylara eklenmelidir.", "hspRef", "BAKIYE", "HBH" },
                    { new Guid("718d4891-59e2-4883-af45-17f959275bd3"), "GET /odeme-emri-rizasi/{RizaNo}", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7450), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladıktan sonra yaptığı kontroller neticesinde logine izin vermez ise YÖS'e bildirim yapılır. YÖS rıza durumunu sorgulayarak işlemin neden iletilmediğine dair bilgi edinebilir.", "HHS", "Anlık", "AYRIK_GKD_BASARISIZ", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7450), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "ODEME_EMRI_RIZASI", "ÖBH" },
                    { new Guid("a159e3ed-6551-4556-84a6-064fae1e4bb1"), "GET /hesap-bilgisi-rizasi/{RizaNo}", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7330), new Guid("00000000-0000-0000-0000-000000000000"), null, "Rıza iptal detay kodu ‘02’ : Kullanıcı İsteği ile HHS üzerinden İptal durumunda", "HHS", "Anlık", "KAYNAK_GUNCELLENDI", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7340), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 30, "30 Dakika - 3 Deneme", "RizaNo", "HESAP_BILGISI_RIZASI", "HBH" },
                    { new Guid("e404b6e8-725d-4756-9b99-82504fae20ad"), "GET /hhs/{hhsKod}", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7490), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS bilgilerinde değişiklik olduğunda, YÖS'ün hhsKod ile sorgulama yapması ve değişen bilgiyi güncellemesi beklenmektedir", "BKM", "Anlık", "HHS_YOS_GUNCELLENDI", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7490), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 5, "5 Dakika - 3 Deneme", "hhsKod", "HHS", "YÖS" },
                    { new Guid("f02fc213-82c6-4645-8c4e-ee9853c18bfc"), "GET /yetkilendirme-kodu?rizaNo={rizaNo}}&rizaTip=O", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7400), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladığında rıza oluşturulur. YÖS'e rıza oluşturulduğuna dair bildirim yapılır. YÖS yetkod değerini sorgulama sonucunda elde eder.", "HHS", "Anlık", "AYRIK_GKD_BASARILI", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7400), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "ODEME_EMRI_RIZASI", "ÖBH" },
                    { new Guid("f63c0519-e202-44dd-ab3f-f1ffff1aeb54"), "GET /yos/{yosKod}", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7520), new Guid("00000000-0000-0000-0000-000000000000"), null, "YÖS bilgilerinde değişiklik olduğunda, HHS'nin yosKod ile sorgulama yapması ve değişen bilgiyi güncellemesi beklenmektedir.", "BKM", "Anlık", "HHS_YOS_GUNCELLENDI", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7520), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 5, "5 Dakika - 3 Deneme", "yosKod", "YÖS", "HHS" },
                    { new Guid("f6a3b98f-ad1d-451c-b118-6a9255ddfd7a"), "GET /hesap-bilgisi-rizasi/{RizaNo}", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7470), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladıktan sonra yaptığı kontroller neticesinde logine izin vermez ise YÖS'e bildirim yapılır. YÖS rıza durumunu sorgulayarak işlemin neden iletilmediğine dair bilgi edinebilir.", "HHS", "Anlık", "AYRIK_GKD_BASARISIZ", new DateTime(2024, 1, 7, 19, 29, 31, 746, DateTimeKind.Utc).AddTicks(7470), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "HESAP_BILGISI_RIZASI", "HBH" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("00181d7e-d3ad-4b41-98dd-4cd35bdef03d"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("535613ef-3ca6-456a-bfe1-3e48b98f0ae0"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("603efc6d-dd8a-4bff-91b8-d876fe41616d"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("6b478182-360d-4796-9fc1-ab5a31152767"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("718d4891-59e2-4883-af45-17f959275bd3"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("a159e3ed-6551-4556-84a6-064fae1e4bb1"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("e404b6e8-725d-4756-9b99-82504fae20ad"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("f02fc213-82c6-4645-8c4e-ee9853c18bfc"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("f63c0519-e202-44dd-ab3f-f1ffff1aeb54"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("f6a3b98f-ad1d-451c-b118-6a9255ddfd7a"));

            migrationBuilder.DropColumn(
                name: "ModuleName",
                table: "OBEventSubscriptions");

            migrationBuilder.InsertData(
                table: "OBEventTypeSourceTypeRelations",
                columns: new[] { "Id", "APIToGetData", "CreatedAt", "CreatedBy", "CreatedByBehalfOf", "EventCase", "EventNotificationReporter", "EventNotificationTime", "EventType", "ModifiedAt", "ModifiedBy", "ModifiedByBehalfOf", "RetryCount", "RetryInMinute", "RetryPolicy", "SourceNumber", "SourceType", "YOSRole" },
                values: new object[,]
                {
                    { new Guid("0eb17791-f3a0-4b2c-b953-2454f90b80a3"), "GET /odeme-emri-rizasi/{RizaNo}", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(4260), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladıktan sonra yaptığı kontroller neticesinde logine izin vermez ise YÖS'e bildirim yapılır. YÖS rıza durumunu sorgulayarak işlemin neden iletilmediğine dair bilgi edinebilir.", "HHS", "Anlık", "AYRIK_GKD_BASARISIZ", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(4260), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "ODEME_EMRI_RIZASI", "ÖBH" },
                    { new Guid("1bc7080e-ea78-4b7b-8cb4-72006a22d78d"), "GET /yos/{yosKod}", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(4330), new Guid("00000000-0000-0000-0000-000000000000"), null, "YÖS bilgilerinde değişiklik olduğunda, HHS'nin yosKod ile sorgulama yapması ve değişen bilgiyi güncellemesi beklenmektedir.", "BKM", "Anlık", "HHS_YOS_GUNCELLENDI", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(4330), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 5, "5 Dakika - 3 Deneme", "yosKod", "YÖS", "HHS" },
                    { new Guid("249d199d-9365-4eff-8932-7be46247cb5b"), "GET /yetkilendirme-kodu?rizaNo={rizaNo}}&rizaTip=H", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(4230), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladığında rıza oluşturulur. YÖS'e rıza oluşturulduğuna dair bildirim yapılır. YÖS yetkod değerini sorgulama sonucunda elde eder.", "HHS", "Anlık", "AYRIK_GKD_BASARILI", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(4230), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "HESAP_BILGISI_RIZASI", "HBH" },
                    { new Guid("308e1243-207f-48e8-82fb-19f68659f248"), "GET /yetkilendirme-kodu?rizaNo={rizaNo}}&rizaTip=O", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(4150), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladığında rıza oluşturulur. YÖS'e rıza oluşturulduğuna dair bildirim yapılır. YÖS yetkod değerini sorgulama sonucunda elde eder.", "HHS", "Anlık", "AYRIK_GKD_BASARILI", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(4150), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "ODEME_EMRI_RIZASI", "ÖBH" },
                    { new Guid("3e56c5b6-b779-4961-8870-ce79ed226b79"), "GET /hesap-bilgisi-rizasi/{RizaNo}", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(3800), new Guid("00000000-0000-0000-0000-000000000000"), null, "Rıza iptal detay kodu ‘02’ : Kullanıcı İsteği ile HHS üzerinden İptal durumunda", "HHS", "Anlık", "KAYNAK_GUNCELLENDI", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(3800), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 30, "30 Dakika - 3 Deneme", "RizaNo", "HESAP_BILGISI_RIZASI", "HBH" },
                    { new Guid("439bb1e1-2796-4685-a0dc-634e8646d655"), "GET /hesap-bilgisi-rizasi/{RizaNo}", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(4280), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladıktan sonra yaptığı kontroller neticesinde logine izin vermez ise YÖS'e bildirim yapılır. YÖS rıza durumunu sorgulayarak işlemin neden iletilmediğine dair bilgi edinebilir.", "HHS", "Anlık", "AYRIK_GKD_BASARISIZ", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(4280), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "HESAP_BILGISI_RIZASI", "HBH" },
                    { new Guid("4f39cb46-a78e-4da8-84a2-ad01af3ef702"), "GET /hhs/{hhsKod}", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(4310), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS bilgilerinde değişiklik olduğunda, YÖS'ün hhsKod ile sorgulama yapması ve değişen bilgiyi güncellemesi beklenmektedir", "BKM", "Anlık", "HHS_YOS_GUNCELLENDI", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(4310), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 5, "5 Dakika - 3 Deneme", "hhsKod", "HHS", "YÖS" },
                    { new Guid("651b1893-076e-4298-8b9e-742a2cf03af2"), "", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(3870), new Guid("00000000-0000-0000-0000-000000000000"), null, "İlgili API İlke ve kurallarına eklendiğinde güncellenecektir.", "HHS", "", "KAYNAK_GUNCELLENDI", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(3870), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 30, "30 Dakika - 3 Deneme", "", "COKLU_ISLEM_TALEBI ( bulk-data)", "HBH" },
                    { new Guid("c1dc6049-4879-4378-9c41-c56641abd408"), "GET /hesaplar/{hspRef}/bakiye", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(3820), new Guid("00000000-0000-0000-0000-000000000000"), null, "Bakiye nesnesindeki tutarla ilgili bir bilgi değiştiğinde ve HBH rızası içerisinde \"06-Anlık Bakiye Bildirimi\" izin türü varsa oluşturulur.\n\nMevcutta alınmış rızalar için bakiye kaynak tipi özelinde 06 izin türü gerektiğinden; mevcut rızanın yenilenmesine dair müşteriye bilgilendirme yapılarak 06 izin türünü kapsayan yeni rıza alınması süreci YÖS tarafından gerçekleştirilebilir.\n\nBloke tutar değişikliği için olay oluşturma ve bildirimi HHS inisiyatifindedir.\n\nKrdHsp içerisinde yer alan kulKrdTtr değerinin değiştiği durumda olay bildirim gönderilmesi gerekmektedir.", "HHS", "Maksimum 10 dakika içerisinde", "KAYNAK_GUNCELLENDI", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(3820), new Guid("00000000-0000-0000-0000-000000000000"), null, null, null, "Retry policy uygulanmamalıdır. İlk istek gönderilemediği durumda İletilemeyen Olaylara eklenmelidir.", "hspRef", "BAKIYE", "HBH" },
                    { new Guid("fa46a41f-7f62-4fe5-b95e-542b3c9cc422"), "GET /odeme-emri/{odemeEmriNo}", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(3760), new Guid("00000000-0000-0000-0000-000000000000"), null, "Tüm ödeme durum değişikliklerinde", "HHS", "Anlık", "KAYNAK_GUNCELLENDI", new DateTime(2024, 1, 5, 20, 3, 36, 157, DateTimeKind.Utc).AddTicks(3760), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 30, "30 Dakika - 3 Deneme", "odemeEmriNo", "ODEME_EMRI", "ÖBH" }
                });
        }
    }
}
