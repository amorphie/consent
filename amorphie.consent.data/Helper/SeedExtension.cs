using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using Microsoft.EntityFrameworkCore;
namespace amorphie.consent.data;
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
            Message = "syfKytSayi value is not valid. syfKytSayi can be between 1-100",
            MessageTr = "syfKytSayi değeri geçersiz. 1-100 aralığında olabilir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSrlmKrtrAccount.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "srlmKrtr value is not valid. it should be hspRef",
            MessageTr = "srlmKrtr değeri geçersiz. Olması gereken değer hspRef."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSrlmYon.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "srlmYon value is not valid.",
            MessageTr = "srlmYon değeri geçersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormathesapIslemBslBtsTrh.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "hesapIslemBtsTrh,hesapIslemBslTrh values not valid",
            MessageTr = "hesapIslemBtsTrh,hesapIslemBslTrh değerleri geçersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormathesapIslemBtsTrhLaterThanToday.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "hesapIslemBtsTrh can not be later than enquiry datetime.",
            MessageTr = "hesapIslemBtsTrh sorgulama zamanından sonra olamaz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatesapIslemBslZmnLaterThanBtsZmn.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "hesapIslemBtsTrh can not be early than hesapIslemBslTrh.",
            MessageTr = "hesapIslemBtsTrh hesapIslemBslTrh den önce olamaz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatBireyselDateDiff.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 month.",
            MessageTr = "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark bireysel ÖHK’lar için en fazla 1 ay olabilir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatKurumsalDateDiff.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "hesapIslemBtsTrh hesapIslemBslTrh difference can be maximum 1 week.",
            MessageTr = "hesapIslemBslTrh ve hesapIslemBtsTrh arası fark kurumsal ÖHK’lar için en fazla 1 hafta olabilir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSystemStartedDateDiff.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "For system enquiry, last 24 hours can be enquirable.",
            MessageTr = "sistemsel yapılan sorgulamalarda hem bireysel, hem de kurumsal ÖHK’lar için;son 24 saat sorgulanabilir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatBrcAlc.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "brcAlc value is not valid.",
            MessageTr = "brcAlc değeri geçersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSrlmKrtrTransaction.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "srlmKrtr value is not valid. it should be islGrckZaman",
            MessageTr = "srlmKrtr değeri geçersiz. Olması gereken değer islGrckZaman."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatMinIslTtr.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "minIslTtr value is not valid.",
            MessageTr = "minIslTtr değeri geçersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatMksIslTtr.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "mksIslTtr value is not valid.",
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
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.MissingSignature.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.MissingSignature",
            Message = "Header x-jws-signature property is empty.",
            MessageTr = "İstek başlığında x-jws-signature alanı eksik."
        });

    }

    public static void SeedOBErrorCodeDetailsVersion2(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureHeaderAlgorithmWrong.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "X-JWS-Signature header in the TPP request is algorithm is wrong.",
            MessageTr = "YOS ten gelen istekteki X-JWS-Signature basligindaki alg geçersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureHeaderExpireDatePassed.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "X-JWS-Signature header in the TPP request is expired.",
            MessageTr = "YOS ten gelen istekteki X-JWS-Signature basligi zaman asimina ugramis."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureMissingBodyClaim.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "X-JWS-Signature header in the TPP request is body claim is missing.",
            MessageTr = "YOS ten gelen istekteki X-JWS-Signature basliginda body claim bulunmamaktadır."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureInvalidKey.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "X-JWS-Signature signature does not match locally computed signature.",
            MessageTr = "YOS ten gelen istekteki X-JWS-Signature kayitli public key ile dogrulanamadi."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureExMissing.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "X-JWS-Signature header in the TPP request ex is missing.",
            MessageTr = "YOS ten gelen istekteki X-JWS-Signature basligi içerisindeki ex eksik."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureExWrong.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "X-JWS-Signature header in the TPP request ex is wrong.",
            MessageTr = "YOS ten gelen istekteki X-JWS-Signature basligi içerisindeki ex hatalı."
        });

        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.MissingSignaturePSUFraudCheck.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.MissingSignature",
            Message = "Header PSU-Fraud-Check property is empty.",
            MessageTr = "İstek başlığında PSU-Fraud-Check alanı eksik."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureHeaderAlgorithmWrongFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header in the TPP request is algorithm is wrong.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligindaki alg geçersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureHeaderExpireDatePassedFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check expired.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check tarihi gecmistir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureInvalidKeyFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check signature does not match locally computed signature.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check kayitli public key ile dogrulanamadi."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureExMissingFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header in the TPP request ex is missing.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki ex eksik."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureExWrongFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header in the TPP request ex is wrong.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki ex hatalı."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureFirstLoginFlagMissingFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header in the TPP request FirstLoginFlag is missing.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki FirstLoginFlag eksik."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureFirstLoginFlagFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header in the TPP request FirstLoginFlag is wrong.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki FirstLoginFlag hatalı."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureDeviceFirstLoginFlagMissingFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header in the TPP request DeviceFirstLoginFlag is missing.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki DeviceFirstLoginFlag eksik."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureDeviceFirstLoginFlagFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header in the TPP request DeviceFirstLoginFlag is wrong.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki DeviceFirstLoginFlag hatalı."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureLastPasswordChangeFlagMissingFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header in the TPP request LastPasswordChangeFlag is missing.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki LastPasswordChangeFlag eksik."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureLastPasswordChangeFlagFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header in the TPP request LastPasswordChangeFlag is wrong.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki LastPasswordChangeFlag hatalı."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureBlacklistFlagFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header in the TPP request BlacklistFlag is wrong.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki BlacklistFlag hatalı."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureUnsafeAccountFlagFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header in the TPP request UnsafeAccountFlag is wrong.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki UnsafeAccountFlag hatalı."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureAnomalyFlagFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header in the TPP request AnomalyFlag is wrong.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki AnomalyFlag hatalı."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureMalwareFlagFraud.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header in the TPP request MalwareFlag is wrong.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligi içerisindeki MalwareFlag hatalı."
        });

        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidPermissionGetAccount.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.Forbidden",
            Message = "There is no account permission for this consent.",
            MessageTr = "İzin türü kontrolü başarısız. Hesap yetkisi olmayan rıza no ile hesap sorgulanamaz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidPermissionGetBalance.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.Forbidden",
            Message = "There is no balance permission for this consent.",
            MessageTr = "İzin türü kontrolü başarısız. Bakiye yetkisi olmayan rıza no ile bakiye sorgulanamaz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidPermissionGetTransaction.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.Forbidden",
            Message = "There is no transaction permission for this consent.",
            MessageTr = "İzin türü kontrolü başarısız. İşlem yetkisi olmayan rıza no ile işlem bilgisi sorgulanamaz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidContent.GetHashCode(),
            BkmCode = "TR.OHVPS.Business.InvalidContent",
            Message = "Invalid post message",
            MessageTr = "İstek mesajı geçersiz."
        });
          modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignaturePsuFraudCheckHeaderInvalid.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "PSU-Fraud-Check header is invalid.",
            MessageTr = "YOS ten gelen istekteki PSU-Fraud-Check basligi gecersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureXJwsSignatureHeaderInvalid.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidSignature",
            Message = "X-JWS-Signature header is invalid.",
            MessageTr = "YOS ten gelen istekteki X-JWS-Signature basligi gecersiz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InternalServerErrorBodyEmptyValidateJwt.GetHashCode(),
            BkmCode = "TR.OHVPS.Server.InternalError",
            Message = "By validating header jwt property, body not set.",
            MessageTr = "Istek başlığında bulunda xjwtsignature alanı kontrol edilirken beklenmedik bir durumla karşılaşıldı. Body değeri set edilmemiş."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldPrBrmLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 3",
            MessageTr = "boyut '3' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmAlcRequiredIfNotKolas.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "If payment is not kolas, unv and hspno is required",
            MessageTr = "Kolas işlemi değilse, unv hspno alanlarının doldu olması gerekmektedir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldTtrLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be between 1-24",
            MessageTr = "boyut '1' ile  '24' arasında olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmAcklmLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be between 1-200",
            MessageTr = "boyut '1' ile '200' arasında olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmKynkNotOpenBanking.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "odmKynk should be O refers to open banking",
            MessageTr = "odmKynk “O” değeri atanarak iletilmelidir. “O” değeri “Açık bankacılık aracılığı ile gönderilen ödemelerde kullanılır.” anlamını taşımaktadır."
        });

        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIsyOdmBlgIsyKtgKodLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 4",
            MessageTr = "boyut '4' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIsyOdmBlgAltIsyKtgKodLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 4",
            MessageTr = "boyut '4' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIsyOdmBlgGenelUyeIsyeriNoLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 8",
            MessageTr = "boyut '8' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmAlcKolasKolasDgrLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be between 7-50",
            MessageTr = "boyut '7' ile '50' arasında olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmKkodUrtcKodLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 4",
            MessageTr = "boyut '4' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidDataKareKodKolasCanNotBeUsedToGether.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.InvalidFormat",
            Message = "karekod and kolas can not be used together.",
            MessageTr = "kkod kolas aynı mesajda dolu olarak gönderilemez."
        });
         modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidContentYonAdrIsNotYosAddress.GetHashCode(),
            BkmCode = "TR.OHVPS.Business.InvalidContent",
            Message = "Gkd yonadr does not match yos api yos addresses.",
            MessageTr = "YonAdr değeri hatalı. Yos api içerisinde belirtilen temel addresler ile uyuşmamaktadır."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmAlcHspNoLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be 26",
            MessageTr = "boyut '26' olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmAlcUnvLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be between 3-140",
            MessageTr = "boyut '3' ile  '140' arasında olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmAlcKolasKolasRefNo.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "Numeric 12 length data",
            MessageTr = "boyur '12' sayısal karakter olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmOdmAyrOhkMsjLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be between 1-200",
            MessageTr = "boyut '1' ile  '200' arasında olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmGonUnvLength.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "size must be between 3-140",
            MessageTr = "boyut '3' ile  '140' arasında olmalı"
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidContentProcessingUserNotInKmlData.GetHashCode(),
            BkmCode = "TR.OHVPS.Business.InvalidContent",
            Message = "kmlk.kmlkVrs does not match processing user tckn.",
            MessageTr = "İşlem yapan kullanıcının tckn si, rıza içerisindeki kimlik verisi ile uyuşmamaktadır."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "Incorrect or missing data.",
            MessageTr = "Alan verisi eksik ya da hatalı."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.ConsentMismatchStatusNotValidToPaymentOrder.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.ConsentMismatch",
            Message = "Consent state not valid to process",
            MessageTr = "Odeme emri rıza durumu, ödeme emri işlemi yapılmaya uygun değil."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.NotFoundPaymentConsentToPaymentOrder.GetHashCode(),
            BkmCode = "TR.OHVPS.Resource.NotFound",
            Message = "Related Payment consent can not found to process payment order.",
            MessageTr = "Ödeme emri yapılmak istenilen ilişkili ödeme emri rızası kaydı bulunamadı."
        });
          modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimTipGsmIban.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "GSM/IBAN can only be used in One Time Payment",
            MessageTr = "GSM/IBAN Ohk Tanım Tipi sadece tek seferlik ödeme de kullanılabilir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTurOneTimePaymentIndividual.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "In One Time Payment, ohkTur must be individual.",
            MessageTr = "Tek seferlik ödeme de ohkTur sadece bireysel olabilir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidContentKolasNotValidInOneTimePayment.GetHashCode(),
            BkmCode = "TR.OHVPS.Business.InvalidContent",
            Message = "Kolas can not be used in one time payment.",
            MessageTr = "Tek seferlik ödeme işlemlerinde kolas kullanılamaz."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmGonUnvOneShouldBeEmptyTimePayment.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "Gon Unv should not be sent in One Time Payment",
            MessageTr = "Gon Unv alanı tek seferlik ödeme de gönderilmemelidir/boş olmalıdır."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldDataKrmKmlkDataShouldBeNull.GetHashCode(),
            BkmCode = "TR.OHVPS.Field.Invalid",
            Message = "When ohkTur is individual, institution data should not be in request data.",
            MessageTr = "Ohk Tur bireysel olan rızalarda, kurumsal kimlik bilgileri gönderilmemelidir."
        });
    }

    public static void SeedOBErrorCodeDetailsVersion3Payment(this ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidContentOneTimePaymentPSUSessionId.GetHashCode(),
            BkmCode = "TR.OHVPS.Business.InvalidContent",
            Message = "Header PSU-Session-ID must be empty in one time payment.",
            MessageTr = "Tek seferlik ödeme gibi ÖHK’nın tanınmadan başlatıldığı işlemlerde PSU-Session-ID başlık değeri boş olarak iletilmelidir."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidContentNoYosRoleForSubscription.GetHashCode(),
            BkmCode = "TR.OHVPS.Business.InvalidContent",
            Message = "Yos does not have desired role to make event subscription for selected eventype/sourcetypes",
            MessageTr = "Yos abone olunmak istenilen abonelik tipleri için gerekli role e sahip değil."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidContentYosNotHaveApiDefinition.GetHashCode(),
            BkmCode = "TR.OHVPS.Business.InvalidContent",
            Message = "Yos does not have desired api definition.",
            MessageTr = "Olay Abonelik kaydı oluşturmak isteyen YÖS'ün ODS API tanımı bulunmamaktadır."
        });
        modelBuilder.Entity<OBErrorCodeDetail>().HasData(new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            InternalCode = OBErrorCodeConstants.ErrorCodesEnum.InvalidContentThereIsAlreadyEventSubscriotion.GetHashCode(),
            BkmCode = "TR.OHVPS.Business.InvalidContent",
            Message = "Yos already has event subscription in the system.",
            MessageTr = "1 YÖS'ün 1 HHS'de 1 adet abonelik kaydı olabilir. Kaynak çakışması."
        });
        
    }

}