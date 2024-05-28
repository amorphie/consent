using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.RegularExpressions;
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
    /// Checks odeme emri rizasi consent post data null objects
    /// </summary>
    /// <returns></returns>
    public static bool PrepareAndCheckInvalidFormatProperties_OERObject(OdemeEmriRizaIstegiHHSDto rizaIstegi,
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
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(rizaIstegi.odmBsltm,
            OBErrorCodeConstants.ObjectNames.OdmBsltm,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(rizaIstegi.odmBsltm?.kmlk,
            OBErrorCodeConstants.ObjectNames.OdmBsltmKmlk,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(rizaIstegi.odmBsltm?.islTtr,
            OBErrorCodeConstants.ObjectNames.OdmBsltmIslTtr,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(rizaIstegi.odmBsltm?.alc,
            OBErrorCodeConstants.ObjectNames.OdmBsltmAlc,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(rizaIstegi.odmBsltm?.odmAyr,
            OBErrorCodeConstants.ObjectNames.OdmBsltmOdmAyr,
            errorCodeDetail, errorResponse);


        // Return false if any errors were added, indicating an issue with the header
        return !errorResponse.FieldErrors.Any();
    }

    /// <summary>
    /// Checks odeme emri istegi consent post data null objects
    /// </summary>
    /// <returns></returns>
    public static bool PrepareAndCheckInvalidFormatProperties_OEIObject(OdemeEmriIstegiHHSDto odemeEmriIstegi,
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
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(odemeEmriIstegi,
            OBErrorCodeConstants.ObjectNames.OdemeEmriIstegi,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(odemeEmriIstegi.rzBlg,
            OBErrorCodeConstants.ObjectNames.RzBlg,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(odemeEmriIstegi.katilimciBlg,
            OBErrorCodeConstants.ObjectNames.KatilimciBlg,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(odemeEmriIstegi.gkd,
            OBErrorCodeConstants.ObjectNames.Gkd,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(odemeEmriIstegi.odmBsltm,
            OBErrorCodeConstants.ObjectNames.OdmBsltm,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(odemeEmriIstegi.odmBsltm?.kmlk,
            OBErrorCodeConstants.ObjectNames.OdmBsltmKmlk,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(odemeEmriIstegi.odmBsltm?.islTtr,
            OBErrorCodeConstants.ObjectNames.OdmBsltmIslTtr,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(odemeEmriIstegi.odmBsltm?.alc,
            OBErrorCodeConstants.ObjectNames.OdmBsltmAlc,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(odemeEmriIstegi.odmBsltm?.gon,
            OBErrorCodeConstants.ObjectNames.OdmBsltmGon,
            errorCodeDetail, errorResponse);
        OBErrorResponseHelper.CheckInvalidFormatProperty_Object(odemeEmriIstegi.odmBsltm?.odmAyr,
            OBErrorCodeConstants.ObjectNames.OdmBsltmOdmAyr,
            errorCodeDetail, errorResponse);


        // Return false if any errors were added, indicating an issue with the header
        return !errorResponse.FieldErrors.Any();
    }

    /// <summary>
    /// Cheks kmlk object and data
    /// </summary>
    /// <returns></returns>
    public static ApiResult CheckKmlkData(KimlikDto kmlk, HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, string objectName)
    {
        ApiResult result = new();
        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);

        errorResponse.FieldErrors = new List<FieldError>();
        // Check each property and add errors if necessary
        if (string.IsNullOrEmpty(kmlk.kmlkTur))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkTur, OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull,
                objectName: objectName);

        if (string.IsNullOrEmpty(kmlk.kmlkVrs))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkVrs, OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull,
                objectName: objectName);

        if (string.IsNullOrEmpty(kmlk.krmKmlkTur) != string.IsNullOrEmpty(kmlk.krmKmlkVrs))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKrmKmlkTur, OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData,
                objectName: objectName);

        if (string.IsNullOrEmpty(kmlk.ohkTur))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkOhkTur, OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull,
                objectName: objectName);

        if (!ConstantHelper.GetKimlikTurList().Contains(kmlk.kmlkTur))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkTur, OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData,
                objectName: objectName);

        if (!ConstantHelper.GetOHKTurList().Contains(kmlk.ohkTur))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkOhkTur, OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData,
                objectName: objectName);

        if (!string.IsNullOrEmpty(kmlk.krmKmlkTur) &&
            !ConstantHelper.GetKurumKimlikTurList().Contains(kmlk.krmKmlkTur))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKrmKmlkTur, OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData,
                objectName: objectName);

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        result = CheckKmlkConstraints(kmlk, context, errorCodeDetails, errorResponse, objectName: objectName);
        return result;
    }

    public static ApiResult CheckKmlkConstraints(KimlikDto kmlk, HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, OBCustomErrorResponseDto errorResponse, string objectName)
    {
        ApiResult result = new();
        errorResponse.FieldErrors = new List<FieldError>();
        // Check field constraints and add errors if necessary
        if (kmlk.kmlkTur == OpenBankingConstants.KimlikTur.TCKN && !IsTcknValid(kmlk.kmlkVrs))
        {
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkTcknLength, objectName: objectName);
        }
        else if (kmlk.kmlkTur == OpenBankingConstants.KimlikTur.MNO
                 && !IsMnoValid(kmlk.kmlkVrs))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkMnoLength, objectName: objectName);
        }
        else if (kmlk.kmlkTur == OpenBankingConstants.KimlikTur.YKN
                 && !IsYknValid(kmlk.kmlkVrs))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkYknLength, objectName: objectName);
        }
        else if (kmlk.kmlkTur == OpenBankingConstants.KimlikTur.PNO
                 && !IsPnoValid(kmlk.kmlkVrs))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkPnoLength, objectName: objectName);
        }

        if (kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.TCKN
            && !IsTcknValid(kmlk.krmKmlkVrs))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKrmKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkTcknLength, objectName: objectName);
        }
        else if (kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.MNO && !IsMnoValid(kmlk.krmKmlkVrs))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKrmKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkMnoLength, objectName: objectName);
        }
        else if (kmlk.krmKmlkTur == OpenBankingConstants.KurumKimlikTur.VKN && !IsVknValid(kmlk.krmKmlkVrs))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.KmlkKrmKmlkVrs,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldKmlkVknLength, objectName: objectName);
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
        }

        return result;
    }

    public static ApiResult CheckIznBlgTur(IzinBilgisiRequestDto iznBlg, HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, string objectName)
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
                OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
            return result;
        }

        // Check if iznTur contains invalid values
        if (iznBlg.iznTur.Any(i => !ConstantHelper.GetIzinTurList().Contains(i)))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgIznTur, OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData,
                objectName: objectName);
        }

        // Check if TemelHesapBilgisi permission is present
        if (!iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.TemelHesapBilgisi))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgIznTur,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIznTurNoTemelHesap, objectName: objectName);
        }

        // Check if AyrintiliIslemBilgisi permission requires TemelIslemBilgisi
        if (iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.AyrintiliIslemBilgisi) &&
            !iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.TemelIslemBilgisi))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgIznTur,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIznTurAyrintiliIslemWithoutTemelIslem,
                objectName: objectName);
        }

        // Check if AnlikBakiyeBildirimi permission requires BakiyeBilgisi
        if (iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.AnlikBakiyeBildirimi) &&
            !iznBlg.iznTur.Contains(OpenBankingConstants.IzinTur.BakiyeBilgisi))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgIznTur,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIznTurAnlikBakiyeWithoutBakiye, objectName: objectName);
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
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldErisimIzniSonTrh, objectName: objectName);
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
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldDateSetTransactionNotSelected, objectName: objectName);
        }

        if (transactionPermissionSelected
            && (!iznBlg.hesapIslemBslZmn.HasValue || !iznBlg.hesapIslemBtsZmn.HasValue))
        {
            //işlem bilgisi seçildiği zaman, başlama ve bitiş zamanı set edilmiş olmalı
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgHesapIslemBslZmnBtsZmn,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldTransactionSelectedDateNotSet, objectName: objectName);
        }

        if (iznBlg.hesapIslemBslZmn.HasValue
            && (iznBlg.hesapIslemBslZmn.Value < todayDate.AddMonths(-12) ||
                iznBlg.hesapIslemBslZmn.Value > todayDate.AddMonths(12).AddDays(1))) //Data constraints
        {
            //max +12 ay, min -12 ay olabilir
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgHesapIslemBslZmn,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldHesapIslemDateRange, objectName: objectName);
        }

        if (iznBlg.hesapIslemBtsZmn.HasValue &&
            (iznBlg.hesapIslemBtsZmn.Value < todayDate.AddMonths(-12) ||
             iznBlg.hesapIslemBtsZmn.Value > todayDate.AddMonths(12).AddDays(1))) //Data constraints
        {
            //max +12 ay, min -12 ay olabilir
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgHesapIslemBtsZmn,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldHesapIslemDateRange, objectName: objectName);
        }

        if (iznBlg is { hesapIslemBslZmn: not null, hesapIslemBtsZmn: not null }
            && iznBlg.hesapIslemBslZmn.Value > iznBlg.hesapIslemBtsZmn.Value) //Data constraints
        {
            //max +12 ay, min -12 ay olabilir
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HspBlgHesapIslemBslZmn,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldesapIslemBslZmnLaterThanBtsZmn, objectName: objectName);
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
        }

        return result;
    }


    public static ApiResult CheckTtrData(TutarDto tutar, HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, string objectName)
    {
        ApiResult result = new();
        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);

        errorResponse.FieldErrors = new List<FieldError>();
        // Check each property and add errors if necessary
        if (string.IsNullOrEmpty(tutar.ttr))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.OdmBsltmIslTtrTtr,
                OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);

        if (string.IsNullOrEmpty(tutar.prBrm))
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.OdmBsltmIslTtrPrBrm,
                OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        //Check amount
        if (!IsTtrValid(tutar.ttr))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.OdmBsltmIslTtrTtr,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldTtrLength, objectName: objectName);
        }

        //Check para birimi
        if (!IsPrBrmValid(tutar.prBrm))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.OdmBsltmIslTtrPrBrm,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldPrBrmLength, objectName: objectName);
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        return result;
    }

    public static ApiResult CheckOdemeAyrinti(OdemeAyrintilariRequestDto odmAyr,
        HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, string objectName)
    {
        ApiResult result = new();
        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();
        result.Data = errorResponse;

        if (string.IsNullOrEmpty(odmAyr.odmKynk)) //Check Odeme kaynak
        {
            //OdmKynk should be set
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmKynk,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
        }

        if (string.IsNullOrEmpty(odmAyr.odmAmc)) //Check Odeme kaynak
        {
            //OdmAcklm should be set
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmAmc,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
        }

        if (!string.IsNullOrEmpty(odmAyr.odmAcklm) &&
            (odmAyr.odmAcklm.Length < 1 || odmAyr.odmAcklm.Length > 200)) //Check odmAcklm length
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmAcklm,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmAcklmLength, objectName: objectName);
        }

        //Check data
        if (!ConstantHelper.GetOdemeAmaciList().Contains(odmAyr.odmAmc))
        {
            //odmAmc value is not valid
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmAmc,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
        }

        if (odmAyr.odmKynk !=
            OpenBankingConstants.OdemeKaynak.AcikBankacilikAraciligiIleGonderilenOdemelerde)
        {
            //odmKynk value is not valid
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmKynk,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmKynkNotOpenBanking,
                objectName: objectName);
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        return result;
    }


    public static ApiResult CheckOdemeAyrintiOdemeEmri(OdemeAyrintilariDto odmAyr,
        HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, string objectName)
    {
        ApiResult result = new();
        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();
        result.Data = errorResponse;

        if (string.IsNullOrEmpty(odmAyr.odmKynk)) //Check Odeme kaynak
        {
            //OdmKynk should be set
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmKynk,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
        }

        if (odmAyr.odmKynk !=
            OpenBankingConstants.OdemeKaynak.AcikBankacilikAraciligiIleGonderilenOdemelerde)
        {
            //odmKynk value is not valid
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmKynk,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmKynkNotOpenBanking,
                objectName: objectName);
        }

        if (string.IsNullOrEmpty(odmAyr.odmAmc)) //Check Odeme kaynak
        {
            //OdmAcklm should be set
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmAmc,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
        }

        if (!ConstantHelper.GetOdemeAmaciList().Contains(odmAyr.odmAmc))
        {
            //odmAmc value is not valid
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmAmc,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
        }

        if (!string.IsNullOrEmpty(odmAyr.odmAcklm)
            && (odmAyr.odmAcklm.Length < 1 || odmAyr.odmAcklm.Length > 200)) //Check odmAcklm length
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmAcklm,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmAcklmLength, objectName: objectName);
        }

        if (!string.IsNullOrEmpty(odmAyr.ohkMsj)
            && (odmAyr.ohkMsj.Length < 1 || odmAyr.ohkMsj.Length > 200)) //Check odmAcklm length
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOhkMsj,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmOdmAyrOhkMsjLength, objectName: objectName);
        }

        if (string.IsNullOrEmpty(odmAyr.odmStm)) //Check Odeme system
        {
            //OdmAcklm should be set
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmStm,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
        }

        if (!ConstantHelper.GetOdemeSistemiList().Contains(odmAyr.odmStm))
        {
            //odmAmc value is not valid
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmStm,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
        }


        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        return result;
    }


    public static ApiResult CheckIsyeriOdemeBilgileri(IsyeriOdemeBilgileriDto? isyOdmBlg,
        HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, string objectName)
    {
        ApiResult result = new();
        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();
        result.Data = errorResponse;
        if (isyOdmBlg is null)
        {
            return result;
        }

        if (!string.IsNullOrEmpty(isyOdmBlg.isyKtgKod) && isyOdmBlg.isyKtgKod.Length != 4) //Check isyKtgKod length
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.IsyOdmBlgIsyKtgKod,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIsyOdmBlgIsyKtgKodLength, objectName: objectName);
        }

        if (!string.IsNullOrEmpty(isyOdmBlg.altIsyKtgKod) &&
            isyOdmBlg.altIsyKtgKod.Length != 4) //Check altIsyKtgKod length
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.IsyOdmBlgAltIsyKtgKod,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIsyOdmBlgAltIsyKtgKodLength, objectName: objectName);
        }

        if (!string.IsNullOrEmpty(isyOdmBlg.genelUyeIsyeriNo) &&
            isyOdmBlg.genelUyeIsyeriNo.Length != 8) //Check genelUyeIsyeriNo length
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.IsyOdmBlgGenelUyeIsyeriNo,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldIsyOdmBlgGenelUyeIsyeriNoLength,
                objectName: objectName);
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        return result;
    }

    public static ApiResult CheckKolasKarekodAlici(OdemeBaslatmaRequestDto odmBsltm,
        HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails,
        string objectName)
    {
        ApiResult result = new();
        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();
        result.Data = errorResponse;

        //Check odmBsltma Alıcı
        //Kolay Adres Sistemi kullanılmıyorsa zorunludur.
        if (odmBsltm.alc.kolas == null
            && (string.IsNullOrEmpty(odmBsltm.alc.unv)
                || string.IsNullOrEmpty(odmBsltm.alc.hspNo)))
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.OdmBsltmAlc,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmAlcRequiredIfNotKolas, objectName: objectName);
            result.Result = false;
        }

        //Check kolas
        if (odmBsltm.alc.kolas != null)
        {
            if (string.IsNullOrEmpty(odmBsltm.alc.kolas.kolasDgr))
            {
                //kolasDgr must be set
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcKolasKolasDgr,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
            }

            if (!string.IsNullOrEmpty(odmBsltm.alc.kolas.kolasDgr)
                && (odmBsltm.alc.kolas.kolasDgr.Length < 7
                    || odmBsltm.alc.kolas.kolasDgr.Length > 50)) //Check kolasDgr length
            {
                AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                    OBErrorCodeConstants.FieldNames.OdmBsltmAlcKolasKolasDgr,
                    OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmAlcKolasKolasDgrLength,
                    objectName: objectName);
            }

            if (string.IsNullOrEmpty(odmBsltm.alc.kolas.kolasTur))
            {
                //kolasTur must be set
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcKolasKolasTur,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
            }

            if (!ConstantHelper.GetKolasTurList().Contains(odmBsltm.alc.kolas.kolasTur))
            {
                //kolasTur value is not valid
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcKolasKolasTur,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
            }
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        if (odmBsltm.alc.kolas != null
            && odmBsltm.kkod != null)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidDataKareKodKolasCanNotBeUsedToGether);
            return result;
        }

        //Check kkod
        if (odmBsltm.kkod != null)
        {
            if (string.IsNullOrEmpty(odmBsltm.kkod.aksTur))
            {
                //aksTur must be set
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmKkodAksTur,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
            }

            if (!ConstantHelper.GetKolasTurList().Contains(odmBsltm.kkod.aksTur))
            {
                //aksTur value is not valid
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmKkodAksTur,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
            }

            if (string.IsNullOrEmpty(odmBsltm.kkod.kkodUrtcKod))
            {
                //kkodUrtcKod must be set
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmKkodUrtcKod,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
            }

            if (!string.IsNullOrEmpty(odmBsltm.kkod.kkodUrtcKod)
                && odmBsltm.kkod.kkodUrtcKod.Length != 4) //Check kkodUrtcKod length
            {
                AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                    OBErrorCodeConstants.FieldNames.OdmBsltmKkodUrtcKod,
                    OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmKkodUrtcKodLength, objectName: objectName);
            }
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        return result;
    }


    public static ApiResult CheckKolasKarekodAliciOdemeEmri(OdemeBaslatmaDto odmBsltm,
        HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails,
        string objectName)
    {
        ApiResult result = new();
        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();
        result.Data = errorResponse;

        //Check odmBsltma Alıcı - Required
        if (string.IsNullOrEmpty(odmBsltm.alc.unv))
        {
            //odmBsltm.alc.unv must be set
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcUnv,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
        }

        if (!string.IsNullOrEmpty(odmBsltm.alc.unv)
            && (odmBsltm.alc.unv.Length < 3
                || odmBsltm.alc.unv.Length > 140)) //Check alc.unv length
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.OdmBsltmAlcUnv,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmAlcUnvLength, objectName: objectName);
        }

        if (string.IsNullOrEmpty(odmBsltm.alc.hspNo))
        {
            //odmBsltm.alc.hspNo must be set
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcHspNo,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
        }

        if (!string.IsNullOrEmpty(odmBsltm.alc.hspNo)
            && odmBsltm.alc.hspNo.Length != 26) //Check alc.hspNo length
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.OdmBsltmAlcHspNo,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmAlcHspNoLength, objectName: objectName);
        }

        //Check kolas
        if (odmBsltm.alc.kolas != null)
        {
            if (string.IsNullOrEmpty(odmBsltm.alc.kolas.kolasDgr))
            {
                //kolasDgr must be set
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcKolasKolasDgr,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
            }

            if (!string.IsNullOrEmpty(odmBsltm.alc.kolas.kolasDgr)
                && (odmBsltm.alc.kolas.kolasDgr.Length < 7
                    || odmBsltm.alc.kolas.kolasDgr.Length > 50)) //Check kolasDgr length
            {
                AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                    OBErrorCodeConstants.FieldNames.OdmBsltmAlcKolasKolasDgr,
                    OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmAlcKolasKolasDgrLength,
                    objectName: objectName);
            }

            if (string.IsNullOrEmpty(odmBsltm.alc.kolas.kolasTur))
            {
                //kolasTur must be set
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcKolasKolasTur,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
            }

            if (!string.IsNullOrEmpty(odmBsltm.alc.kolas.kolasTur)
                && !ConstantHelper.GetKolasTurList().Contains(odmBsltm.alc.kolas.kolasTur))
            {
                //kolasTur value is not valid
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcKolasKolasTur,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
            }

            if (odmBsltm.alc.kolas.kolasRefNo.ToString().Length != 12)
            {
                //kolasRefNo value is not valid
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcKolasKolasRefNo,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmAlcKolasKolasRefNo,
                    objectName: objectName);
            }

            if (string.IsNullOrEmpty(odmBsltm.alc.kolas.kolasHspTur))
            {
                //kolasHspTur must be set
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcKolasKolasHspTur,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
            }

            if (!string.IsNullOrEmpty(odmBsltm.alc.kolas.kolasHspTur)
                && !ConstantHelper.GetKolasHspTurList().Contains(odmBsltm.alc.kolas.kolasHspTur))
            {
                //kolasTur value is not valid
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcKolasKolasHspTur,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
            }
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        if (odmBsltm.alc.kolas != null
            && odmBsltm.kkod != null)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidDataKareKodKolasCanNotBeUsedToGether);
            return result;
        }

        //Check kkod
        if (odmBsltm.kkod != null)
        {
            if (string.IsNullOrEmpty(odmBsltm.kkod.aksTur))
            {
                //aksTur must be set
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmKkodAksTur,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
            }

            if (!string.IsNullOrEmpty(odmBsltm.kkod.aksTur)
                && !ConstantHelper.GetKolasTurList().Contains(odmBsltm.kkod.aksTur))
            {
                //aksTur value is not valid
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmKkodAksTur,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
            }

            if (string.IsNullOrEmpty(odmBsltm.kkod.kkodUrtcKod))
            {
                //kkodUrtcKod must be set
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmKkodUrtcKod,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
            }

            if (!string.IsNullOrEmpty(odmBsltm.kkod.kkodUrtcKod)
                && odmBsltm.kkod.kkodUrtcKod.Length != 4) //Check kkodUrtcKod length
            {
                AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                    OBErrorCodeConstants.FieldNames.OdmBsltmKkodUrtcKod,
                    OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmKkodUrtcKodLength,
                    objectName: objectName);
            }
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        return result;
    }

    public static ApiResult CheckGonderen(GonderenHesapDto gonderen,
        HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails,
        string objectName)
    {
        ApiResult result = new();
        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();
        result.Data = errorResponse;

        //Check odmBsltma gon - Required
        if (string.IsNullOrEmpty(gonderen.unv))
        {
            //odmBsltm.gon.unv must be set
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmGonUnv,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
        }

        if (!string.IsNullOrEmpty(gonderen.unv)
            && (gonderen.unv.Length < 3
                || gonderen.unv.Length > 140)) //Check alc.unv length
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.OdmBsltmGonUnv,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOdmBsltmGonUnvLength, objectName: objectName);
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        return result;
    }

    public static ApiResult CheckRizaBlg(RizaBilgileriRequestDto rzBlg,
        HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, string objectName)
    {
        ApiResult result = new();
        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();
        result.Data = errorResponse;

        if (string.IsNullOrEmpty(rzBlg.rizaDrm))
        {
            //rizaDrm must be set
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.RzBlgRizaDrm,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
        }

        if (!string.IsNullOrEmpty(rzBlg.rizaDrm)
            && !ConstantHelper.GetRizaDurumuList().Contains(rzBlg.rizaDrm))
        {
            //rizaDrm value is not valid
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.RzBlgRizaDrm,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
        }

        if (string.IsNullOrEmpty(rzBlg.rizaNo))
        {
            //rizaNo must be set
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.RzBlgRizaNo,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
        }

        if (!string.IsNullOrEmpty(rzBlg.rizaNo)
            && !Guid.TryParse(rzBlg.rizaNo, out Guid rizaNo))
        {
            //rizaNo value is not valid
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.RzBlgRizaNo,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
        }

        if (rzBlg.olusZmn == DateTime.MinValue)
        {
            //olusZmn value is not valid
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.RzBlgOlusZmn,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        return result;
    }


    /// <summary>
    /// Checks if gkd data is valid
    /// </summary>
    /// <returns>Is gkd data valid</returns>
    public static async Task<ApiResult> IsGkdValid(GkdRequestDto gkd, KimlikDto kimlik, string yosCode,
        HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails,
        IOBEventService eventService,
        IYosInfoService yosInfoService,
        string objectName)
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
                propertyName: OBErrorCodeConstants.FieldNames.GkdTur,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
            return result;
        }

        if (gkd.yetYntm == OpenBankingConstants.GKDTur.Yonlendirmeli
            && string.IsNullOrEmpty(gkd.yonAdr))
        {
            //YonAdr should be set
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.GkdyonAdr,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
            result.Result = false;
            return result;
        }

        if (gkd.yetYntm == OpenBankingConstants.GKDTur.Yonlendirmeli)
        {
            //Check yonAdr
            //Check setted yos value
            var yosCheckResult = await yosInfoService.IsYosAddressCorrect(yosCode, gkd.yetYntm, gkd.yonAdr);
            if (yosCheckResult.Result == false
                || yosCheckResult.Data == null
                || (bool)yosCheckResult.Data == false)
            {
                //No yos data in the system
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.InvalidContentYonAdrIsNotYosAddress);
                return result;
            }
        }

        if (gkd.yetYntm == OpenBankingConstants.GKDTur.Ayrik)
        {
            //AyrikGKD object should be set
            if (gkd.ayrikGkd == null)
            {
                AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                    propertyName: OBErrorCodeConstants.ObjectNames.GkdAyrikGkd,
                    errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
                result.Result = false;
                return result;
            }

            result = await ValidateAyrikGkd(gkd.ayrikGkd, yosCode, errorCodeDetails, errorResponse, eventService,
                context, objectName: objectName); //validate ayrik gkd data
            if (!result.Result)
            {
                //Not valid
                return result;
            }

            //From Document:
            //Rıza başlatma akışı içerisinde kimlik bilgisinin olduğu durumlarda; ÖHK'ya ait kimlik verisi(kmlk.kmlkVrs) ile ayrık GKD içerisinde
            //yer alan OHK Tanım Değer alanı (ayrikGkd.ohkTanimDeger) birebir aynı olmalıdır.
            //Kimlik alanı içermeyen tek seferlik ödeme emri akışlarında bu kural geçerli değildir. 
            if (!string.IsNullOrEmpty(kimlik.kmlkVrs)
                && (kimlik.kmlkVrs != gkd.ayrikGkd.ohkTanimDeger
                || kimlik.kmlkTur != gkd.ayrikGkd.ohkTanimTip))
            {
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.GkdTanimDegerKimlikNotMatch);
                return result;
            }
        }

        return result;
    }


    public static async Task<ApiResult> IsGkdValid(GkdDto gkd, KimlikDto kimlik, string yosCode,
        HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails,
        IOBEventService eventService,
        IYosInfoService yosInfoService,
        string objectName)
    {
        ApiResult result = new();
        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        result.Data = errorResponse;
        if (gkd.yetTmmZmn == DateTime.MinValue)
        {
            //YonAdr should be set
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.GkdYetTmmZmn,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
            result.Result = false;
            return result;
        }

        return await IsGkdValid(
            new GkdRequestDto() { ayrikGkd = gkd.ayrikGkd, yetYntm = gkd.yetYntm, yonAdr = gkd.yonAdr },
            kimlik, yosCode, context, errorCodeDetails, eventService, yosInfoService, objectName);
    }

    /// <summary>
    /// Adds fields error of given error code to OBCustomErrorResponseDto object
    /// </summary>
    private static void AddFieldError_DefaultInvalidField(List<OBErrorCodeDetail> errorCodeDetails,
        OBCustomErrorResponseDto errorResponse, string propertyName, OBErrorCodeConstants.ErrorCodesEnum errorCode,
        string objectName)
    {
        //TODO:Özlem burada hesapbilgisi rızası kalmış bunu kaldır.
        errorResponse.FieldErrors?.Add(OBErrorResponseHelper.GetFieldErrorObject_DefaultInvalidField(errorCodeDetails,
            propertyName, errorCode, objectName));
    }

    /// <summary>
    /// Validates ayrık gkd data inside gkd object
    /// </summary>
    private static async Task<ApiResult> ValidateAyrikGkd(AyrikGkdDto ayrikGkd, string yosCode,
        List<OBErrorCodeDetail> errorCodeDetails,
        OBCustomErrorResponseDto errorResponse, IOBEventService eventService, HttpContext context, string objectName)
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
                propertyName: OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDeger,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
        }
        else if (ayrikGkd.ohkTanimDeger.Length < 1 || ayrikGkd.ohkTanimDeger.Length > 30) //Check length
        {
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDeger,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerLength, objectName: objectName);
        }

        if (string.IsNullOrEmpty(ayrikGkd.ohkTanimTip))
        {
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimTip,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
        }
        else if (ayrikGkd.ohkTanimTip.Length < 1 || ayrikGkd.ohkTanimTip.Length > 8) //Check length
        {
            result.Result = false;
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimTip,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimTipLength, objectName: objectName);
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
                OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimTip,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldData, objectName: objectName);
            return result;
        }

        //Check GKDTanımDeger values
        result = ValidateOhkTanimDeger(ayrikGkd, errorCodeDetails, errorResponse, objectName: objectName);
        return result;
    }

    /// <summary>
    /// Validates, data check of ohktanımdeğer
    /// </summary>
    private static ApiResult ValidateOhkTanimDeger(AyrikGkdDto ayrikGkd, List<OBErrorCodeDetail> errorCodeDetails,
        OBCustomErrorResponseDto errorResponse, string objectName)
    {
        ApiResult result = new()
        {
            Data = errorResponse
        };
        errorResponse.FieldErrors = new List<FieldError>();
        switch (ayrikGkd.ohkTanimTip)
        {
            case OpenBankingConstants.OhkTanimTip.TCKN:
                if (!IsTcknValid(ayrikGkd.ohkTanimDeger))
                {
                    result.Result = false;
                    AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                        OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDeger,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerTcknLength,
                        objectName: objectName);
                }

                break;
            case OpenBankingConstants.OhkTanimTip.MNO:
                if (!IsMnoValid(ayrikGkd.ohkTanimDeger))
                {
                    result.Result = false;
                    AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                        OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDeger,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerMnoLength, objectName: objectName);
                }

                break;
            case OpenBankingConstants.OhkTanimTip.YKN:
                if (!IsYknValid(ayrikGkd.ohkTanimDeger))
                {
                    result.Result = false;
                    AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                        OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDeger,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerYknLength, objectName: objectName);
                }

                break;
            case OpenBankingConstants.OhkTanimTip.PNO:
                if (!IsPnoValid(ayrikGkd.ohkTanimDeger))
                {
                    result.Result = false;
                    AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                        OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDeger,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerPnoLength, objectName: objectName);
                }

                break;
            case OpenBankingConstants.OhkTanimTip.GSM:
                if (!IsGsmValid(ayrikGkd.ohkTanimDeger))
                {
                    result.Result = false;
                    AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                        OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDeger,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerGsmLength, objectName: objectName);
                }

                break;
            case OpenBankingConstants.OhkTanimTip.IBAN:
                if (!IsIbanValid(ayrikGkd.ohkTanimDeger))
                {
                    result.Result = false;
                    AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                        OBErrorCodeConstants.FieldNames.GkdAyrikGkdOhkTanimDeger,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldOhkTanimDegerIbanLength,
                        objectName: objectName);
                }

                break;
            default:
                break;
        }

        return result;
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
    /// Checks if ttr data is valid
    /// </summary>
    /// <param name="ttr">To be checked data</param>
    /// <returns>Is valid amount</returns>
    private static bool IsTtrValid(string ttr)
    {
        return !string.IsNullOrEmpty(ttr) && ttr.Trim().Length >= 1 && ttr.Trim().Length <= 24 && IsValidAmount(ttr);
    }

    /// <summary>
    /// Checks para birimi data is  valid
    /// </summary>
    /// <param name="prBrm">To be checked data</param>
    /// <returns>Is valid para birimi</returns>
    private static bool IsPrBrmValid(string prBrm)
    {
        return !string.IsNullOrEmpty(prBrm) && prBrm.Trim().Length == 3;
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
        List<OBErrorCodeDetail> errorCodeDetails,
        string objectName)
    {
        ApiResult result = new();

        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();

        if (string.IsNullOrEmpty(katilimciBlg.hhsKod)) //Check hhskod 
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HhsCodeHbr,
                OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull, objectName: objectName);
        }
        else if (katilimciBlg.hhsKod.Length != 4) //Check hhskod length
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.HhsCodeHbr,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldHhsCodeYosCodeLength, objectName: objectName);
        }

        if (string.IsNullOrEmpty(katilimciBlg.yosKod)) //Check yoskod 
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.YosCodeHbr, OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull,
                objectName: objectName);
        }
        else if (katilimciBlg.yosKod.Length != 4) //Check yoskod length
        {
            AddFieldError_DefaultInvalidField(errorCodeDetails, errorResponse,
                OBErrorCodeConstants.FieldNames.YosCodeHbr,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldHhsCodeYosCodeLength, objectName: objectName);
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

    public static ApiResult CheckOdemeEmriRizasiOdemeEmri(OdemeEmriRizasiHHSDto odemeEmriRizasi,
        OdemeEmriIstegiHHSDto odemeEmriIstegi,
        HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, string objectName)
    {
        ApiResult result = new();
        //Get 400 error response
        var errorResponse = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
            OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();
        result.Data = errorResponse;

        //odmBsltma Kmlk must be same
        if (odemeEmriRizasi.odmBsltm.kmlk.kmlkTur != odemeEmriIstegi.odmBsltm.kmlk.kmlkTur)
        {
            //kmlkTur should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmKmlkKmlkTur,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm.kmlk.kmlkVrs != odemeEmriIstegi.odmBsltm.kmlk.kmlkVrs)
        {
            //kmlkVrs should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmKmlkKmlkVrs,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm.kmlk.ohkTur != odemeEmriIstegi.odmBsltm.kmlk.ohkTur)
        {
            //ohkTur should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmKmlkOhkTur,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm.kmlk.ohkTur == OpenBankingConstants.OHKTur.Kurumsal 
            &&  odemeEmriRizasi.odmBsltm.kmlk.krmKmlkTur != odemeEmriIstegi.odmBsltm.kmlk.krmKmlkTur)
        {
            //krmKmlkTur should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmKmlkKrmKmlkTur,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm.kmlk.ohkTur == OpenBankingConstants.OHKTur.Kurumsal 
            &&  odemeEmriRizasi.odmBsltm.kmlk.krmKmlkVrs != odemeEmriIstegi.odmBsltm.kmlk.krmKmlkVrs)
        {
            //krmKmlkVrs should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmKmlkKrmKmlkVrs,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        //odmBsltma gon must be same
        if (odemeEmriRizasi.odmBsltm.gon.hspNo != odemeEmriIstegi.odmBsltm.gon.hspNo)
        {
            //hspNo should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmGonHspNo,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm.gon.unv != odemeEmriIstegi.odmBsltm.gon.unv)
        {
            //unv should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmGonUnv,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm.alc.hspNo != odemeEmriIstegi.odmBsltm.alc.hspNo)
        {
            //alc.hspNo should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcHspNo,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm.alc.kolas != null
            && odemeEmriRizasi.odmBsltm.alc.kolas.kolasTur != odemeEmriIstegi.odmBsltm.alc.kolas?.kolasTur)
        {
            //kolas.kolasTur should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcKolasKolasTur,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm.alc.kolas != null
            && odemeEmriRizasi.odmBsltm.alc.kolas.kolasDgr != odemeEmriIstegi.odmBsltm.alc.kolas?.kolasDgr)
        {
            //kolas.kolasDgr should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmAlcKolasKolasDgr,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm?.kkod != null
            && odemeEmriRizasi.odmBsltm.kkod.aksTur != odemeEmriIstegi.odmBsltm.kkod?.aksTur)
        {
            //kkod.aksTur should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmKkodAksTur,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm?.kkod != null
            && odemeEmriRizasi.odmBsltm.kkod.kkodRef != odemeEmriIstegi.odmBsltm.kkod?.kkodRef)
        {
            //kkod.kkodRef should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmKkodKkodRef,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm?.odmAyr.odmKynk != odemeEmriIstegi.odmBsltm.odmAyr.odmKynk)
        {
            //odmKynk should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmKynk,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm?.odmAyr.odmAmc != odemeEmriIstegi.odmBsltm.odmAyr.odmAmc)
        {
            //odmAmc should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmAmc,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm?.odmAyr.refBlg != odemeEmriIstegi.odmBsltm.odmAyr.refBlg)
        {
            //refBlg should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrRefBlg,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm?.odmAyr.odmStm != odemeEmriIstegi.odmBsltm.odmAyr.odmStm)
        {
            //odmStm should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmOdmAyrOdmStm,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (odemeEmriRizasi.odmBsltm?.islTtr.prBrm != odemeEmriIstegi.odmBsltm.islTtr.prBrm)
        {
            //prBrm should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmIslTtrPrBrm,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }
        if (NormalizeAmount(odemeEmriRizasi.odmBsltm?.islTtr.ttr) != NormalizeAmount(odemeEmriIstegi.odmBsltm.islTtr.ttr))
        {
            //ttr should be same
            AddFieldError_DefaultInvalidField(errorCodeDetails: errorCodeDetails, errorResponse,
                propertyName: OBErrorCodeConstants.FieldNames.OdmBsltmIslTtrTtr,
                errorCode: OBErrorCodeConstants.ErrorCodesEnum.InvalidFieldMissingOrInCorrect, objectName: objectName);
        }

        if (errorResponse.FieldErrors.Any())
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        return result;
    }


    /// <summary>
    /// Checks if parameters valid to get balances and accounts
    /// </summary>
    /// <returns></returns>
    public static ApiResult IsParametersValidToGetAccountsBalances(HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails,
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
    public static ApiResult IsParametersValidToGetTransactionsByHspRef(HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, Consent consent,
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


        if (!string.IsNullOrEmpty(minIslTtr) && !IsValidAmount(minIslTtr))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatMinIslTtr);
            return result;
        }

        if (!string.IsNullOrEmpty(mksIslTtr) && !IsValidAmount(mksIslTtr))
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
        if (!OBErrorResponseHelper.PrepareAndCheckHeaderInvalidFormatProperties(header, context, errorCodeDetails,
                out var errorResponse))
        {
            result.Result = false;
            result.Data = errorResponse;
            return result;
        }

        if (configuration["HHSCode"] != header.XASPSPCode)
        {
            //XASPSPCode value should be BurganBanks hhscode value
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidAspsp);
            return result;
        }

        if (ConstantHelper.GetPSUInitiatedValues().Contains(header.PSUInitiated) == false)
        {
            //Check psu initiated value
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidContentPsuInitiated);
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
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidTpp);
            return result;
        }

        if (isUserRequired.HasValue
            && isUserRequired.Value
            && string.IsNullOrEmpty(header.UserReference))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidContentUserReference);
            return result;
        }

        if (isConsentIdRequired.HasValue
            && isConsentIdRequired.Value
            && string.IsNullOrEmpty(header.ConsentId))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidContentConsentIdInHeader);
            return result;
        }

        result = await IsXJwsSignatureValid(context, configuration, yosInfoService, header, errorCodeDetails, body,
            isXJwsSignatureRequired);
        if (!result.Result)
        {
            return result;
        }

        result = await IsPsuFraudCheckValid(context, configuration, yosInfoService, header, errorCodeDetails);
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
                result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.InvalidAspsp);
                return result;
            }

            if (header.XTPPCode != katilimciBlg.yosKod)
            {
                //YOSCode must match with header x-tpp-code
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetBadRequestError(context, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.InvalidTpp);
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
        if (bool.TryParse(configuration["CheckXJWSSignature"], out bool isXJWSSignatureCheckEnabled) &&
            !isXJWSSignatureCheckEnabled)
        {
            //XJWSSignature check config is false
            //NO need to check
            return result;
        }

        if (isXJwsSignatureRequired.HasValue
            && isXJwsSignatureRequired.Value
            && string.IsNullOrEmpty(header.XJWSSignature))
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.MissingSignature);
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
        try
        {
            // Decode JWT to get header
            var jwtHeaderDecoded = JWT.Headers(headerXjwsSignature);
            // Verify algorithm used is RS256
            if (jwtHeaderDecoded["alg"].ToString() != "RS256")
            {
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureHeaderAlgorithmWrong);
                return result;
            }

            var jwtPayloadDecoded = JWT.Payload(headerXjwsSignature);
            var jwtPayloadJson = JsonDocument.Parse(jwtPayloadDecoded);
            if (jwtPayloadJson.RootElement.TryGetProperty("exp", out var expValue)) //Get exp data in payload
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
                            OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureHeaderExpireDatePassed);
                        return result;
                    }
                }
                else
                {
                    // Handle the case where "exp" property is not a valid JSON number
                    result.Result = false;
                    result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                        OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureExWrong);
                    return result;
                }
            }
            else
            {
                // Handle the case where "exp" property is missing
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureExMissing);
                return result;
            }
        }
        catch (Exception)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureXJwsSignatureHeaderInvalid);
            return result;
        }

        if (body is null)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetInternalServerError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InternalServerErrorBodyEmptyValidateJwt);
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
            if (getPublicKeyResult.Result == false
                || getPublicKeyResult.Data is null
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

            var verifyResult = VerifyJwt(headerXjwsSignature, publicKey);
            if (verifyResult.Result) //Verified
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
        catch (Exception)
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
        List<OBErrorCodeDetail> errorCodeDetails
    )
    {
        ApiResult result = new();

        //No need to check header property
        if (header.PSUInitiated == OpenBankingConstants.PSUInitiated.SystemStarted)
        {
            return result;
        }

        //Check CheckPSUFraudCheck config value 
        if (bool.TryParse(configuration["CheckPSUFraudCheck"], out bool isPSUFraudCheckEnabled) &&
            !isPSUFraudCheckEnabled)
        {
            //CheckPSUFraudCheck config is false. No need to check
            return result;
        }

        if (header.PSUInitiated == OpenBankingConstants.PSUInitiated.OHKStarted
            && string.IsNullOrEmpty(header.PSUFraudCheck)) //PSUFraudCheck is required when OHK started
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.MissingSignaturePSUFraudCheck);
            return result;
        }

        var headerPsuFraudCheck = header.PSUFraudCheck!;
        // "eyJhbGciOiJSUzI1NiJ9.eyJBbm9tYWx5RmxhZyI6IjAiLCJMYXN0UGFzc3dvcmRDaGFuZ2VGbGFnIjoiMSIsIkZpcnN0TG9naW5GbGFnIjoiMSIsIkRldmljZUZpcnN0TG9naW5GbGFnIjoiMSIsIkJsYWNrbGlzdEZsYWciOiIwIiwiTWFsd2FyZUZsYWciOiIwIiwiVW5zYWZlQWNjb3VudEZsYWciOiIwIiwiZXhwIjoxNjY1NDc1NTU2LCJpYXQiOjE2NjUzODkxNTYsImlzcyI6Imh0dHBzOi8vYXBpZ3cuYmttLmNvbS50ciJ9.DhUh_nsXDuNIrvsQ3KOhOXdVcJg6fTDVW8K1oea8kLtmb7n-_hJHY3mWX5zzobu-Vh2VvFzIxPhHtol6gLHFktmIMiQ9TDHb_mRZFXgJB4ToNfqc3Fy9mi5bS8By2IYi1HxDaCStstaZDaunzXfHCtqybfZXyk6teDrf-iIf6lqX9Keo7GZO-Y7mP7C13-c_QwyNKrZK4TZwUQbecRqXYn1DcEHM7kukQHTar_hKBWkXPmNpScY0J2rKksr4ejR1uLhdQm-Pdwoe9y6qrNEB79vMLBkRNtbuV0vc1GYHp_YKkzBKBI_58uuB2GD9877CsrcRnHMQb88xpxiPKh6-ew";
        string jwtPayloadDecoded;
        JsonDocument jwtPayloadJson;
        try
        {
            // Decode JWT to get header
            var jwtHeaderDecoded = JWT.Headers(headerPsuFraudCheck);
            // Verify algorithm used is RS256
            if (jwtHeaderDecoded["alg"].ToString() != "RS256")
            {
                result.Result = false;
                result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                    OBErrorCodeConstants.ErrorCodesEnum.InvalidSignatureHeaderAlgorithmWrongFraud);
                return result;
            }

            jwtPayloadDecoded = JWT.Payload(headerPsuFraudCheck);
            jwtPayloadJson = JsonDocument.Parse(jwtPayloadDecoded);
        }
        catch (Exception)
        {
            result.Result = false;
            result.Data = OBErrorResponseHelper.GetForbiddenError(context, errorCodeDetails,
                OBErrorCodeConstants.ErrorCodesEnum.InvalidSignaturePsuFraudCheckHeaderInvalid);
            return result;
        }

        result = ValidateExPropertyFraud(context, errorCodeDetails, jwtPayloadJson); //Validate ex property
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

    private static ApiResult ValidateExPropertyFraud(HttpContext context, List<OBErrorCodeDetail> errorCodeDetails,
        JsonDocument jwtPayloadJson)
    {
        ApiResult result = new();
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
    private static ApiResult ValidateRequiredZmnAralikProperty(JsonDocument jwtPayloadJson, string propertyName,
        HttpContext context, List<OBErrorCodeDetail> errorCodeDetails,
        OBErrorCodeConstants.ErrorCodesEnum missingErrorCode, OBErrorCodeConstants.ErrorCodesEnum invalidErrorCode)
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
    private static ApiResult ValidateZmnAralikProperty(JsonDocument jwtPayloadJson, string propertyName,
        HttpContext context, List<OBErrorCodeDetail> errorCodeDetails,
        OBErrorCodeConstants.ErrorCodesEnum invalidErrorCode)
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
    private static ApiResult ValidateVarYokProperty(JsonDocument jwtPayloadJson, string propertyName,
        HttpContext context, List<OBErrorCodeDetail> errorCodeDetails,
        OBErrorCodeConstants.ErrorCodesEnum invalidErrorCode)
    {
        ApiResult result = new();
        if (jwtPayloadJson.RootElement.TryGetProperty(propertyName, out var propertyValue))
        {
            if (!string.IsNullOrEmpty(propertyValue.ToString())
                && (!Int32.TryParse(propertyValue.ToString(), out var propInt)
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
            if (getPublicKeyResult.Result == false
                || getPublicKeyResult.Data is null
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
            var verifyResult = VerifyJwt(headerPsuFraudCheck, publicKey);
            if (verifyResult.Result) //Verified
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

    /// <summary>
    /// Checks amount data pattern
    /// </summary>
    /// <param name="ttr">MksIslTtr data</param>
    /// <returns>Is amount pattern is valid </returns>
    public static bool IsValidAmount(string ttr)
    {
        // Define the regular expression pattern
        string pattern = OpenBankingConstants.RegexPatterns.amount;

        // Check if the input matches the pattern
        return Regex.IsMatch(ttr, pattern, RegexOptions.NonBacktracking);
    }
    
    private static string? NormalizeAmount(string? amount)
    {
        if (decimal.TryParse(amount, out decimal decimalAmount))
        {
            // Normalize by trimming trailing zeros and any redundant decimal point
            return decimalAmount.ToString("G29"); // G29 format removes trailing zeros and the decimal point if unnecessary
        }

        return amount;
    }
}