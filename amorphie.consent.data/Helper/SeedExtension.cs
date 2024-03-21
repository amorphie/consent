using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using Microsoft.EntityFrameworkCore;

public static class SeedExtension
{
    public static void SeedOBEventTypeSourceTypeRelation(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OBEventTypeSourceTypeRelation>().HasData(new OBEventTypeSourceTypeRelation
        {
            Id = Guid.NewGuid(),
            EventType = "KAYNAK_GUNCELLENDI",
            YOSRole = "ÖBH",
            SourceType = "ODEME_EMRI",
            EventCase = "Tüm ödeme durum değişikliklerinde",
            SourceNumber = "odemeEmriNo",
            APIToGetData = "GET /odeme-emri/{odemeEmriNo}",
            EventNotificationReporter = "HHS",
            EventNotificationTime = "Anlık",
            RetryPolicy = "30 Dakika - 3 Deneme",
            RetryInMinute = 30,
            RetryCount = 3,
            CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            CreatedBy = Guid.Empty,
            ModifiedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            ModifiedBy = Guid.Empty
        });
        modelBuilder.Entity<OBEventTypeSourceTypeRelation>().HasData(new OBEventTypeSourceTypeRelation
        {
            Id = Guid.NewGuid(),
            EventType = "KAYNAK_GUNCELLENDI",
            YOSRole = "HBH",
            SourceType = "HESAP_BILGISI_RIZASI",
            EventCase = "Rıza iptal detay kodu ‘02’ : Kullanıcı İsteği ile HHS üzerinden İptal durumunda",
            SourceNumber = "RizaNo",
            APIToGetData = "GET /hesap-bilgisi-rizasi/{RizaNo}",
            EventNotificationReporter = "HHS",
            EventNotificationTime = "Anlık",
            RetryPolicy = "30 Dakika - 3 Deneme",
            RetryInMinute = 30,
            RetryCount = 3,
            CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            CreatedBy = Guid.Empty,
            ModifiedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            ModifiedBy = Guid.Empty
        });
        modelBuilder.Entity<OBEventTypeSourceTypeRelation>().HasData(new OBEventTypeSourceTypeRelation
        {
            Id = Guid.NewGuid(),
            EventType = "KAYNAK_GUNCELLENDI",
            YOSRole = "HBH",
            SourceType = "BAKIYE",
            EventCase = "Bakiye nesnesindeki tutarla ilgili bir bilgi değiştiğinde ve HBH rızası içerisinde \"06-Anlık Bakiye Bildirimi\" izin türü varsa oluşturulur.\n\nMevcutta alınmış rızalar için bakiye kaynak tipi özelinde 06 izin türü gerektiğinden; mevcut rızanın yenilenmesine dair müşteriye bilgilendirme yapılarak 06 izin türünü kapsayan yeni rıza alınması süreci YÖS tarafından gerçekleştirilebilir.\n\nBloke tutar değişikliği için olay oluşturma ve bildirimi HHS inisiyatifindedir.\n\nKrdHsp içerisinde yer alan kulKrdTtr değerinin değiştiği durumda olay bildirim gönderilmesi gerekmektedir.",
            SourceNumber = "hspRef",
            APIToGetData = "GET /hesaplar/{hspRef}/bakiye",
            EventNotificationReporter = "HHS",
            EventNotificationTime = "Maksimum 10 dakika içerisinde",
            RetryPolicy = "Retry policy uygulanmamalıdır. İlk istek gönderilemediği durumda İletilemeyen Olaylara eklenmelidir.",
            RetryInMinute = null,
            RetryCount = null,
            CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            CreatedBy = Guid.Empty,
            ModifiedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            ModifiedBy = Guid.Empty
        });
        modelBuilder.Entity<OBEventTypeSourceTypeRelation>().HasData(new OBEventTypeSourceTypeRelation
        {
            Id = Guid.NewGuid(),
            EventType = "KAYNAK_GUNCELLENDI",
            YOSRole = "HBH",
            SourceType = "COKLU_ISLEM_TALEBI ( bulk-data)",
            EventCase = "İlgili API İlke ve kurallarına eklendiğinde güncellenecektir.",
            SourceNumber = string.Empty,
            APIToGetData = string.Empty,
            EventNotificationReporter = "HHS",
            EventNotificationTime = string.Empty,
            RetryPolicy = "30 Dakika - 3 Deneme",
            RetryInMinute = 30,
            RetryCount = 3,
            CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            CreatedBy = Guid.Empty,
            ModifiedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            ModifiedBy = Guid.Empty
        });
        modelBuilder.Entity<OBEventTypeSourceTypeRelation>().HasData(new OBEventTypeSourceTypeRelation
        {
            Id = Guid.NewGuid(),
            EventType = "AYRIK_GKD_BASARILI",
            YOSRole = "ÖBH",
            SourceType = "ODEME_EMRI_RIZASI",
            EventCase = "HHS sisteminde ÖHK kendini doğruladığında rıza oluşturulur. YÖS'e rıza oluşturulduğuna dair bildirim yapılır. YÖS yetkod değerini sorgulama sonucunda elde eder.",
            SourceNumber = "RizaNo",
            APIToGetData = "GET /yetkilendirme-kodu?rizaNo={rizaNo}}&rizaTip=O",
            EventNotificationReporter = "HHS",
            EventNotificationTime = "Anlık",
            RetryPolicy = "1 Dakika - 3 Deneme",
            RetryInMinute = 1,
            RetryCount = 3,
            CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            CreatedBy = Guid.Empty,
            ModifiedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            ModifiedBy = Guid.Empty
        });
        modelBuilder.Entity<OBEventTypeSourceTypeRelation>().HasData(new OBEventTypeSourceTypeRelation
        {
            Id = Guid.NewGuid(),
            EventType = "AYRIK_GKD_BASARILI",
            YOSRole = "HBH",
            SourceType = "HESAP_BILGISI_RIZASI",
            EventCase = "HHS sisteminde ÖHK kendini doğruladığında rıza oluşturulur. YÖS'e rıza oluşturulduğuna dair bildirim yapılır. YÖS yetkod değerini sorgulama sonucunda elde eder.",
            SourceNumber = "RizaNo",
            APIToGetData = "GET /yetkilendirme-kodu?rizaNo={rizaNo}}&rizaTip=H",
            EventNotificationReporter = "HHS",
            EventNotificationTime = "Anlık",
            RetryPolicy = "1 Dakika - 3 Deneme",
            RetryInMinute = 1,
            RetryCount = 3,
            CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            CreatedBy = Guid.Empty,
            ModifiedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            ModifiedBy = Guid.Empty
        });
        modelBuilder.Entity<OBEventTypeSourceTypeRelation>().HasData(new OBEventTypeSourceTypeRelation
        {
            Id = Guid.NewGuid(),
            EventType = "AYRIK_GKD_BASARISIZ",
            YOSRole = "ÖBH",
            SourceType = "ODEME_EMRI_RIZASI",
            EventCase = "HHS sisteminde ÖHK kendini doğruladıktan sonra yaptığı kontroller neticesinde logine izin vermez ise YÖS'e bildirim yapılır. YÖS rıza durumunu sorgulayarak işlemin neden iletilmediğine dair bilgi edinebilir.",
            SourceNumber = "RizaNo",
            APIToGetData = "GET /odeme-emri-rizasi/{RizaNo}",
            EventNotificationReporter = "HHS",
            EventNotificationTime = "Anlık",
            RetryPolicy = "1 Dakika - 3 Deneme",
            RetryInMinute = 1,
            RetryCount = 3,
            CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            CreatedBy = Guid.Empty,
            ModifiedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            ModifiedBy = Guid.Empty
        });
        modelBuilder.Entity<OBEventTypeSourceTypeRelation>().HasData(new OBEventTypeSourceTypeRelation
        {
            Id = Guid.NewGuid(),
            EventType = "AYRIK_GKD_BASARISIZ",
            YOSRole = "HBH",
            SourceType = "HESAP_BILGISI_RIZASI",
            EventCase = "HHS sisteminde ÖHK kendini doğruladıktan sonra yaptığı kontroller neticesinde logine izin vermez ise YÖS'e bildirim yapılır. YÖS rıza durumunu sorgulayarak işlemin neden iletilmediğine dair bilgi edinebilir.",
            SourceNumber = "RizaNo",
            APIToGetData = "GET /hesap-bilgisi-rizasi/{RizaNo}",
            EventNotificationReporter = "HHS",
            EventNotificationTime = "Anlık",
            RetryPolicy = "1 Dakika - 3 Deneme",
            RetryInMinute = 1,
            RetryCount = 3,
            CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            CreatedBy = Guid.Empty,
            ModifiedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            ModifiedBy = Guid.Empty
        });
        modelBuilder.Entity<OBEventTypeSourceTypeRelation>().HasData(new OBEventTypeSourceTypeRelation
        {
            Id = Guid.NewGuid(),
            EventType = "HHS_YOS_GUNCELLENDI",
            YOSRole = "YÖS",
            SourceType = "HHS",
            EventCase = "HHS bilgilerinde değişiklik olduğunda, YÖS'ün hhsKod ile sorgulama yapması ve değişen bilgiyi güncellemesi beklenmektedir",
            SourceNumber = "hhsKod",
            APIToGetData = "GET /hhs/{hhsKod}",
            EventNotificationReporter = "BKM",
            EventNotificationTime = "Anlık",
            RetryPolicy = "5 Dakika - 3 Deneme",
            RetryInMinute = 5,
            RetryCount = 3,
            CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            CreatedBy = Guid.Empty,
            ModifiedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            ModifiedBy = Guid.Empty
        });
        modelBuilder.Entity<OBEventTypeSourceTypeRelation>().HasData(new OBEventTypeSourceTypeRelation
        {
            Id = Guid.NewGuid(),
            EventType = "HHS_YOS_GUNCELLENDI",
            YOSRole = "HHS",
            SourceType = "YÖS",
            EventCase = "YÖS bilgilerinde değişiklik olduğunda, HHS'nin yosKod ile sorgulama yapması ve değişen bilgiyi güncellemesi beklenmektedir.",
            SourceNumber = "yosKod",
            APIToGetData = "GET /yos/{yosKod}",
            EventNotificationReporter = "BKM",
            EventNotificationTime = "Anlık",
            RetryPolicy = "5 Dakika - 3 Deneme",
            RetryInMinute = 5,
            RetryCount = 3,
            CreatedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            CreatedBy = Guid.Empty,
            ModifiedAt = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),
            ModifiedBy = Guid.Empty
        });

    }

    public static void SeedOBPermissionTypes(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OBPermissionType>().HasData(new OBPermissionType
        {
            Id = Guid.NewGuid(),
            Code = "01",
            Description = "Temel Hesap Bilgisi",
            GroupId = 1,
            GroupName = "Hesap Bilgileri",
            Language = "tr-TR"
        });
        modelBuilder.Entity<OBPermissionType>().HasData(new OBPermissionType
        {
            Id = Guid.NewGuid(),
            Code = "02",
            Description = "Ayrıntılı Hesap Bilgisi",
            GroupId = 1,
            GroupName = "Hesap Bilgileri",
            Language = "tr-TR"
        });
        modelBuilder.Entity<OBPermissionType>().HasData(new OBPermissionType
        {
            Id = Guid.NewGuid(),
            Code = "03",
            Description = "Bakiye Bilgisi",
            GroupId = 2,
            GroupName = "Hesap Bakiyesi",
            Language = "tr-TR"
        });
        modelBuilder.Entity<OBPermissionType>().HasData(new OBPermissionType
        {
            Id = Guid.NewGuid(),
            Code = "04",
            Description = "Temel İşlem (Hesap Hareketleri) Bilgisi",
            GroupId = 3,
            GroupName = "Hesap Hareketleri",
            Language = "tr-TR"
        });
        modelBuilder.Entity<OBPermissionType>().HasData(new OBPermissionType
        {
            Id = Guid.NewGuid(),
            Code = "05",
            Description = "Ayrıntılı İşlem Bilgisi",
            GroupId = 3,
            GroupName = "Hesap Hareketleri",
            Language = "tr-TR"
        });
        modelBuilder.Entity<OBPermissionType>().HasData(new OBPermissionType
        {
            Id = Guid.NewGuid(),
            Code = "06",
            Description = "Anlık Bakiye Bildirimi",
            GroupId = 2,
            GroupName = "Hesap Bakiyesi",
            Language = "tr-TR"
        });

    }


    public static void SeedOBErrorCodeDetail(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Missing",
            Message = "must not be null",
            MessageTr = "boş değer olamaz"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "Validation error",
            MessageTr = "Şema kontrolleri başarısız"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidAspsp.GetHashCode(),
            BkmCode = "TR.OHVPS.Connection.InvalidASPSP",
            Message = "Invalid ASPSP Code",
            MessageTr = "Geçersiz HHS kodu."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidTpp.GetHashCode(),
            BkmCode = "TR.OHVPS.Connection.InvalidTPP",
            Message = "Invalid TPP Code",
            MessageTr = "Geçersiz Yos kodu."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.NotFound.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.NotFound",
            Message = "Resource not found",
            MessageTr = "Kaynak bulunamadı."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidContentPsuInitiated.GetHashCode(),
            BkmCode = "TR.OHVPS.Business.InvalidContent",
            Message = "PsuInitiated invalid",
            MessageTr = "PsuInitiated değeri hatalı."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidContentUserReference.GetHashCode(),
            BkmCode = "TR.OHVPS.Business.InvalidContent",
            Message = "User reference in header is wrong.",
            MessageTr = "User reference değeri hatalı."
        });

    }

    
}