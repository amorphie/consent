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
        public const string GkdTur = "gkd.yetYntm";
        public const string GkdyonAdr = "gkd.yonAdr";
        public const string GkdYetTmmZmn = "gkd.yetTmmZmn";
        public const string GkdAyrikGkdHbr = "gkd.ayrikGkd";
        public const string GkdAyrikGkdOhkTanimDeger = "gkd.ayrikGkd.ohkTanimDeger";
        public const string GkdAyrikGkdOhkTanimTip = "gkd.ayrikGkd.ohkTanimTip";
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
        public const string OdmBsltmIslTtrTtr = "odmBsltm.islTtr.ttr";
        public const string OdmBsltmIslTtrPrBrm = "odmBsltm.islTtr.prBrm";
        public const string OdmBsltmOdmAyrOdmKynk = "odmBsltm.odmAyr.odmKynk";
        public const string OdmBsltmOdmAyrOdmAmc = "odmBsltm.odmAyr.odmAmc";
        public const string OdmBsltmOdmAyrOdmAcklm = "odmBsltm.odmAyr.odmAcklm";
        public const string OdmBsltmOdmAyrOhkMsj = "odmBsltm.odmAyr.ohkMsj";
        public const string OdmBsltmOdmAyrOdmStm = "odmBsltm.odmAyr.odmStm";
        public const string OdmBsltmOdmAyrRefBlg = "odmBsltm.odmAyr.refBlg";
        public const string OdmBsltmAlc = "odmBsltm.alc";
        public const string OdmBsltmAlcUnv = "odmBsltm.alc.unv";
        public const string OdmBsltmAlcHspNo = "odmBsltm.alc.hspNo";
        public const string OdmBsltmAlcKolasKolasDgr = "odmBsltm.alc.kolas.kolasDgr";
        public const string OdmBsltmAlcKolasKolasTur = "odmBsltm.alc.kolas.kolasTur";
        public const string OdmBsltmAlcKolasKolasRefNo = "odmBsltm.alc.kolas.kolasRefNo";
        public const string OdmBsltmAlcKolasKolasHspTur = "odmBsltm.alc.kolas.kolasHspTur";
        public const string OdmBsltmKkodAksTur = "odmBsltm.kkod.aksTur";
        public const string OdmBsltmKkodUrtcKod = "odmBsltm.kkod.kkodUrtcKod";
        public const string OdmBsltmKkodKkodRef = "odmBsltm.kkod.kkodRef";
        public const string OdmBsltmGonUnv = "odmBsltm.gon.unv";
        public const string OdmBsltmGonHspNo = "odmBsltm.gon.hspNo";
        public const string OdmBsltmKmlkKmlkTur = "odmBsltm.kmlk.kmlkTur";
        public const string OdmBsltmKmlkKmlkVrs = "odmBsltm.kmlk.kmlkVrs";
        public const string OdmBsltmKmlkKrmKmlkTur = "odmBsltm.kmlk.krmKmlkTur";
        public const string OdmBsltmKmlkKrmKmlkVrs = "odmBsltm.kmlk.krmKmlkVrs";
        public const string OdmBsltmKmlkOhkTur = "odmBsltm.kmlk.ohkTur";
        public const string RzBlgRizaDrm = "rzBlg.rizaDrm";
        public const string RzBlgRizaNo = "rzBlg.rizaNo";
        public const string RzBlgOlusZmn = "rzBlg.olusZmn";
        public const string IsyOdmBlgIsyKtgKod = "isyOdmBlg.isyKtgKod";
        public const string IsyOdmBlgAltIsyKtgKod = "isyOdmBlg.altIsyKtgKod";
        public const string IsyOdmBlgGenelUyeIsyeriNo = "isyOdmBlg.genelUyeIsyeriNo";
        public const string OlayTipiOA = "abonelikTipleri.olayTipi";
        public const string KaynakTipiOA = "abonelikTipleri.kaynakTipi";


    }
    public static class ObjectNames
    {
        public const string HesapBilgisiRizasiIstegi = "HesapBilgisiRizasiIstegi";
        public const string OdemeEmriRizasiIstegi = "OdemeEmriRizasiIstegi";
        public const string OdemeEmriIstegi = "OdemeEmriIstegi";
        public const string OlayAbonelikIstegi = "OlayAbonelikIstegi";
        public const string RzBlg = "rzBlg";
        public const string KatilimciBlg = "katilimciBlg";
        public const string Gkd = "gkd";
        public const string Kmlk = "kmlk";
        public const string HspBlg = "hspBlg";
        public const string HspBlgIznBlg = "hspBlg.iznBlg";
        public const string GkdAyrikGkd = "gkd.ayrikGkd";
        public const string OdmBsltm = "odmBsltm";
        public const string OdmBsltmKmlk = "odmBsltm.kmlk";
        public const string OdmBsltmIslTtr = "odmBsltm.islTtr";
        public const string OdmBsltmAlc = "odmBsltm.alc";
        public const string OdmBsltmOdmAyr = "odmBsltm.odmAyr";
        public const string OdmBsltmGon = "odmBsltm.gon";
        public const string AbonelikTipleri = "abonelikTipleri";
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
        InvalidFieldPrBrmLength = 26,
        InvalidFieldTtrLength = 27,
        InvalidFieldOdmAcklmLength = 28,
        InvalidFieldOdmKynkNotOpenBanking = 29,
        InvalidFieldIsyOdmBlgIsyKtgKodLength = 30,
        InvalidFieldIsyOdmBlgAltIsyKtgKodLength = 31,
        InvalidFieldIsyOdmBlgGenelUyeIsyeriNoLength = 32,
        InvalidFieldOdmBsltmAlcRequiredIfNotKolas = 33,
        InvalidFieldOdmBsltmAlcKolasKolasDgrLength = 34,
        InvalidFieldOdmBsltmKkodUrtcKodLength = 35,
        InvalidFieldOdmBsltmAlcUnvLength = 36,
        InvalidFieldOdmBsltmAlcHspNoLength = 37,
        InvalidFieldOdmBsltmAlcKolasKolasRefNo=38,
        InvalidFieldOdmBsltmOdmAyrOhkMsjLength=39,
        InvalidFieldOdmBsltmGonUnvLength = 40,
        InvalidFieldMissingOrInCorrect = 41,
        InvalidFieldOhkTanimTipGsmIban = 42,
        InvalidFieldOhkTurOneTimePaymentIndividual = 43,
        InvalidFieldOdmBsltmGonUnvOneShouldBeEmptyTimePayment = 44,
        InvalidFieldDataKrmKmlkDataShouldBeNull = 45,
        
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
        InvalidDataKareKodKolasCanNotBeUsedToGether = 119,
        NotFoundPaymentConsentToPaymentOrder = 120,
        
        NotFound = 150,
        InternalServerError = 151,
        InternalServerErrorCheckingIdempotency = 152,
        InternalServerErrorBodyEmptyValidateJwt = 153,
        
        
        ConsentMismatch = 160,
        ConsentMismatchStateNotValidToDelete = 161,
        ConsentMismatchAccountPostAlreadyAuthroized = 162,
        ConsentRevokedStateEnd = 163,
        ConsentRevokedStateNotAutUsed = 164,
        ConsentMismatchStatusNotValidToPaymentOrder = 165,
        
        InvalidContentPsuInitiated = 200,
        InvalidContentUserReference = 201,
        InvalidContentConsentIdInHeader = 202,
        InvalidContent = 203,
        InvalidContentYonAdrIsNotYosAddress = 204,
        InvalidContentProcessingUserNotInKmlData = 205,
        InvalidContentNoYosRoleForSubscription = 206,
        InvalidContentYosNotHaveApiDefinition = 207,
        InvalidContentThereIsAlreadyEventSubscriotion = 208,
        MissingSignature = 300,
        InvalidSignatureHeaderAlgorithmWrong = 301,
        InvalidSignatureHeaderExpireDatePassed = 302,
        InvalidSignatureMissingBodyClaim = 303,
        InvalidSignatureInvalidKey = 304,
        InvalidSignatureExMissing = 305,
        InvalidSignatureExWrong = 306,
        
        MissingSignaturePSUFraudCheck = 307,
        InvalidSignatureHeaderAlgorithmWrongFraud = 308,
        InvalidSignatureHeaderExpireDatePassedFraud = 309,
        InvalidSignatureInvalidKeyFraud = 310,
        InvalidSignatureFirstLoginFlagMissingFraud = 311,
        InvalidSignatureExMissingFraud = 312,
        InvalidSignatureExWrongFraud = 313,
        InvalidSignatureFirstLoginFlagFraud = 314,
        InvalidSignatureDeviceFirstLoginFlagMissingFraud = 315,
        InvalidSignatureDeviceFirstLoginFlagFraud = 316,
        InvalidSignatureLastPasswordChangeFlagMissingFraud = 317,
        InvalidSignatureLastPasswordChangeFlagFraud = 318,
        InvalidSignatureBlacklistFlagFraud = 319,
        InvalidSignatureUnsafeAccountFlagFraud = 320,
        InvalidSignatureAnomalyFlagFraud = 321,
        InvalidSignatureMalwareFlagFraud = 322,
  
        InvalidPermissionGetAccount = 323,
        InvalidPermissionGetBalance = 324,
        InvalidPermissionGetTransaction = 325,
        InvalidSignaturePsuFraudCheckHeaderInvalid = 326,
        InvalidSignatureXJwsSignatureHeaderInvalid = 327,
        InvalidContentKolasNotValidInOneTimePayment = 328,
        InvalidContentOneTimePaymentPSUSessionId = 329

    }
}