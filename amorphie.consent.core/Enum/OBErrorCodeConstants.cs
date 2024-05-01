using System.Net;

namespace amorphie.consent.core.Enum;

public static class OBErrorCodeConstants
{
    static OBErrorCodeConstants()
    {
    }

    public static class FieldNames
    {
        public const string HeaderPsuInitiated = "X-Request-ID";
        public const string HeaderXGroupId = "X-Group-ID";
        public const string HeaderXaspspCode = "X-ASPSP-Code";
        public const string HeaderXRequestId = "X-TPP-Code";
        public const string HeaderXtppCode = "PSU-Initiated";
        public const string HhsCodeHbr = "katilimciBlg.hhsKod";
        public const string YosCodeHbr = "katilimciBlg.yosKod";
        public const string GkdTurHbr = "gkd.yetYntm";
        public const string GkdyonAdrHbr = "gkd.yonAdr";
        public const string GkdAyrikGkdHbr = "gkd.ayrikGkd";
        public const string GkdAyrikGkdOhkTanimDegerHbr = "gkd.ayrikGkd.ohkTanimDeger";
        public const string GkdAyrikGkdOhkTanimTipHbr = "gkd.ayrikGkd.ohkTanimTip";
        public const string KmlkKmlkTur = "kmlk.kmlkTur";
        public const string KmlkKmlkVrs = "kmlk.kmlkVrs";
        public const string KmlkKrmKmlkTur = "kmlk.krmKmlkTur";
        public const string KmlkKrmKmlkVrs = "kmlk.krmKmlkVrs";
        public const string KmlkOhkTur = "kmlk.ohkTur";
        public const string HspBlgIznBlg = "hspBlg.iznBlg";
        public const string HspBlgIznTur = "hspBlg.iznTur";
        public const string HspBlgErisimIzniSonTrh = "hspBlg.iznBlg.erisimIzniSonTrh";
        public const string HspBlgHesapIslemBslZmn = "hspBlg.iznBlg.hesapIslemBslZmn";
        public const string HspBlgHesapIslemBtsZmn = "hspBlg.iznBlg.hesapIslemBtsZmn";
        public const string HspBlgHesapIslemBslZmnBtsZmn = "hspBlg.iznBlg.hesapIslemBslZmn-hspBlg.iznBlg.hesapIslemBtsZmn";

    }
    public static class ObjectNames
    {
        public const string HesapBilgisiRizasiIstegi = "HesapBilgisiRizasiIstegi";
        public const string KatilimciBlg = "katilimciBlg";
        public const string Gkd = "gkd";
        public const string Kmlk = "kmlk";
        public const string HspBlg = "hspBlg";
        public const string HspBlgIznBlg = "hspBlg.iznBlg";
        public const string GkdAyrikGkdHbr = "gkd.ayrikGkd";
    }

    public static class HttpMessage
    {
        public const string BadRequest = "Bad Request";
        public const string NotFound = "Not Found";
        public const string InternalServerError = "Internal Server Error";
        public const string Forbidden = "Forbidden";
        
    }

    public enum ErrorCodesEnum
    {
        FieldCanNotBeNullWithName = 1,
        FieldCanNotBeNull = 2,
        InvalidFieldData = 3,
        InvalidFieldHhsCodeYosCodeLength = 4,
        InvalidFieldOhkTanimDegerLength = 5,
        InvalidFieldOhkTanimTipLength = 6,
        InvalidFieldOhkTanimDegerTcknLength = 7,
        InvalidFieldOhkTanimDegerMnoLength = 8,
        InvalidFieldOhkTanimDegerYknLength = 9,
        InvalidFieldOhkTanimDegerPnoLength = 10,
        InvalidFieldOhkTanimDegerGsmLength = 11,
        InvalidFieldOhkTanimDegerIbanLength = 12,
        InvalidFieldKmlkTcknLength = 13,
        InvalidFieldKmlkMnoLength = 14,
        InvalidFieldKmlkYknLength = 15,
        InvalidFieldKmlkPnoLength = 16,
        InvalidFieldKmlkVknLength = 17,
        InvalidFieldIznTurNoTemelHesap = 18,
        InvalidFieldIznTurAyrintiliIslemWithoutTemelIslem = 19,
        InvalidFieldIznTurAnlikBakiyeWithoutBakiye = 20,
        InvalidFieldErisimIzniSonTrh = 21,
        InvalidFieldTransactionSelectedDateNotSet = 22,
        InvalidFieldDateSetTransactionNotSelected = 23,
        InvalidFieldHesapIslemDateRange = 24,
        InvalidFieldesapIslemBslZmnLaterThanBtsZmn = 25,



        InvalidFormatValidationError = 100,
        InvalidAspsp = 101,
        InvalidTpp = 102,
        InvalidTppRole = 103,
        GkdTanimDegerKimlikNotMatch = 104,
        AyrikGkdEventSubscriptionNotFound = 105,
        InvalidFormatSyfKytSayi = 106,
        InvalidFormatSrlmKrtrAccount = 107,
        InvalidFormatSrlmYon = 108,
        InvalidFormathesapIslemBslBtsTrh = 109,
        InvalidFormathesapIslemBtsTrhLaterThanToday = 110,
        InvalidFormatesapIslemBslZmnLaterThanBtsZmn = 111,
        InvalidFormatBireyselDateDiff = 112,
        InvalidFormatKurumsalDateDiff = 113,
        InvalidFormatSystemStartedDateDiff = 114,
        InvalidFormatBrcAlc = 115,
        InvalidFormatSrlmKrtrTransaction = 116,
        InvalidFormatMinIslTtr = 117,
        InvalidFormatMksIslTtr = 118,
        
        
        
        NotFound = 150,

        InternalServerError = 151,
        InternalServerErrorCheckingIdempotency = 152,
        
        ConsentMismatch = 160,
        ConsentMismatchStateNotValidToDelete = 161,
        ConsentMismatchAccountPostAlreadyAuthroized = 162,
        ConsentRevokedStateEnd = 163,
        ConsentRevokedStateNotAutUsed = 164,
        InvalidContentPsuInitiated = 200,
        InvalidContentUserReference = 201,
        InvalidContentConsentIdInHeader = 202,
        MissingSignature = 300,
        InvalidSignatureHeaderAlgorithmWrong = 301,
        InvalidSignatureHeaderExpireDatePassed = 302,
        InvalidSignatureMissingBodyClaim = 303,
        InvalidSignatureInvalidKey = 304,
        InvalidSignatureExMissing = 305,
        InvalidSignatureExWrong = 306,
        InvalidPermissionGetAccount = 310,
        InvalidPermissionGetBalance = 311,
        InvalidPermissionGetTransaction = 312

    }
}