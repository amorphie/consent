using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class Task200664SeedOBEventTypeSourceTypeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
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
        }
    }
}
