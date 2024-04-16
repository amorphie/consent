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
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "Invalid data.",
            MessageTr = "Geçersiz veri."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldHhsCodeYosCodeLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 4",
            MessageTr = "boyut '4' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be between 1-30",
            MessageTr = "boyut '1' ile  '30' arasında olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimTipLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be between 1-8",
            MessageTr = "boyut '1' ile '8' arasında olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerTcknLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 11",
            MessageTr = "boyut '11' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerMnoLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be between 1-30",
            MessageTr = "boyut '1' ile  '30' arasında olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerYknLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 11",
            MessageTr = "boyut '11' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerPnoLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be between 1-9",
            MessageTr = "boyut '1' ile  '9' arasında olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerGsmLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 10",
            MessageTr = "boyut '10' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerIbanLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 26",
            MessageTr = "boyut '26' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkTcknLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 11",
            MessageTr = "boyut '11' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkMnoLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be between 1-30",
            MessageTr = "boyut '1' ile  '30' arasında olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkYknLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 11",
            MessageTr = "boyut '11' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkPnoLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be between 1-9",
            MessageTr = "boyut '1' ile  '9' arasında olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkVknLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 10",
            MessageTr = "boyut '10' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIznTurNoTemelHesap.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "Temel hesap bilgisi izni must.",
            MessageTr = "Temel hesap bilgisi izni seçimi zorunludur."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIznTurAyrintiliIslemWithoutTemelIslem.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "Detail transactions permission can not be selected without Basic transactions.",
            MessageTr = "Temelişlem bilgisi izni seçimi yapılmadan ayrıntılı işlem bilgisi seçimi yapılamaz.."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIznTurAnlikBakiyeWithoutBakiye.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "Instant balance notification cannot be selected without selecting balance information permission.",
            MessageTr = "Bakiye bilgisi izni seçimi yapılmadan anlık bakiye bildirimi seçimi yapılamaz.."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldErisimIzniSonTrh.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "The minimum value it can take is consent date +1 day, the maximum value it can take is: Consent date + 6 months.",
            MessageTr = "Alabileceği minimum değer tarihi +1 gün, alabileceği maksimum değer : Rıza tarihi + 6 ay "
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldTransactionSelectedDateNotSet.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "When the Basic Transaction Information/Detailed Transaction Information permission is selected, the hesapIslemBslZmn hesapIslemBtsZmn fields must be filled.",
            MessageTr = "Temel İşlem bilgisi/ayrıntlı işlem bilgisi izni seçilmiş olduğu zaman hesapIslemBslZmn hesapIslemBtsZmn alanlarının doldurulması zorunludur."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldDateSetTransactionNotSelected.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "hesapIslemBslZmn hesapIslemBtsZmn fields should only be sent when the Basic Transaction Information/Detailed Transaction Information permission is selected.",
            MessageTr = "hesapIslemBslZmn hesapIslemBtsZmn alanları sadece Temel İşlem bilgisi/ayrıntlı işlem bilgisi izni seçilmiş olduğu zaman gönderilmelidir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldHesapIslemDateRange.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "Minimum date: Date of consent given – 12 months Maximum date: Date of consent given + 12 months",
            MessageTr = "Minimum tarih : Rızanın veriliş tarihi – 12 ay Maksimum tarih : Rızanın veriliş tarihi + 12 ay"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldHesapIslemDateRange.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "Minimum date: Date of consent given – 12 months Maximum date: Date of consent given + 12 months",
            MessageTr = "Minimum tarih : Rızanın veriliş tarihi – 12 ay Maksimum tarih : Rızanın veriliş tarihi + 12 ay"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldesapIslemBslZmnLaterThanBtsZmn.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "hesapIslemBslZmn can not be later than hesapIslemBtsZmn.",
            MessageTr = "hesapIslemBslZmn, hesapIslemBtsZmn verisinden sonra olamaz. "
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
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.GkdTanimDegerKimlikNotMatch.GetHashCode(),
            BkmCode = "TR.OHVPS.Business.CustomerInfoMismatch",
            Message = "kmlk.kmlkVrs - ayrikGkd.ohkTanimDeger must match.",
            MessageTr = "kmlk.kmlkVrs - ayrikGkd.ohkTanimDeger aynı olmalı."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.AyrikGkdEventSubscriptionNotFound.GetHashCode(),
            BkmCode = "TR.OHVPS.Business.EventSubscriptionNotFound",
            Message = "No evet subscription for AYRIK_GKD_BASARILI and AYRIK_GKD_BASARISIZ.",
            MessageTr = "AYRIK_GKD_BASARILI ve AYRIK_GKD_BASARISIZ olay tipleri için olay aboneliği yapılmalıdır."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSyfKytSayi.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message =  "syfKytSayi value is not valid. syfKytSayi can be between 1-100",
            MessageTr = "syfKytSayi değeri geçersiz. 1-100 aralığında olabilir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSrlmKrtrAccount.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message =  "srlmKrtr value is not valid. it should be hspRef",
            MessageTr = "srlmKrtr değeri geçersiz. Olması gereken değer hspRef."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSrlmYon.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message =  "srlmYon value is not valid.",
            MessageTr = "srlmYon değeri geçersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormathesapIslemBslBtsTrh.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message =  "hesapIslemBtsTrh,hesapIslemBslTrh values not valid",
            MessageTr = "hesapIslemBtsTrh,hesapIslemBslTrh değerleri geçersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormathesapIslemBtsTrhLaterThanToday.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message =  "hesapIslemBtsTrh can not be later than enquiry datetime.",
            MessageTr = "hesapIslemBtsTrh sorgulama zamanından sonra olamaz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatesapIslemBslZmnLaterThanBtsZmn.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message =  "hesapIslemBtsTrh can not be early than hesapIslemBslTrh.",
            MessageTr = "hesapIslemBtsTrh hesapIslemBslTrh den önce olamaz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatBireyselDateDiff.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message =  "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 month.",
            MessageTr = "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark bireysel ÖHK’lar için en fazla 1 ay olabilir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatKurumsalDateDiff.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message =  "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 week.",
            MessageTr = "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark kurumsal ÖHK’lar için en fazla 1 hafta olabilir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSystemStartedDateDiff.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message =  "For system enquiry, last 24 hours can be enquirable.",
            MessageTr = "sistemsel yapılan sorgulamalarda hem bireysel, hem de kurumsal ÖHK’lar için;son 24 saat sorgulanabilir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatBrcAlc.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message =  "brcAlc value is not valid.",
            MessageTr = "brcAlc değeri geçersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSrlmKrtrTransaction.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message =  "srlmKrtr value is not valid. it should be islGrckZaman",
            MessageTr = "srlmKrtr değeri geçersiz. Olması gereken değer islGrckZaman."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatMinIslTtr.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message =  "minIslTtr value is not valid.",
            MessageTr = "minIslTtr değeri geçersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatMksIslTtr.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message =  "mksIslTtr value is not valid.",
            MessageTr = "mksIslTtr değeri geçersiz."
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
            MessageTr = "User reference değeri geçersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidContentConsentIdInHeader.GetHashCode(),
            BkmCode = "TR.OHVPS.Business.InvalidContent",
            Message = "OpenBanking Consent Id in header is invalid.",
            MessageTr = "OpenBanking Consent Id değeri geçersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.ConsentMismatch.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.ConsentMismatch",
            Message = "Consent not valid to process",
            MessageTr = "Consent işlem yapılmaya uygun değil."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.ConsentMismatchStateNotValidToDelete.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.ConsentMismatch",
            Message = "Consent state not valid to delete",
            MessageTr = "Consent rıza durumu silme işlemine uygun değil."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.ConsentMismatchAccountPostAlreadyAuthroized.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.ConsentMismatch",
            Message = "There is a Authorized / Authorzation Used consent in the system. First cancel the consent.",
            MessageTr = "Sistemde Yetkilendirildi / Yetki Kullanıldı durumunda rıza olduğu için rıza kabul edilmedi. Öncelikli olarak rızayı iptal ediniz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.ConsentRevokedStateEnd.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.ConsentRevoked",
            Message = "Consent ended. Not valid to process.",
            MessageTr = "Sonlandırılmış rıza için işlem yapılamaz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.ConsentRevokedStateNotAutUsed.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.ConsentRevoked",
            Message = "Consent state is not authorization used. Not valid to process.",
            MessageTr = "Rıza durumu yetki kullanıldı olmadığı için işlem yapılamaz."
        });
        
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InternalServerError.GetHashCode(),
            BkmCode = "TR.OHVPS.Server.InternalError",
            Message = "Unexpected condition was encountered.",
            MessageTr = "Beklenmeyen bir durumla karşılaşıldı."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InternalServerErrorCheckingIdempotency.GetHashCode(),
            BkmCode = "TR.OHVPS.Server.InternalError",
            Message = "By Checking Idempotency Unexpected condition was encountered.",
            MessageTr = "Idempotency kontrol edilirken beklenmeyen bir durumla karşılaşıldı."
        });

    }


}