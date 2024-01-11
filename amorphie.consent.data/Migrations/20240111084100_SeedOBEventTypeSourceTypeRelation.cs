using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class SeedOBEventTypeSourceTypeRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "OBEventTypeSourceTypeRelations",
                columns: new[] { "Id", "APIToGetData", "CreatedAt", "CreatedBy", "CreatedByBehalfOf", "EventCase", "EventNotificationReporter", "EventNotificationTime", "EventType", "ModifiedAt", "ModifiedBy", "ModifiedByBehalfOf", "RetryCount", "RetryInMinute", "RetryPolicy", "SourceNumber", "SourceType", "YOSRole" },
                values: new object[,]
                {
                    { new Guid("1421ba33-e7ae-4c59-9182-fc68d4e82bb2"), "GET /yos/{yosKod}", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(1130), new Guid("00000000-0000-0000-0000-000000000000"), null, "YÖS bilgilerinde değişiklik olduğunda, HHS'nin yosKod ile sorgulama yapması ve değişen bilgiyi güncellemesi beklenmektedir.", "BKM", "Anlık", "HHS_YOS_GUNCELLENDI", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(1130), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 5, "5 Dakika - 3 Deneme", "yosKod", "YÖS", "HHS" },
                    { new Guid("17322cb5-20e5-4046-bddb-78e84d30b6e4"), "GET /hesaplar/{hspRef}/bakiye", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(940), new Guid("00000000-0000-0000-0000-000000000000"), null, "Bakiye nesnesindeki tutarla ilgili bir bilgi değiştiğinde ve HBH rızası içerisinde \"06-Anlık Bakiye Bildirimi\" izin türü varsa oluşturulur.\n\nMevcutta alınmış rızalar için bakiye kaynak tipi özelinde 06 izin türü gerektiğinden; mevcut rızanın yenilenmesine dair müşteriye bilgilendirme yapılarak 06 izin türünü kapsayan yeni rıza alınması süreci YÖS tarafından gerçekleştirilebilir.\n\nBloke tutar değişikliği için olay oluşturma ve bildirimi HHS inisiyatifindedir.\n\nKrdHsp içerisinde yer alan kulKrdTtr değerinin değiştiği durumda olay bildirim gönderilmesi gerekmektedir.", "HHS", "Maksimum 10 dakika içerisinde", "KAYNAK_GUNCELLENDI", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(940), new Guid("00000000-0000-0000-0000-000000000000"), null, null, null, "Retry policy uygulanmamalıdır. İlk istek gönderilemediği durumda İletilemeyen Olaylara eklenmelidir.", "hspRef", "BAKIYE", "HBH" },
                    { new Guid("55cf6777-4542-4e26-bfb5-8df4600e4e5c"), "GET /hesap-bilgisi-rizasi/{RizaNo}", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(910), new Guid("00000000-0000-0000-0000-000000000000"), null, "Rıza iptal detay kodu ‘02’ : Kullanıcı İsteği ile HHS üzerinden İptal durumunda", "HHS", "Anlık", "KAYNAK_GUNCELLENDI", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(910), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 30, "30 Dakika - 3 Deneme", "RizaNo", "HESAP_BILGISI_RIZASI", "HBH" },
                    { new Guid("80a30b1d-406c-4fba-bdb4-29d66bdc664f"), "GET /hhs/{hhsKod}", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(1110), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS bilgilerinde değişiklik olduğunda, YÖS'ün hhsKod ile sorgulama yapması ve değişen bilgiyi güncellemesi beklenmektedir", "BKM", "Anlık", "HHS_YOS_GUNCELLENDI", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(1110), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 5, "5 Dakika - 3 Deneme", "hhsKod", "HHS", "YÖS" },
                    { new Guid("80a51750-7100-4d54-85c8-442774f8494c"), "GET /odeme-emri-rizasi/{RizaNo}", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(1040), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladıktan sonra yaptığı kontroller neticesinde logine izin vermez ise YÖS'e bildirim yapılır. YÖS rıza durumunu sorgulayarak işlemin neden iletilmediğine dair bilgi edinebilir.", "HHS", "Anlık", "AYRIK_GKD_BASARISIZ", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(1040), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "ODEME_EMRI_RIZASI", "ÖBH" },
                    { new Guid("85a73a7d-7519-4c1b-bb9a-099bbbdb3686"), "GET /yetkilendirme-kodu?rizaNo={rizaNo}}&rizaTip=O", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(990), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladığında rıza oluşturulur. YÖS'e rıza oluşturulduğuna dair bildirim yapılır. YÖS yetkod değerini sorgulama sonucunda elde eder.", "HHS", "Anlık", "AYRIK_GKD_BASARILI", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(990), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "ODEME_EMRI_RIZASI", "ÖBH" },
                    { new Guid("8bf4a8dd-1531-4ca8-bed9-cfc4ae035bee"), "GET /odeme-emri/{odemeEmriNo}", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(870), new Guid("00000000-0000-0000-0000-000000000000"), null, "Tüm ödeme durum değişikliklerinde", "HHS", "Anlık", "KAYNAK_GUNCELLENDI", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(870), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 30, "30 Dakika - 3 Deneme", "odemeEmriNo", "ODEME_EMRI", "ÖBH" },
                    { new Guid("cc538bf8-fb05-4515-9a69-d017e9e3228e"), "GET /yetkilendirme-kodu?rizaNo={rizaNo}}&rizaTip=H", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(1010), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladığında rıza oluşturulur. YÖS'e rıza oluşturulduğuna dair bildirim yapılır. YÖS yetkod değerini sorgulama sonucunda elde eder.", "HHS", "Anlık", "AYRIK_GKD_BASARILI", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(1010), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "HESAP_BILGISI_RIZASI", "HBH" },
                    { new Guid("e7778043-2ac8-4136-b93f-abe79d720203"), "GET /hesap-bilgisi-rizasi/{RizaNo}", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(1060), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladıktan sonra yaptığı kontroller neticesinde logine izin vermez ise YÖS'e bildirim yapılır. YÖS rıza durumunu sorgulayarak işlemin neden iletilmediğine dair bilgi edinebilir.", "HHS", "Anlık", "AYRIK_GKD_BASARISIZ", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(1060), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "HESAP_BILGISI_RIZASI", "HBH" },
                    { new Guid("f766cd71-265a-4de0-8d0e-40d02effc336"), "", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(970), new Guid("00000000-0000-0000-0000-000000000000"), null, "İlgili API İlke ve kurallarına eklendiğinde güncellenecektir.", "HHS", "", "KAYNAK_GUNCELLENDI", new DateTime(2024, 1, 11, 8, 41, 0, 460, DateTimeKind.Utc).AddTicks(970), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 30, "30 Dakika - 3 Deneme", "", "COKLU_ISLEM_TALEBI ( bulk-data)", "HBH" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("1421ba33-e7ae-4c59-9182-fc68d4e82bb2"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("17322cb5-20e5-4046-bddb-78e84d30b6e4"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("55cf6777-4542-4e26-bfb5-8df4600e4e5c"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("80a30b1d-406c-4fba-bdb4-29d66bdc664f"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("80a51750-7100-4d54-85c8-442774f8494c"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("85a73a7d-7519-4c1b-bb9a-099bbbdb3686"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("8bf4a8dd-1531-4ca8-bed9-cfc4ae035bee"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("cc538bf8-fb05-4515-9a69-d017e9e3228e"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("e7778043-2ac8-4136-b93f-abe79d720203"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("f766cd71-265a-4de0-8d0e-40d02effc336"));
        }
    }
}
