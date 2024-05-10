using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text.Json;
using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;
using amorphie.consent.data;
using amorphie.consent.Service.Interface;
using Jose;

using Microsoft.IdentityModel.Tokens;

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
            || syfKytSayi <= 0)
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
    public static ApiResult IsParametersValidToGetTransactionsByHspRef(HttpContext context, List<OBErrorCodeDetail> errorCodeDetails, Consent consent,
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
            || syfKytSayi <= 0)
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
    /// <param name="isXJwsSignatureRequired">There should be x-jws-signature in header. Optional parameter with default false value</param>
    /// <returns>Validation result</returns>
    public static async Task<ApiResult> IsHeaderDataValid(HttpContext context,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        RequestHeaderDto? header = null,
        KatilimciBilgisiDto? katilimciBlg = null,
        bool? isUserRequired = false,
        bool? isConsentIdRequired = false,
        bool? isXJwsSignatureRequired = false,
        List<OBErrorCodeDetail>? errorCodeDetails = null,
        string? body = null)
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

        result = await IsXJwsSignatureValid(context, configuration, yosInfoService, header, errorCodeDetails, body, isXJwsSignatureRequired);
        if (!result.Result)
        {
            return result;
        }
        
        result = await IsPsuFraudCheckValid(context, configuration, yosInfoService, header, errorCodeDetails, body);
        if (!result.Result)
        {
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

    public static async Task<ApiResult> IsXJwsSignatureValid(HttpContext context,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        RequestHeaderDto header,
        List<OBErrorCodeDetail> errorCodeDetails,
        string? body,
        bool? isXJwsSignatureRequired = false
        )
    {
        ApiResult result = new();
        
        //Check config value CheckXJWSSignature
        if (bool.TryParse(configuration["CheckXJWSSignature"], out bool isXJWSSignatureCheckEnabled) && !isXJWSSignatureCheckEnabled)
        { //XJWSSignature check config is false
            //NO need to check
            return result;
        }
        
        if (isXJwsSignatureRequired.HasValue
            && isXJwsSignatureRequired.Value
            && string.IsNullOrEmpty(header.XJWSSignature))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.MissingSignature);
            return result;
        }

        //No need to check header property
        if (isXJwsSignatureRequired is null
        || isXJwsSignatureRequired is false)
        {
            return result;
        }
        
        var headerXjwsSignature = header.XJWSSignature!;
         //   "eyJhbGciOiJSUzI1NiJ9.eyJpc3MiOiJodHRwczovL2FwaWd3LmJrbS5jb20udHIiLCJleHAiOjE2NjU0NzU1NjIsImlhdCI6MTY2NTM4OTE2MiwiYm9keSI6ImE2NGIxOWY5NWVlYjFmYjBhMGEzZTJkYmJjNmUzZDg0NzJjNTIxODRkNDU0MzQxN2RkYzZkMTU2ZmM1YzU1NzEifQ.Q65PI_1fTEzzBMirvmJvXgVX3orhhZ4_UqujtGdHkU7me-1ymIjvPrzy3kfyER1pedFb7HDCBuPvYoqjX8eUnpiiZsxfzCiEa0McIhoFeUOggq-O8VihItp8bLr2DWwQ9JHN1-WXB2mL31KAKFAL1VY9-DXuAdT-RfE_SLYsl2ycmNy4ti4XvfDvvlE56ZsieFZ727VuwR8wi7F0kKDc6UhjaMF9xcUeAM1fxX-bmcOaOo1NZGC0vvgjNZKz_OJrN-q8VhWYnQPiJ7wY7S9IG8bHIkBImKSVf8LuOEvl8u0BZzADLH1iOBd9x2l1plyI_NLPTrnOqhWhKlljkkJBCg";
        
        // Decode JWT to get header
        var jwtHeaderDecoded = JWT.Headers(headerXjwsSignature);
        // Verify algorithm used is RS256
        if (jwtHeaderDecoded["alg"].ToString() != "RS256")
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureHeaderAlgorithmWrong);
            return result;
        }

        var jwtPayloadDecoded = JWT.Payload(headerXjwsSignature);

        var jwtPayloadJson = JsonDocument.Parse(jwtPayloadDecoded);
        if (jwtPayloadJson.RootElement.TryGetProperty("exp", out var expValue))
        {
            if (expValue.TryGetInt64(out long expUnixTime))
            {
                // Convert the Unix time to a DateTime object
                var expDateTime = DateTimeOffset.FromUnixTimeSeconds(expUnixTime).UtcDateTime;

                // Check if the token has expired
                if (expDateTime <= DateTime.UtcNow)
                {
                    // Token is invalid
                    result.Result = false;
                    result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureHeaderExpireDatePassed);
                    return result;
                }
            }
            else
            {
                // Handle the case where "exp" property is not a valid JSON number
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureExWrong);
                return result;
            }
        }
        else
        {
            // Handle the case where "exp" property is missing
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureExMissing);
            return result;
        }

        var validateSignatureResult = await
            ValidateJWTSignature(yosInfoService, header, headerXjwsSignature, context, errorCodeDetails, body);
        if (!validateSignatureResult.Result)
        {
            return validateSignatureResult;
        }

        return result;
    }

    private static async Task<ApiResult> ValidateJWTSignature(IYosInfoService yosInfoService,
        RequestHeaderDto header, string headerXjwsSignature, HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, string body)
    {
        ApiResult result = new();
        int maxRetryCount = 2;
        int tryCount = 0;
        bool isPublicKeyUpdated = false;
        Dictionary<string, object>? payload = null;
        while (tryCount < maxRetryCount)
        {
            var getPublicKeyResult = await yosInfoService.GetYosPublicKey(header.XTPPCode);
            if (!getPublicKeyResult.Result)
            {
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetInternalServerError(context, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.MissingSignature);
                return result;
            }

            if (getPublicKeyResult.Data is null
                || string.IsNullOrEmpty((string)getPublicKeyResult.Data))
            {
                if (!isPublicKeyUpdated)
                {
                    //Get yos public key and update system
                    await yosInfoService.SaveYos(header.XTPPCode);
                    isPublicKeyUpdated = true;
                }

                ++tryCount;
                continue;
            }

            string publicKey = getPublicKeyResult.Data.ToString()!;
            // Convert the base64 encoded public key to bytes
            byte[] publicKeyBytes = Convert.FromBase64String(publicKey);

           var verifyResult= VerifyJwt(headerXjwsSignature, publicKey);
           if (verifyResult.Result)//Verified
           {
               payload = (Dictionary<string, object>?)verifyResult.Data;
               break;
           }
           if (!isPublicKeyUpdated)
           {
               //Get yos public key and update system
               await yosInfoService.SaveYos(header.XTPPCode);
               isPublicKeyUpdated = true;
           }
           tryCount++;
        }

        if (tryCount == 2)
        {
            // If token validation fails, return an error
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureInvalidKey);
            return result;
        }

        var bodyClaim = payload?["body"]?.ToString();
        if (string.IsNullOrEmpty(bodyClaim))
        {
            // If "body" claim is missing, return an error
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureMissingBodyClaim);
            return result;
        }
        
        // Calculate SHA256 hash of the request body
        var requestBodyHash = OBModuleHelper.GetChecksumSHA256(body);

        // Compare the calculated hash with the value in the "body" claim
        if (!string.Equals(bodyClaim, requestBodyHash, StringComparison.OrdinalIgnoreCase))
        {
            // If the hashes don't match, return an error
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureInvalidKey);
            return result;
        }

        return result;
    }
    
    public static ApiResult VerifyJwt(string token, string publicKey)
    {
        ApiResult result = new();
        RSA rsa = RSA.Create();
 
        // Convert the public key string to byte array
        byte[] publicKeyBytes = Convert.FromBase64String(publicKey);
 
        // Import the public key bytes to the RSA object
        rsa.ImportSubjectPublicKeyInfo(publicKeyBytes, out _);
 
 
        // Create token validation parameters
        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = false, // Set to true if you want to validate the issuer
            ValidateAudience = false, // Set to true if you want to validate the audience
            ValidateLifetime = false, // Set to true to validate the token expiration
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new RsaSecurityKey(rsa)
        };
 
        try
        {
            // Try to validate the token
            var handler = new JwtSecurityTokenHandler();
            SecurityToken securityToken;
            var principal = handler.ValidateToken(token, validationParameters, out securityToken);
            // Token is valid
            // Extract claims from the principal
            var claims = principal.Claims.ToDictionary(c => c.Type, c => (object)c.Value);
            result.Data = claims;
            return result; // Return the payload (claims)
        }
        catch (Exception ex)
        {
            // Token validation failed
            result.Result = false;
            return result;
        }
    }


    
     public static async Task<ApiResult> IsPsuFraudCheckValid(HttpContext context,
        IConfiguration configuration,
        IYosInfoService yosInfoService,
        RequestHeaderDto header,
        List<OBErrorCodeDetail> errorCodeDetails,
        string? body
        )
    {
        ApiResult result = new();
        
        //No need to check header property
        if (header.PSUInitiated == OpenBankingConstants.PSUInitiated.SystemStarted)
        {
            return result;
        }
        
        //Check CheckPSUFraudCheck config value 
        if (bool.TryParse(configuration["CheckPSUFraudCheck"], out bool isPSUFraudCheckEnabled) && !isPSUFraudCheckEnabled)
        { //CheckPSUFraudCheck config is false. No need to check
            return result;
        }
        
        if (header.PSUInitiated == OpenBankingConstants.PSUInitiated.OHKStarted
            && string.IsNullOrEmpty(header.PSUFraudCheck))//PSUFraudCheck is required when OHK started
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.MissingSignaturePSUFraudCheck);
            return result;
        }

        var headerPsuFraudCheck =  header.PSUFraudCheck!;
            // "eyJhbGciOiJSUzI1NiJ9.eyJBbm9tYWx5RmxhZyI6IjAiLCJMYXN0UGFzc3dvcmRDaGFuZ2VGbGFnIjoiMSIsIkZpcnN0TG9naW5GbGFnIjoiMSIsIkRldmljZUZpcnN0TG9naW5GbGFnIjoiMSIsIkJsYWNrbGlzdEZsYWciOiIwIiwiTWFsd2FyZUZsYWciOiIwIiwiVW5zYWZlQWNjb3VudEZsYWciOiIwIiwiZXhwIjoxNjY1NDc1NTU2LCJpYXQiOjE2NjUzODkxNTYsImlzcyI6Imh0dHBzOi8vYXBpZ3cuYmttLmNvbS50ciJ9.DhUh_nsXDuNIrvsQ3KOhOXdVcJg6fTDVW8K1oea8kLtmb7n-_hJHY3mWX5zzobu-Vh2VvFzIxPhHtol6gLHFktmIMiQ9TDHb_mRZFXgJB4ToNfqc3Fy9mi5bS8By2IYi1HxDaCStstaZDaunzXfHCtqybfZXyk6teDrf-iIf6lqX9Keo7GZO-Y7mP7C13-c_QwyNKrZK4TZwUQbecRqXYn1DcEHM7kukQHTar_hKBWkXPmNpScY0J2rKksr4ejR1uLhdQm-Pdwoe9y6qrNEB79vMLBkRNtbuV0vc1GYHp_YKkzBKBI_58uuB2GD9877CsrcRnHMQb88xpxiPKh6-ew";
     
        // Decode JWT to get header
        var jwtHeaderDecoded = JWT.Headers(headerPsuFraudCheck);
        // Verify algorithm used is RS256
        if (jwtHeaderDecoded["alg"].ToString() != "RS256")
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureHeaderAlgorithmWrongFraud);
            return result;
        }

        var jwtPayloadDecoded = JWT.Payload(headerPsuFraudCheck);
        var jwtPayloadJson = JsonDocument.Parse(jwtPayloadDecoded);

        result = ValidateExPropertyFraud(context, errorCodeDetails, jwtPayloadJson);//Validate ex property
        if (!result.Result)
            return result;

        result = ValidateRequiredZmnAralikProperty(jwtPayloadJson, "FirstLoginFlag", context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureFirstLoginFlagMissingFraud,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureFirstLoginFlagFraud);
        if (!result.Result)
            return result;
        result = ValidateRequiredZmnAralikProperty(jwtPayloadJson, "DeviceFirstLoginFlag", context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureDeviceFirstLoginFlagMissingFraud,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureDeviceFirstLoginFlagFraud);
        if (!result.Result)
            return result;
        result = ValidateRequiredZmnAralikProperty(jwtPayloadJson, "LastPasswordChangeFlag", context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureLastPasswordChangeFlagMissingFraud, 
            OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureLastPasswordChangeFlagFraud);
        if (!result.Result)
            return result;
        
      
        result = ValidateVarYokProperty(jwtPayloadJson, "BlacklistFlag", context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureBlacklistFlagFraud);
        if (!result.Result)
            return result;
        
        result = ValidateZmnAralikProperty(jwtPayloadJson, "MalwareFlag", context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureMalwareFlagFraud);
        if (!result.Result)
            return result;
      
        result = ValidateVarYokProperty(jwtPayloadJson, "AnomalyFlag", context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureAnomalyFlagFraud);
        if (!result.Result)
            return result;
      
        result = ValidateZmnAralikProperty(jwtPayloadJson, "UnsafeAccountFlag", context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureUnsafeAccountFlagFraud);
        if (!result.Result)
            return result;
        
        //Validate fraud jwt with public key
        var validateSignatureResult = await
            ValidatePsuFraudCheckJWT(yosInfoService, header, headerPsuFraudCheck, context, errorCodeDetails);
        if (!validateSignatureResult.Result)
        {
            return validateSignatureResult;
        }

        return result;
    }

     private static ApiResult ValidateExPropertyFraud(HttpContext context, List<OBErrorCodeDetail> errorCodeDetails, JsonDocument jwtPayloadJson)
     {
         ApiResult result =new();
         if (jwtPayloadJson.RootElement.TryGetProperty("exp", out var expValue))
         {
             if (expValue.TryGetInt64(out long expUnixTime))
             {
                 // Convert the Unix time to a DateTime object
                 var expDateTime = DateTimeOffset.FromUnixTimeSeconds(expUnixTime).UtcDateTime;

                 // Check if the token has expired
                 if (expDateTime <= DateTime.UtcNow)
                 {
                     // Token is invalid
                     result.Result = false;
                     result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                         OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureHeaderExpireDatePassedFraud);
                     return result;
                 }
             }
             else
             {
                 // Handle the case where "exp" property is not a valid JSON number
                 result.Result = false;
                 result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                     OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureExWrongFraud);
                 return result;
             }
         }
         else
         {
             // Handle the case where "exp" property is missing
             result.Result = false;
             result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                 OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureExMissingFraud);
             return result;
         }

         return result;
     }

     /// <summary>
     /// Validates property. It is  required property. Its value is in zmnaralik list enum.
     /// </summary>
     /// <returns>Validate property result</returns>
     private static ApiResult ValidateRequiredZmnAralikProperty(JsonDocument jwtPayloadJson, string propertyName, HttpContext context, List<OBErrorCodeDetail> errorCodeDetails , OBErrorCodeConstants.ErrorCodesEnum missingErrorCode, OBErrorCodeConstants.ErrorCodesEnum invalidErrorCode)
     {
         ApiResult result = new();
         if (jwtPayloadJson.RootElement.TryGetProperty(propertyName, out var propertyValue))
         {
             if (string.IsNullOrEmpty(propertyValue.ToString())
             || !Int32.TryParse(propertyValue.ToString(), out var propInt)
                 || !ConstantHelper.GetZmnAralikList().Contains(propInt))
             {
                 //property  is invalid
                 result.Result = false;
                 result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                     invalidErrorCode);
                 return result;
             }
         }
         else
         {
             //property is missing
             result.Result = false;
             result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                 missingErrorCode);
             return result;
         }

         return result;
     }
     
     /// <summary>
     /// Validates property. Its not requeired property. Its value is in zmnaralik list enum.
     /// </summary>
     /// <returns>Validate property result</returns>
     private static ApiResult ValidateZmnAralikProperty(JsonDocument jwtPayloadJson, string propertyName, HttpContext context, List<OBErrorCodeDetail> errorCodeDetails , OBErrorCodeConstants.ErrorCodesEnum invalidErrorCode)
     {
         ApiResult result = new();
         if (jwtPayloadJson.RootElement.TryGetProperty(propertyName, out var propertyValue))
         {
             if (!string.IsNullOrEmpty(propertyValue.ToString())
                && (!Int32.TryParse(propertyValue.ToString(), out var propInt)
                 || !ConstantHelper.GetZmnAralikList().Contains(propInt)))
             {
                 //property  is invalid
                 result.Result = false;
                 result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                     invalidErrorCode);
                 return result;
             }
         }

         return result;
     }
     
     /// <summary>
     /// Validates property. Its not requeired property. Its value is in varyok enum.
     /// </summary>
     /// <param name="jwtPayloadJson">To be checked property object</param>
     /// <param name="propertyName">property name</param>
     /// <param name="context"></param>
     /// <param name="errorCodeDetails"></param>
     /// <param name="invalidErrorCode">If not valid, which error code result will be generated</param>
     /// <returns>Validate property result</returns>
     private static ApiResult ValidateVarYokProperty(JsonDocument jwtPayloadJson, string propertyName, HttpContext context, List<OBErrorCodeDetail> errorCodeDetails , OBErrorCodeConstants.ErrorCodesEnum invalidErrorCode)
     {
         ApiResult result = new();
         if (jwtPayloadJson.RootElement.TryGetProperty(propertyName, out var propertyValue))
         {
             if (!string.IsNullOrEmpty(propertyValue.ToString())
                 &&  (!Int32.TryParse(propertyValue.ToString(), out var propInt)
                      || !ConstantHelper.GetVarYok().Contains(propInt)))
             {
                 //property  is invalid
                 result.Result = false;
                 result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                     invalidErrorCode);
                 return result;
             }
         }

         return result;
     }
     

    private static async Task<ApiResult> ValidatePsuFraudCheckJWT(IYosInfoService yosInfoService,
        RequestHeaderDto header, string headerPsuFraudCheck, HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails)
    {
        ApiResult result = new();
        int maxRetryCount = 2;
        int tryCount = 0;
        bool isPublicKeyUpdated = false;
        while (tryCount < maxRetryCount)
        {
            var getPublicKeyResult = await yosInfoService.GetYosPublicKey(header.XTPPCode);
            if (!getPublicKeyResult.Result)
            {
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetInternalServerError(context, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.MissingSignaturePSUFraudCheck);
                return result;
            }

            if (getPublicKeyResult.Data is null
                || string.IsNullOrEmpty((string)getPublicKeyResult.Data))
            {
                if (!isPublicKeyUpdated)
                {
                    //Get yos public key and update system
                    await yosInfoService.SaveYos(header.XTPPCode);
                    isPublicKeyUpdated = true;
                }

                ++tryCount;
                continue;
            }

            string publicKey = getPublicKeyResult.Data.ToString()!;
            var verifyResult= VerifyJwt(headerPsuFraudCheck, publicKey);
            if (verifyResult.Result)//Verified
            {
                break;
            }
            if (!isPublicKeyUpdated)
            {
                //Get yos public key and update system
                await yosInfoService.SaveYos(header.XTPPCode);
                isPublicKeyUpdated = true;
            }
            tryCount++;
        }

        if (tryCount == 2)
        {
            // If token validation fails, return an error
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureInvalidKeyFraud);
            return result;
        }
        
        return result;
    }


    
    
}