using System.Net;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.Enum;
using amorphie.consent.core.Model;

namespace amorphie.consent.Helper;

public static class OBErrorResponseHelper
{
    public static OBCustomErrorResponseDto BuildErrorResponse(HttpStatusCode httpCode, string httpMessage, string path,
        OBErrorCodeDetail errorCodeDetail, List<FieldError>? fieldErrors = null)
    {
        return new OBCustomErrorResponseDto
        {
            Path = path,
            Id = Guid.NewGuid().ToString(),
            Timestamp = DateTime.Now,
            HttpCode = (int)httpCode,
            HttpMessage = httpMessage,
            MoreInformation = errorCodeDetail.Message,
            MoreInformationTr = errorCodeDetail.MessageTr,
            ErrorCode = errorCodeDetail.BkmCode,
            FieldErrors = fieldErrors,
        };
    }
    
    

    /// <summary>
    /// Generates 400 OBCustomErrorResponseDto with given error code
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="errorCodeDetails">All error code constant data</param>
    /// <param name="errorCode">To be created error internal code</param>
    /// <returns>OBCustomErrorResponseDto type of object</returns>
    public static OBCustomErrorResponseDto GetBadRequestError(HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum errorCode)
    {
        //Get errorCode detail
        var errorCodeDetail = GetErrorCodeDetail_DefaultInternalServer(errorCodeDetails,errorCode);
        //Generate customerrorresponse of badrequest
        return BuildErrorResponse(HttpStatusCode.BadRequest,
            OBErrorCodeConstants.HttpMessage.BadRequest, context.Request.Path, errorCodeDetail);
    }
    
    /// <summary>
    /// Generates 500 OBCustomErrorResponseDto with given error code
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="errorCodeDetails">All error code constant data</param>
    /// <param name="errorCode">To be created error internal code</param>
    /// <returns>OBCustomErrorResponseDto type of object</returns>
    public static OBCustomErrorResponseDto GetInternalServerError(HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum errorCode)
    {
        //Get errorCode detail
        var errorCodeDetail =GetErrorCodeDetail_DefaultInternalServer(errorCodeDetails,errorCode);
        //Generate customerrorresponse of InternalServerError
        return BuildErrorResponse(HttpStatusCode.InternalServerError,
            OBErrorCodeConstants.HttpMessage.InternalServerError, context.Request.Path, errorCodeDetail);
    }
    
    /// <summary>
    /// Generates 404 OBCustomErrorResponseDto with given error code
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="errorCodeDetails">All error code constant data</param>
    /// <param name="errorCode">To be created error internal code</param>
    /// <returns>OBCustomErrorResponseDto type of object</returns>
    public static OBCustomErrorResponseDto GetNotFoundError(HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum errorCode)
    {
        //Get errorCode detail
        var errorCodeDetail =GetErrorCodeDetail_DefaultInternalServer(errorCodeDetails,errorCode);
        //Generate customerrorresponse of NotFound
        return BuildErrorResponse(HttpStatusCode.NotFound,
            OBErrorCodeConstants.HttpMessage.NotFound, context.Request.Path, errorCodeDetail);
    }
    
    /// <summary>
    /// Gets errorCodeDetail in errorCodeDetail list by internalCode.
    /// If can not be found, default  TR.OHVPS.Field.Invalid bkm code error code detail object created
    /// </summary>
    /// <param name="errorCodeDetails"></param>
    /// <param name="internalCode"></param>
    /// <returns>errorCodeDetail object of given internalCode</returns>
    private static OBErrorCodeDetail GetErrorCodeDetail_DefaultInvalidField(List<OBErrorCodeDetail> errorCodeDetails,OBErrorCodeConstants.ErrorCodesEnum internalCode)
    {
        var errorCodeDetail = errorCodeDetails.FirstOrDefault(e =>
                                  e.InternalCode ==
                                  internalCode.GetHashCode()) ??
                              BuildDefaultErrorCodeDetail_InvalidField(internalCode.GetHashCode());
        return errorCodeDetail;
    }
    
