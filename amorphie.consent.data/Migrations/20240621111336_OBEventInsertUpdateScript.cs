using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace amorphie.consent.data.Migrations
{
    /// <inheritdoc />
    public partial class OBEventInsertUpdateScript : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {

              migrationBuilder.Sql(@"
        DO
        $do$
        BEGIN
            IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'OBEventTypeSourceTypeRelations') THEN
                TRUNCATE TABLE ""OBEventTypeSourceTypeRelations"";
            END IF;
        END
        $do$;");
           
            migrationBuilder.InsertData(
                table: "OBErrorCodeDetails",
                columns: new[] { "Id", "BkmCode", "InternalCode", "Message", "MessageTr" },
                values: new object[,]
                {
                    { new Guid("1bf82773-c74a-43a8-98ad-253b3183b485"), "TR.OHVPS.Business.InvalidContent", 211, "There can be only one item in olaylar object.", "Olaylar nesnesi içerisinde sadece 1 tane olay olabilir." },
                    { new Guid("ef21aad7-bae6-4e05-b228-e78362e60c07"), "TR.OHVPS.Business.InvalidContent", 212, "Event type source type relation is incorrect in BKM system event post process.", "BKM sistem olay dinleme mesajında yer alan olay tipi - kaynak tipi verileri hatalı." }
                });

            migrationBuilder.InsertData(
                table: "OBEventTypeSourceTypeRelations",
                columns: new[] { "Id", "APIToGetData", "CreatedAt", "CreatedBy", "CreatedByBehalfOf", "EventCase", "EventNotificationReporter", "EventNotificationTime", "EventType", "IsImmediateNotification", "ModifiedAt", "ModifiedBy", "ModifiedByBehalfOf", "RetryCount", "RetryInMinute", "RetryPolicy", "SourceNumber", "SourceType", "YOSRole" },
                values: new object[,]
                {
                    { new Guid("0a9d7a66-339f-4ee4-8e08-a2021d44108c"), "GET /yetkilendirme-kodu?rizaNo={rizaNo}}&rizaTip=O", new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4870), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladığında rıza oluşturulur. YÖS'e rıza oluşturulduğuna dair bildirim yapılır. YÖS yetkod değerini sorgulama sonucunda elde eder.", "HHS", "Anlık", "AYRIK_GKD_BASARILI", false, new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4870), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "ODEME_EMRI_RIZASI", "ÖBH" },
                    { new Guid("326b700f-1a5d-4ac5-9db3-4d447a59a6c6"), "GET /yetkilendirme-kodu?rizaNo={rizaNo}}&rizaTip=H", new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4890), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladığında rıza oluşturulur. YÖS'e rıza oluşturulduğuna dair bildirim yapılır. YÖS yetkod değerini sorgulama sonucunda elde eder.", "HHS", "Anlık", "AYRIK_GKD_BASARILI", false, new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4890), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "HESAP_BILGISI_RIZASI", "HBH" },
                    { new Guid("60de6441-dc10-4df5-8f1e-1641b11da815"), "GET /hesap-bilgisi-rizasi/{RizaNo}", new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4750), new Guid("00000000-0000-0000-0000-000000000000"), null, "Rıza iptal detay kodu ‘02’ : Kullanıcı İsteği ile HHS üzerinden İptal durumunda", "HHS", "Anlık", "KAYNAK_GUNCELLENDI", false, new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4750), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 30, "30 Dakika - 3 Deneme", "RizaNo", "HESAP_BILGISI_RIZASI", "HBH" },
                    { new Guid("740b07fd-b0ad-46f8-9f4e-70a881941c1b"), "GET /odeme-emri-rizasi/{RizaNo}", new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4920), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladıktan sonra yaptığı kontroller neticesinde logine izin vermez ise YÖS'e bildirim yapılır. YÖS rıza durumunu sorgulayarak işlemin neden iletilmediğine dair bilgi edinebilir.", "HHS", "Anlık", "AYRIK_GKD_BASARISIZ", false, new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4920), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "ODEME_EMRI_RIZASI", "ÖBH" },
                    { new Guid("74fb00c2-32fd-47f3-9973-da1d1dfb7a97"), "GET /hesap-bilgisi-rizasi/{RizaNo}", new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4940), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS sisteminde ÖHK kendini doğruladıktan sonra yaptığı kontroller neticesinde logine izin vermez ise YÖS'e bildirim yapılır. YÖS rıza durumunu sorgulayarak işlemin neden iletilmediğine dair bilgi edinebilir.", "HHS", "Anlık", "AYRIK_GKD_BASARISIZ", false, new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4940), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 1, "1 Dakika - 3 Deneme", "RizaNo", "HESAP_BILGISI_RIZASI", "HBH" },
                    { new Guid("92e2ec7d-5aae-426f-9a11-e511239a6a89"), "GET /yos/{yosKod}", new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4990), new Guid("00000000-0000-0000-0000-000000000000"), null, "YÖS bilgilerinde değişiklik olduğunda, HHS'nin yosKod ile sorgulama yapması ve değişen bilgiyi güncellemesi beklenmektedir.", "BKM", "Anlık", "HHS_YOS_GUNCELLENDI", false, new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4990), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 5, "5 Dakika - 3 Deneme", "yosKod", "YOS", "HHS" },
                    { new Guid("b1a20d09-d60f-47c0-8140-a9037654eddf"), "GET /odeme-emri/{odemeEmriNo}", new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4720), new Guid("00000000-0000-0000-0000-000000000000"), null, "Tüm ödeme durum değişikliklerinde", "HHS", "Anlık", "KAYNAK_GUNCELLENDI", false, new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4730), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 30, "30 Dakika - 3 Deneme", "odemeEmriNo", "ODEME_EMRI", "ÖBH" },
                    { new Guid("d338cd31-ce5d-411d-ad8e-4df4924c1f31"), "GET /hesaplar/{hspRef}/bakiye", new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4770), new Guid("00000000-0000-0000-0000-000000000000"), null, "Bakiye nesnesindeki tutarla ilgili bir bilgi değiştiğinde ve HBH rızası içerisinde \"06-Anlık Bakiye Bildirimi\" izin türü varsa oluşturulur.\n\nMevcutta alınmış rızalar için bakiye kaynak tipi özelinde 06 izin türü gerektiğinden; mevcut rızanın yenilenmesine dair müşteriye bilgilendirme yapılarak 06 izin türünü kapsayan yeni rıza alınması süreci YÖS tarafından gerçekleştirilebilir.\n\nBloke tutar değişikliği için olay oluşturma ve bildirimi HHS inisiyatifindedir.\n\nKrdHsp içerisinde yer alan kulKrdTtr değerinin değiştiği durumda olay bildirim gönderilmesi gerekmektedir.", "HHS", "Maksimum 10 dakika içerisinde", "KAYNAK_GUNCELLENDI", false, new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4770), new Guid("00000000-0000-0000-0000-000000000000"), null, null, null, "Retry policy uygulanmamalıdır. İlk istek gönderilemediği durumda İletilemeyen Olaylara eklenmelidir.", "hspRef", "BAKIYE", "HBH" },
                    { new Guid("db3e1652-bb2b-4dcf-801d-cc6d0ce4cca9"), "GET /hhs/{hhsKod}", new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4960), new Guid("00000000-0000-0000-0000-000000000000"), null, "HHS bilgilerinde değişiklik olduğunda, YÖS'ün hhsKod ile sorgulama yapması ve değişen bilgiyi güncellemesi beklenmektedir", "BKM", "Anlık", "HHS_YOS_GUNCELLENDI", false, new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4960), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 5, "5 Dakika - 3 Deneme", "hhsKod", "HHS", "YÖS" },
                    { new Guid("f4386d9e-9df0-4fbd-9b9e-a532e1d98809"), "", new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4850), new Guid("00000000-0000-0000-0000-000000000000"), null, "İlgili API İlke ve kurallarına eklendiğinde güncellenecektir.", "HHS", "", "KAYNAK_GUNCELLENDI", false, new DateTime(2024, 6, 21, 11, 13, 36, 220, DateTimeKind.Utc).AddTicks(4850), new Guid("00000000-0000-0000-0000-000000000000"), null, 3, 30, "30 Dakika - 3 Deneme", "", "COKLU_ISLEM_TALEBI", "HBH" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("1bf82773-c74a-43a8-98ad-253b3183b485"));

            migrationBuilder.DeleteData(
                table: "OBErrorCodeDetails",
                keyColumn: "Id",
                keyValue: new Guid("ef21aad7-bae6-4e05-b228-e78362e60c07"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("0a9d7a66-339f-4ee4-8e08-a2021d44108c"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("326b700f-1a5d-4ac5-9db3-4d447a59a6c6"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("60de6441-dc10-4df5-8f1e-1641b11da815"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("740b07fd-b0ad-46f8-9f4e-70a881941c1b"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("74fb00c2-32fd-47f3-9973-da1d1dfb7a97"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("92e2ec7d-5aae-426f-9a11-e511239a6a89"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("b1a20d09-d60f-47c0-8140-a9037654eddf"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("d338cd31-ce5d-411d-ad8e-4df4924c1f31"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("db3e1652-bb2b-4dcf-801d-cc6d0ce4cca9"));

            migrationBuilder.DeleteData(
                table: "OBEventTypeSourceTypeRelations",
                keyColumn: "Id",
                keyValue: new Guid("f4386d9e-9df0-4fbd-9b9e-a532e1d98809"));

         
        }
    }
}
