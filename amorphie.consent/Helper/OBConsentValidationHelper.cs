using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.Service.Interface;
using Microsoft.EntityFrameworkCore;

namespace amorphie.consent.Helper;

public static class OBConsentValidationHelper
{
    /// <summary>
    /// Checks account consent post data null objects
    /// </summary>
    /// <param name="rizaIstegi"></param>
    /// <param name="context"></param>
    /// <param name="errorCodeDetails"></param>
    /// <param name="errorResponse"></param>
    /// <returns></returns>
    public static bool PrepareAndCheckInvalidFormatProperties_HBRObject(HesapBilgisiRizaIstegiHHSDto rizaIstegi,
        HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, out OBCustomErrorResponseDto errorResponse)
    {
        //Get 400 error response
        errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();

        //Field can not be empty error code
        var errorCodeDetail = OBErrorResponseHelper.GetErrorCodeDetail_DefaultInvalidField(errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);

        // Check each property and add errors if necessary
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(rizaIstegi.katilimciBlg,
            OBErrorCodeConstants.ObjectNames.KatilimciBlg,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(rizaIstegi.gkd, OBErrorCodeConstants.ObjectNames.Gkd,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(rizaIstegi.kmlk, OBErrorCodeConstants.ObjectNames.Kmlk,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(rizaIstegi.hspBlg,
            OBErrorCodeConstants.ObjectNames.HspBlg,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(rizaIstegi.hspBlg?.iznBlg,
            OBErrorCodeConstants.ObjectNames.HspBlgIznBlg,
            errorCodeDetail, errorResponse);

        // Return false if any errors were added, indicating an issue with the header
        return !errorResponse.FieldErrors.Any();
    }

    /// <summary>
    /// Cheks kmlk object and data
    /// </summary>
    /// <param name="kmlk"></param>
    /// <param name="context"></param>
    /// <param name="errorCodeDetails"></param>
    /// <returns></returns>
    public static ApiResult CheckKmlkData(KimlikDto kmlk, HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails)
    {
        ApiResult result = new();
        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);

        errorResponse.FieldErrors = new List<FieldError>();
        // Check each property and add errors if necessary
        if (string.IsNullOrEmpty(kmlk.kmlkTur))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkTur, OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);

        if (string.IsNullOrEmpty(kmlk.kmlkVrs))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkVrs, OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);

        if (string.IsNullOrEmpty(kmlk.krmKmlkTur) != string.IsNullOrEmpty(kmlk.krmKmlkVrs))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKrmKmlkTur, OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData);

        if (string.IsNullOrEmpty(kmlk.ohkTur))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkOhkTur, OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);