    /// <summary>
    /// Gets errorCodeDetail in errorCodeDetail list by internalCode.
    /// If can not be found, default  TR.OHVPS.Server.InternalError bkm code error code detail object created
    /// </summary>
    /// <param name="errorCodeDetails"></param>
    /// <param name="internalCode"></param>
    /// <returns>errorCodeDetail object of given internalCode</returns>
    private static OBErrorCodeDetail GetErrorCodeDetail_DefaultInternalServer(List<OBErrorCodeDetail> errorCodeDetails,OBErrorCodeConstants.ErrorCodesEnum internalCode)
    {
        var errorCodeDetail = errorCodeDetails.FirstOrDefault(e =>
                                  e.InternalCode ==
                                  internalCode.GetHashCode()) ??
                              BuildDefaultErrorCodeDetail_InternalServer(internalCode.GetHashCode());
        return errorCodeDetail;
    }
    
    private static OBErrorCodeDetail BuildDefaultErrorCodeDetail_InternalServer(int internalCode)
    {
        return new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            Message = $"{internalCode} internal code information is not in system.",
            MessageTr = $"{internalCode} kodlu hata detay bilgisi sistemde bulunamadı.",
            BkmCode = "TR.OHVPS.Server.InternalError",
        };
    }

    private static OBErrorCodeDetail BuildDefaultErrorCodeDetail_InvalidField(int internalCode)
    {
        return new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            Message = $"{internalCode} internal code information is not in system.",
            MessageTr = $"{internalCode} kodlu hata detay bilgisi sistemde bulunamadı.",
            BkmCode = "TR.OHVPS.Field.Invalid",
        };
    }


    public static bool PrepareAndCheckHeaderInvalidFormatProperties(RequestHeaderDto header, HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, out OBCustomErrorResponseDto errorResponse)
    {
        //Get 400 error response
        errorResponse = GetBadRequestError(context, errorCodeDetails,OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError);
        errorResponse.FieldErrors = new List<FieldError>();

        //Field can not be empty error code
        var errorCodeDetail = GetErrorCodeDetail_DefaultInvalidField(errorCodeDetails,OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull);

        // Check each header property and add errors if necessary
        CheckHeaderInvalidFormatProperty(header.PSUInitiated, OBErrorCodeConstants.FieldNames.HeaderPsuInitiated,
            errorCodeDetail, errorResponse);
        CheckHeaderInvalidFormatProperty(header.XGroupID, OBErrorCodeConstants.FieldNames.HeaderXGroupId,
            errorCodeDetail, errorResponse);
        CheckHeaderInvalidFormatProperty(header.XASPSPCode, OBErrorCodeConstants.FieldNames.HeaderXaspspCode,
            errorCodeDetail, errorResponse);
        CheckHeaderInvalidFormatProperty(header.XRequestID, OBErrorCodeConstants.FieldNames.HeaderXRequestId,
            errorCodeDetail, errorResponse);
        CheckHeaderInvalidFormatProperty(header.XTPPCode, OBErrorCodeConstants.FieldNames.HeaderXtppCode,
            errorCodeDetail, errorResponse);

        // Return false if any errors were added, indicating an issue with the header
        return !errorResponse.FieldErrors.Any();
    }

    
    public static void CheckHeaderInvalidFormatProperty(string propertyValue, string propertyName,
        OBErrorCodeDetail errorCodeDetail, OBCustomErrorResponseDto errorResponse)
    {
        if (string.IsNullOrEmpty(propertyValue))
        {
            errorResponse.FieldErrors?.Add(new FieldError
            {
                Field = propertyName,
                Message = errorCodeDetail.Message,
                MessageTr = errorCodeDetail.MessageTr,
                Code = errorCodeDetail.BkmCode
            });
        }
    }
    
    
  
}