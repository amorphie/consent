using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;

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
    /// Checks if gkd data is valid
    /// </summary>
    /// <param name="gkd">To be checked data</param>
    /// <param name="kimlik">Identity Information in consent</param>
    /// <returns>Is gkd data valid</returns>
    public static ApiResult IsGkdValid_Hbr(GkdRequestDto gkd, KimlikDto kimlik, HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails)
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

        if ((gkd.yetYntm == OpenBankingConstants.GKDTur.Yonlendirmeli
             && string.IsNullOrEmpty(gkd.yonAdr)))
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

            result = ValidateAyrikGkd(gkd.ayrikGkd, errorCodeDetails, errorResponse); //validate ayrik gkd data
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
    /// <param name="errorCodeDetails">All error codes</param>
    /// <param name="errorResponse">Response object</param>
    /// <param name="propertyName">To be checked property name</param>
    /// <param name="errorCode">Error code - internal code</param>
    private static void AddFieldError_DefaultInvalidField(List<OBErrorCodeDetail> errorCodeDetails,
        OBCustomErrorResponseDto errorResponse, string propertyName, OBErrorCodeConstants.ErrorCodesEnum errorCode)
    {
        errorResponse.FieldErrors?.Add(OBErrorResponseHelper.GetFieldErrorObject_DefaultInvalidField(errorCodeDetails,
            propertyName, errorCode, OBErrorCodeConstants.ObjectNames.HesapBilgisiRizasiIstegi));
    }

    /// <summary>
    /// Validates ayrık gkd data inside gkd object
    /// </summary>
    /// <param name="ayrikGkd"></param>
    /// <param name="errorCodeDetails"></param>
    /// <param name="errorResponse"></param>
    /// <returns></returns>
    private static ApiResult ValidateAyrikGkd(AyrikGkdDto ayrikGkd, List<OBErrorCodeDetail> errorCodeDetails,
        OBCustomErrorResponseDto errorResponse)
    {
        ApiResult result = new()
        {
            Data = errorResponse
        };
        errorResponse.FieldErrors = new List<FieldError>();
        if (string.IsNullOrEmpty(ayrikGkd?.ohkTanimDeger))
        {
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDegerHbr,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);
        }
        else if (ayrikGkd?.ohkTanimDeger.Length < 1 || ayrikGkd?.ohkTanimDeger.Length > 30) //Check length
        {
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDegerHbr,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerLength);
        }

        if (string.IsNullOrEmpty(ayrikGkd?.ohkTanimTip))
        {
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimTipHbr,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);
        }
        else if (ayrikGkd?.ohkTanimTip.Length != 8) //Check length
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

        if (!ConstantHelper.GetOhkTanimTipList().Contains(ayrikGkd!.ohkTanimTip))
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
    /// <param name="gkd"></param>
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
    private static bool IsTcknValid(string tckn)
    {
        return !string.IsNullOrEmpty(tckn) && tckn.Trim().Length == 11 && tckn.All(char.IsDigit);
    }

    /// <summary>
    /// Checks if the Customer Number (MNO) data is valid.
    /// </summary>
    /// <param name="mno">The data to be checked.</param>
    /// <returns>True if the MNO is valid; otherwise, false.</returns>
    private static bool IsMnoValid(string mno)
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
            errorResponse.FieldErrors.Add(OBErrorResponseHelper.GetFieldErrorObject(
                OBErrorCodeConstants.FieldNames.HhsCodeHbr, errorCodeDetail,
                OBErrorCodeConstants.ObjectNames.HesapBilgisiRizasiIstegi));
        }
        else if (katilimciBlg.hhsKod.Length != 4) //Check hhskod length
        {
            errorResponse.FieldErrors.Add(OBErrorResponseHelper.GetFieldErrorObject(
                OBErrorCodeConstants.FieldNames.HhsCodeHbr, invalidFieldHhsCodeYosCodeLength,
                OBErrorCodeConstants.ObjectNames.HesapBilgisiRizasiIstegi));
        }

        if (string.IsNullOrEmpty(katilimciBlg.yosKod)) //Check yoskod 
        {
            errorResponse.FieldErrors.Add(OBErrorResponseHelper.GetFieldErrorObject(
                OBErrorCodeConstants.FieldNames.YosCodeHbr, errorCodeDetail,
                OBErrorCodeConstants.ObjectNames.HesapBilgisiRizasiIstegi));
        }
        else if (katilimciBlg.yosKod.Length != 4) //Check yoskod length
        {
            errorResponse.FieldErrors.Add(OBErrorResponseHelper.GetFieldErrorObject(
                OBErrorCodeConstants.FieldNames.YosCodeHbr, invalidFieldHhsCodeYosCodeLength,
                OBErrorCodeConstants.ObjectNames.HesapBilgisiRizasiIstegi));
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
}