        if (!ConstantHelper.GetKimlikTurList().Contains(kmlk.kmlkTur))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkTur, OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData);

        if (!ConstantHelper.GetOHKTurList().Contains(kmlk.ohkTur))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkOhkTur, OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData);

        if (!string.IsNullOrEmpty(kmlk.krmKmlkTur) &&
            !ConstantHelper.GetKurumKimlikTurList().Contains(kmlk.krmKmlkTur))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKrmKmlkTur, OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData);

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        result = CheckKmlkConstraints(kmlk, context, errorCodeDetails, errorResponse);
        return result;
    }

    public static ApiResult CheckKmlkConstraints(KimlikDto kmlk, HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, OBCustomErrorResponseDto errorResponse)
    {
        ApiResult result = new();
        errorResponse.FieldErrors = new List<FieldError>();
        // Check field constraints and add errors if necessary
        if (kmlk.kmlkTur == OpenBankingConstants.KimlikTur.TCKN && !IsTcknValid(kmlk.kmlkVrs))
        {
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkTcknLength);
        }
        else if (kmlk.kmlkTur == OpenBankingConstants.KimlikTur.MNO
                 && !IsMnoValid(kmlk.kmlkVrs))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkMnoLength);
        }
        else if (kmlk.kmlkTur == OpenBankingConstants.KimlikTur.YKN
                 && !IsYknValid(kmlk.kmlkVrs))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkYknLength);
        }
        else if (kmlk.kmlkTur == OpenBankingConstants.KimlikTur.PNO
                 && !IsPnoValid(kmlk.kmlkVrs))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkPnoLength);
        }

        if (kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.TCKN
            && !IsTcknValid(kmlk.krmKmlkVrs))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKrmKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkTcknLength);
        }
        else if (kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.MNO && !IsMnoValid(kmlk.krmKmlkVrs))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKrmKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkMnoLength);
        }
        else if (kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.VKN && !IsVknValid(kmlk.krmKmlkVrs))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKrmKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkVknLength);
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
        }

        return result;
    }

    public static ApiResult CheckIznBlgTur(IzinBilgisiRequestDto iznBlg, HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails)
    {
        //Check izinbilgisi properties
        ApiResult result = new();
        OBCustomErrorResponseDto errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();
        result.Data = errorResponse;
        if (iznBlg.iznTur.Any() == false)
        {
            // iznTur is not present
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgIznBlg,
                OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);
            return result;
        }

        // Check if iznTur contains invalid values
        if (iznBlg.iznTur.Any(i => !ConstantHelper.GetIzinTurList().Contains(i)))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgIznTur, OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData);
        }

        // Check if TemelHesapBilgisi permission is present
        if (!iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.TemelHesapBilgisi))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgIznTur,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIznTurNoTemelHesap);
        }

        // Check if AyrintiliIslemBilgisi permission requires TemelIslemBilgisi
        if (iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.AyrintiliIslemBilgisi) &&
            !iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.TemelIslemBilgisi))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgIznTur,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIznTurAyrintiliIslemWithoutTemelIslem);
        }

        // Check if AnlikBakiyeBildirimi permission requires BakiyeBilgisi
        if (iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.AnlikBakiyeBildirimi) &&
            !iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.BakiyeBilgisi))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgIznTur,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIznTurAnlikBakiyeWithoutBakiye);
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        var todayDate = DateTime.UtcNow.Date;
        //check erisimIzniSonTrh
        if (iznBlg.erisimIzniSonTrh.Date == DateTime.MinValue
            || iznBlg.erisimIzniSonTrh > todayDate.AddMonths(6).AddDays(1)
            || iznBlg.erisimIzniSonTrh < todayDate.AddDays(2))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgErisimIzniSonTrh,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldErisimIzniSonTrh);
        }


        //Check işlem sorgulama başlangıç zamanı
        var transactionPermissionSelected = iznBlg.iznTur.Any(p =>
            p == OpenBankingConstants.IzinTur.TemelIslemBilgisi
            || p == OpenBankingConstants.IzinTur
                .AyrintiliIslemBilgisi);
        if (transactionPermissionSelected == false
            && (iznBlg.hesapIslemBslZmn.HasValue || iznBlg.hesapIslemBtsZmn.HasValue))
        {
            //işlem bilgisi seçilmediği zaman, başlama ve bitiş zamanı olmamalı
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgHesapIslemBslZmnBtsZmn,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldDateSetTransactionNotSelected);
        }

        if (transactionPermissionSelected
            && (!iznBlg.hesapIslemBslZmn.HasValue || !iznBlg.hesapIslemBtsZmn.HasValue))
        {
            //işlem bilgisi seçildiği zaman, başlama ve bitiş zamanı set edilmiş olmalı
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgHesapIslemBslZmnBtsZmn,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldTransactionSelectedDateNotSet);
        }

        if (iznBlg.hesapIslemBslZmn.HasValue
            && (iznBlg.hesapIslemBslZmn.Value < todayDate.AddMonths(-12) ||
                iznBlg.hesapIslemBslZmn.Value > todayDate.AddMonths(12).AddDays(1))) //Data constraints
        {
            //max +12 ay, min -12 ay olabilir
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgHesapIslemBslZmn,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldHesapIslemDateRange);
        }

        if (iznBlg.hesapIslemBtsZmn.HasValue &&
            (iznBlg.hesapIslemBtsZmn.Value < todayDate.AddMonths(-12) ||
             iznBlg.hesapIslemBtsZmn.Value > todayDate.AddMonths(12).AddDays(1))) //Data constraints
        {
            //max +12 ay, min -12 ay olabilir
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgHesapIslemBtsZmn,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldHesapIslemDateRange);
        }

        if (iznBlg is { hesapIslemBslZmn: not null, hesapIslemBtsZmn: not null }
            && iznBlg.hesapIslemBslZmn.Value > iznBlg.hesapIslemBtsZmn.Value) //Data constraints
        {
            //max +12 ay, min -12 ay olabilir
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgHesapIslemBslZmn,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldesapIslemBslZmnLaterThanBtsZmn);
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
        }

        return result;
    }


    /// <summary>
    /// Checks if gkd data is valid
    /// </summary>
    /// <param name="gkd">To be checked data</param>
    /// <param name="kimlik">Identity Information in consent</param>
    /// <param name="context"></param>
    /// <param name="errorCodeDetails"></param>
    /// <returns>Is gkd data valid</returns>
    public static async Task<ApiResult> IsGkdValid_Hbr(GkdRequestDto gkd, KimlikDto kimlik, string yosCode,
        HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, IOBEventService eventService)
    {
        ApiResult result = new();
        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();
        result.Data = errorResponse;
        if (string.IsNullOrEmpty(gkd.yetYntm)) //YetYntm is no set set
        {
            //TODO:Özlem. Set edilmeyebilir. Ama set edilmediği durumda nasıl ilerleyeceğimiz konuşulmalı. Patlamasın diye zorunluymuş gibi ilerltiyoruz.
            return result;
        }

        //Check data
        if (!ConstantHelper.GetGKDTurList().Contains(gkd.yetYntm))
        {
            //GDKTur value is not valid
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.GkdTurHbr,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData);
            return result;
        }

        if (gkd.yetYntm == OpenBankingConstants.GKDTur.Yonlendirmeli
            && string.IsNullOrEmpty(gkd.yonAdr))
        {
            //YonAdr should be set
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.GkdyonAdrHbr,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);
            result.Result = false;
            return result;
        }

        if (gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik)
        {
            //AyrikGKD object should be set
            if (gkd.ayrikGkd == null)
            {
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.ObjectNames.GkdAyrikGkdHbr,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);
                result.Result = false;
                return result;
            }

            result = await ValidateAyrikGkd(gkd.ayrikGkd, yosCode, errorCodeDetails, errorResponse, eventService,
                context); //validate ayrik gkd data
            if (!result.Result)
            {
                //Not valid
                return result;
            }

            //From Document:
            //Rıza başlatma akışı içerisinde kimlik bilgisinin olduğu durumlarda; ÖHK'ya ait kimlik verisi(kmlk.kmlkVrs) ile ayrık GKD içerisinde
            //yer alan OHK Tanım Değer alanı (ayrikGkd.ohkTanimDeger) birebir aynı olmalıdır.
            //Kimlik alanı içermeyen tek seferlik ödeme emri akışlarında bu kural geçerli değildir. 
            if (kimlik.kmlkTur == OpenBankingConstants.KimlikTur.TCKN
                && kimlik.kmlkVrs != gkd.ayrikGkd.ohkTanimDeger)
            {
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.GkdTanimDegerKimlikNotMatch);
                return result;
            }
        }

        return result;
    }

    /// <summary>
    /// Adds fields error of given error code to OBCustomErrorResponseDto object
    /// </summary>
    private static void AddFieldError_DefaultInvalidField(List<OBErrorCodeDetail> errorCodeDetails,
        OBCustomErrorResponseDto errorResponse, string propertyName, OBErrorCodeConstants.ErrorCodesEnum errorCode)
    {
        errorResponse.FieldErrors?.Add(OBErrorResponseHelper.GetFieldErrorObject_DefaultInvalidField(errorCodeDetails,
            propertyName, errorCode, OBErrorCodeConstants.ObjectNames.HesapBilgisiRizasiIstegi));
    }

    /// <summary>
    /// Validates ayrık gkd data inside gkd object
    /// </summary>
    private static async Task<ApiResult> ValidateAyrikGkd(AyrikGkdDto ayrikGkd, string yosCode,
        List<OBErrorCodeDetail> errorCodeDetails,
        OBCustomErrorResponseDto errorResponse, IOBEventService eventService, HttpContext context)
    {
        ApiResult result = new()
        {
            Data = errorResponse
        };
        bool isSubscribed =
            await eventService.IsSubscsribedForAyrikGkd(yosCode, ConsentConstants.ConsentType.OpenBankingAccount);
        if (!isSubscribed)
        {
            //No subscription for ayrik gkd
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.AyrikGkdEventSubscriptionNotFound);
            return result;
        }

        errorResponse.FieldErrors = new List<FieldError>();
        if (string.IsNullOrEmpty(ayrikGkd.ohkTanimDeger))
        {
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDegerHbr,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);
        }
        else if (ayrikGkd.ohkTanimDeger.Length < 1 || ayrikGkd.ohkTanimDeger.Length > 30) //Check length
        {
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDegerHbr,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerLength);
        }

        if (string.IsNullOrEmpty(ayrikGkd.ohkTanimTip))
        {
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimTipHbr,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);
        }
        else if (ayrikGkd.ohkTanimTip.Length < 1 || ayrikGkd.ohkTanimTip.Length > 8) //Check length
        {
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimTipHbr,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimTipLength);
        }

        if (errorResponse.FieldErrors.Any())
        {
            return result;
        }

        if (!ConstantHelper.GetOhkTanimTipList().Contains(ayrikGkd.ohkTanimTip))
        {
            //GDKTur value is not valid
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimTipHbr,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData);
            return result;
        }

        //Check GKDTanımDeger values
        ValidateOhkTanimDeger(ayrikGkd, errorCodeDetails, errorResponse, result);


        return result;
    }

    /// <summary>
    /// Validates, data check of ohktanımdeğer
    /// </summary>
    /// <param name="ayrikGkd"></param>
    /// <param name="errorCodeDetails"></param>
    /// <param name="errorResponse"></param>
    /// <param name="result"></param>
    private static void ValidateOhkTanimDeger(AyrikGkdDto ayrikGkd, List<OBErrorCodeDetail> errorCodeDetails,
        OBCustomErrorResponseDto errorResponse, ApiResult result)
    {
        errorResponse.FieldErrors = new List<FieldError>();
        switch (ayrikGkd.ohkTanimTip)
        {
            case OpenBankingConstants.OhkTanimTip.TCKN:
                if (!IsTcknValid(ayrikGkd.ohkTanimDeger))
                {
                    result.Result = false;
                    AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                        OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDegerHbr,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerTcknLength);
                }

                break;
            case OpenBankingConstants.OhkTanimTip.MNO:
                if (!IsMnoValid(ayrikGkd.ohkTanimDeger))
                {
                    result.Result = false;
                    AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                        OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDegerHbr,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerMnoLength);
                }

                break;
            case OpenBankingConstants.OhkTanimTip.YKN:
                if (!IsYknValid(ayrikGkd.ohkTanimDeger))
                {
                    result.Result = false;
                    AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                        OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDegerHbr,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerYknLength);
                }

                break;
            case OpenBankingConstants.OhkTanimTip.PNO:
                if (!IsPnoValid(ayrikGkd.ohkTanimDeger))
                {
                    result.Result = false;
                    AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                        OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDegerHbr,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerPnoLength);
                }

                break;
            case OpenBankingConstants.OhkTanimTip.GSM:
                if (!IsGsmValid(ayrikGkd.ohkTanimDeger))
                {
                    result.Result = false;
                    AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                        OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDegerHbr,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerGsmLength);
                }

                break;
            case OpenBankingConstants.OhkTanimTip.IBAN:
                if (!IsIbanValid(ayrikGkd.ohkTanimDeger))
                {
                    result.Result = false;
                    AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                        OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDegerHbr,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerIbanLength);
                }

                break;
            default:
                break;
        }
    }


    /// <summary>
    /// Checks if tckn data is valid
    /// </summary>
    /// <param name="tckn">To be checked data</param>
    /// <returns>Is valid tckn</returns>
    private static bool IsTcknValid(string? tckn)
    {
        return !string.IsNullOrEmpty(tckn) && tckn.Trim().Length == 11 && tckn.All(char.IsDigit);
    }

    /// <summary>
    /// Checks if the Customer Number (MNO) data is valid.
    /// </summary>
    /// <param name="mno">The data to be checked.</param>
    /// <returns>True if the MNO is valid; otherwise, false.</returns>
    private static bool IsMnoValid(string? mno)
    {
        return !string.IsNullOrEmpty(mno) && mno.Trim().Length >= 1 && mno.Trim().Length <= 30;
    }

    /// <summary>
    /// Checks if the Yabancı Kimlik Numarası (YKN) data is valid.
    /// </summary>
    /// <param name="ykn">The data to be checked.</param>
    /// <returns>True if the YKN is valid; otherwise, false.</returns>
    private static bool IsYknValid(string ykn)
    {
        return !string.IsNullOrEmpty(ykn) && ykn.Trim().Length == 11 && ykn.All(char.IsDigit);
    }

    /// <summary>
    /// Checks if the Passport Number (PNO) data is valid.
    /// </summary>
    /// <param name="pno">The data to be checked.</param>
    /// <returns>True if the PNO is valid; otherwise, false.</returns>
    private static bool IsPnoValid(string pno)
    {
        return !string.IsNullOrEmpty(pno) && pno.Trim().Length >= 7 && pno.Trim().Length <= 9;
    }

    /// <summary>
    /// Checks if the GSM (Global System for Mobile Communications) number data is valid.
    /// </summary>
    /// <param name="gsm">The data to be checked.</param>
    /// <returns>True if the GSM number is valid; otherwise, false.</returns>
    private static bool IsGsmValid(string gsm)
    {
        return !string.IsNullOrEmpty(gsm) && gsm.Trim().Length == 10 && gsm.All(char.IsDigit);
    }

    /// <summary>
    /// Checks if the International Bank Account Number (IBAN) data is valid.
    /// </summary>
    /// <param name="iban">The data to be checked.</param>
    /// <returns>True if the IBAN is valid; otherwise, false.</returns>
    private static bool IsIbanValid(string iban)
    {
        return !string.IsNullOrEmpty(iban) && iban.Trim().Length == 26 && iban.All(char.IsDigit);
    }

    /// <summary>
    /// Checks if the vergi kimlik numarası data is valid.
    /// </summary>
    /// <param name="vkn">The data to be checked.</param>
    /// <returns>True if the VKN is valid; otherwise, false.</returns>
    private static bool IsVknValid(string? vkn)
    {
        return !string.IsNullOrEmpty(vkn) && vkn.Trim().Length == 10;
    }

    /// <summary>
    /// Checks if KatilimciBlgData is valid
    /// </summary>
    /// <param name="context"></param>
    /// <param name="configuration"></param>
    /// <param name="katilimciBlg"></param>
    /// <param name="errorCodeDetails"></param>
    /// <returns></returns>
    public static ApiResult IsKatilimciBlgDataValid(HttpContext context,
        IConfiguration configuration,
        KatilimciBilgisiDto katilimciBlg,
        List<OBErrorCodeDetail> errorCodeDetails)
    {
        ApiResult result = new();

        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();

        //Field can not be empty error code
        var errorCodeDetail = OBErrorResponseHelper.GetErrorCodeDetail_DefaultInvalidField(errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);
        var invalidFieldHhsCodeYosCodeLength =
            OBErrorResponseHelper.GetErrorCodeDetail_DefaultInvalidField(errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldHhsCodeYosCodeLength);
        if (string.IsNullOrEmpty(katilimciBlg.hhsKod)) //Check hhskod 
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HhsCodeHbr,
                OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);
        }
        else if (katilimciBlg.hhsKod.Length != 4) //Check hhskod length
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HhsCodeHbr,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldHhsCodeYosCodeLength);
        }

        if (string.IsNullOrEmpty(katilimciBlg.yosKod)) //Check yoskod 
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.YosCodeHbr, OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);
        }
        else if (katilimciBlg.yosKod.Length != 4) //Check yoskod length
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.YosCodeHbr,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldHhsCodeYosCodeLength);
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        //Check HHSCode is correct
        if (configuration["HHSCode"] != katilimciBlg.hhsKod)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidAspsp);
            return result;
        }

        return result;
    }
    
    
    /// <summary>
    /// Checks if parameters valid to get balances and accounts
    /// </summary>
    /// <returns></returns>
    public static ApiResult IsParametersValidToGetAccountsBalances(HttpContext context, List<OBErrorCodeDetail> errorCodeDetails,
        int syfKytSayi,
        string srlmKrtr,
        string srlmYon)
    {
        ApiResult result = new();
        var today = DateTime.UtcNow;
     
        if (syfKytSayi > OpenBankingConstants.AccountServiceParameters.syfKytSayi 
            || syfKytSayi <=0 )
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSyfKytSayi);
            return result;
        }

        if (!string.IsNullOrEmpty(srlmKrtr)
            && OpenBankingConstants.AccountServiceParameters.srlmKrtrAccountAndBalance != srlmKrtr)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSrlmKrtrAccount);
            return result;
        }

        if (!string.IsNullOrEmpty(srlmYon)
            && !ConstantHelper.GetSrlmYonList().Contains(srlmYon))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSrlmYon);
            return result;
        }
        return result;
    }

    /// <summary>
    /// Checks if parameters valid to get transactions
    /// </summary>
    /// <returns>Parameters validation result</returns>
    public static ApiResult IsParametersValidToGetTransactionsByHspRef(HttpContext context, List<OBErrorCodeDetail> errorCodeDetails,Consent consent,
        string psuInitiated,
        DateTime hesapIslemBslTrh,
        DateTime hesapIslemBtsTrh,
        string? minIslTtr,
        string? mksIslTtr,
        string? brcAlc,
        int syfKytSayi,
        string srlmKrtr,
        string srlmYon)
    {
        ApiResult result = new();
        var today = DateTime.UtcNow;
        if (hesapIslemBtsTrh == DateTime.MinValue
            || hesapIslemBslTrh == DateTime.MinValue) //required parameters
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormathesapIslemBslBtsTrh);
            return result;
        }

        //Check hesapIslemBtsTrh
        if (hesapIslemBtsTrh > today)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormathesapIslemBtsTrhLaterThanToday);
            return result;
        }

        if (hesapIslemBtsTrh < hesapIslemBslTrh)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatesapIslemBslZmnLaterThanBtsZmn);
            return result;
        }

        //ÖHK tarafından tetiklenen sorgularda; hesapIslemBslTrh ve hesapIslemBtsTrh arası fark bireysel ÖHK’lar için en fazla 1 ay,kurumsal ÖHK’lar için ise en fazla 1 hafta olabilir.
        if (psuInitiated == OpenBankingConstants.PSUInitiated.OHKStarted)
        {
            if (consent.OBAccountConsentDetails.FirstOrDefault()?.UserType == OpenBankingConstants.OHKTur.Bireysel)
            {
                if (hesapIslemBslTrh.AddMonths(1) < hesapIslemBtsTrh)
                {
                    result.Result = false;
                    result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatBireyselDateDiff);
                    return result;
                }
            }
            else if (consent.OBAccountConsentDetails.FirstOrDefault()?.UserType == OpenBankingConstants.OHKTur.Kurumsal)
            {
                if (hesapIslemBslTrh.AddDays(7) < hesapIslemBtsTrh)
                {
                    result.Result = false;
                    result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatKurumsalDateDiff);
                    return result;
                }
            }
        }

        //YÖS tarafından sistemsel yapılan sorgulamalarda hem bireysel, hem de kurumsal ÖHK’lar için;son 24 saat sorgulanabilir. Bu yüzden hesapIslemBtsTrh-24 saat’ten daha uzun bir aralık sorgulanamaz olmalıdır.
        if (psuInitiated == OpenBankingConstants.PSUInitiated.SystemStarted
            && (hesapIslemBtsTrh - hesapIslemBslTrh).TotalHours > 24)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSystemStartedDateDiff);
            return result;
        }

        if (!string.IsNullOrEmpty(brcAlc) && !ConstantHelper.GetBrcAlcList().Contains(brcAlc))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatBrcAlc);
            return result;
        }
        

        if (!string.IsNullOrEmpty(minIslTtr) && !ConstantHelper.IsValidAmount(minIslTtr))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatMinIslTtr);
            return result;
        }
        if (!string.IsNullOrEmpty(mksIslTtr) && !ConstantHelper.IsValidAmount(mksIslTtr))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatMksIslTtr);
            return result;
        }
        
        if (syfKytSayi > OpenBankingConstants.AccountServiceParameters.syfKytSayi 
            || syfKytSayi <=0 )
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSyfKytSayi);
            return result;
        }

        if (!string.IsNullOrEmpty(srlmKrtr)
            && OpenBankingConstants.AccountServiceParameters.srlmKrtrTransaction != srlmKrtr)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSrlmKrtrTransaction);
            return result;
        }

        if (!string.IsNullOrEmpty(srlmYon)
            && !ConstantHelper.GetSrlmYonList().Contains(srlmYon))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatSrlmYon);
            return result;
        }


        return result;
    }
    
    /// <summary>
    /// Checks if header is valid by controlling;
    /// PSU Initiated value is in predefined values
    /// Required fields are checked
    /// XASPSPCode is equal with BurganBank hhscode
    /// Checks hhskod yoskod if katilimciBlg parameter is set.
    /// </summary>
    /// <param name="context">Context</param>
    /// <param name="configuration">Configuration instance</param>
    /// <param name="yosInfoService">Yos service instance</param>
    /// <param name="header">Header object</param>
    /// <param name="katilimciBlg">Katilimci data object default value with null</param>
    /// <param name="isUserRequired">There should be userreference value in header. Optional parameter with default false value</param>
    /// <param name="isConsentIdRequired">There should be openbanking_consent_id in header. Optional parameter with default false value</param>
    /// <returns>Validation result</returns>
    public static async Task<ApiResult> IsHeaderDataValid(HttpContext context,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        RequestHeaderDto? header = null,
        KatilimciBilgisiDto? katilimciBlg = null,
        bool? isUserRequired = false,
        bool? isConsentIdRequired = false,
        List<OBErrorCodeDetail>? errorCodeDetails = null)
    {
        ApiResult result = new();
        header ??= OBModuleHelper.GetHeader(context);

        errorCodeDetails ??= new List<OBErrorCodeDetail>();
       
         // Separate method to prepare and check header properties
        if (!OBErrorResponseHelper.PrepareAndCheckHeaderInvalidFormatProperties(header, context, errorCodeDetails, out var errorResponse))
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        if (configuration["HHSCode"] != header.XASPSPCode)
        {
            //XASPSPCode value should be BurganBanks hhscode value
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidAspsp);
            return result;
        }

        if (ConstantHelper.GetPSUInitiatedValues().Contains(header.PSUInitiated) == false)
        {
            //Check psu initiated value
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidContentPsuInitiated);
            return result;
        }

        //Check setted yos value
        var yosCheckResult = await yosInfoService.IsYosInApplication(header.XTPPCode);
        if (yosCheckResult.Result == false
            || yosCheckResult.Data == null
            || (bool)yosCheckResult.Data == false)
        {
            //No yos data in the system
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidTpp);
            return result;
        }

        if (isUserRequired.HasValue
            && isUserRequired.Value
            && string.IsNullOrEmpty(header.UserReference))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidContentUserReference);
            return result;
        }
        
        if (isConsentIdRequired.HasValue
            && isConsentIdRequired.Value
            && string.IsNullOrEmpty(header.ConsentId))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidContentConsentIdInHeader);
            return result;
        }

        //If there is katilimciBlg object, validate data in it with header
        if (katilimciBlg != null)
        {
            //Check header data and message data
            if (header.XASPSPCode != katilimciBlg.hhsKod)
            {
                //HHSCode must match with header x-aspsp-code
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidAspsp);
                return result;
            }

            if (header.XTPPCode != katilimciBlg.yosKod)
            {
                //YOSCode must match with header x-tpp-code
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidTpp);
                return result;
            }
        }

        return result;
    }

    
    
    
}