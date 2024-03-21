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

    public static OBErrorCodeDetail BuildDefaultErrorCodeDetail(int internalCode)
    {
        return new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            Message = $"{internalCode} internal code information is not in system.",
            MessageTr = $"{internalCode} kodlu hata detay bilgisi sistemde bulunamadı.",
            BkmCode = "TR.OHVPS.Server.InternalError",
        };
    }

    public static OBErrorCodeDetail BuildDefaultFieldErrorCodeDetail(int internalCode)
    {
        return new OBErrorCodeDetail
        {
            Id = Guid.NewGuid(),
            Message = $"{internalCode} internal code information is not in system.",
            MessageTr = $"{internalCode} kodlu hata detay bilgisi sistemde bulunamadı.",
            BkmCode = "TR.OHVPS.Field.Invalid",
        };
    }

    /// <summary>
    /// Generates 400 OBCustomErrorResponseDto with given error code
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="errorCodeDetails">All error code constant data</param>
    /// <param name="errorCode">To be created error internal code</param>
    /// <returns>OBCustomErrorResponseDto type of object</returns>
    public static OBCustomErrorResponseDto GetInvalidCodeError(HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum errorCode)
    {
        var validationErrorCode = errorCodeDetails.FirstOrDefault(e =>
                                      e.InternalCode == errorCode.GetHashCode()) ??
                                  BuildDefaultErrorCodeDetail(OBErrorCodeConstants.HttpMessage
                                      .BadRequest.GetHashCode());
        return BuildErrorResponse(HttpStatusCode.BadRequest,
            OBErrorCodeConstants.HttpMessage.BadRequest, context.Request.Path, validationErrorCode);
    }

    /// <summary>
    /// Generates 400 OBCustomErrorResponseDto with given error code
    /// </summary>
    /// <param name="context">HttpContext</param>
    /// <param name="errorCodeDetails">All error code constant data</param>
    /// <param name="errorCode">To be created error internal code</param>
    /// <returns>OBCustomErrorResponseDto type of object</returns>
    public static OBCustomErrorResponseDto GetInvalidContentError(HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, OBErrorCodeConstants.ErrorCodesEnum errorCode)
    {
        var validationErrorCode = errorCodeDetails.FirstOrDefault(e =>
                                      e.InternalCode == errorCode.GetHashCode()) ??
                                  BuildDefaultErrorCodeDetail(OBErrorCodeConstants.HttpMessage
                                      .BadRequest.GetHashCode());

        return BuildErrorResponse(HttpStatusCode.BadRequest,
            OBErrorCodeConstants.HttpMessage.BadRequest, context.Request.Path, validationErrorCode);
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
        var validationErrorCode = errorCodeDetails.FirstOrDefault(e =>
                                      e.InternalCode == errorCode.GetHashCode()) ??
                                  BuildDefaultErrorCodeDetail(OBErrorCodeConstants.HttpMessage
                                      .NotFound.GetHashCode());

        return BuildErrorResponse(HttpStatusCode.NotFound,
            OBErrorCodeConstants.HttpMessage.NotFound, context.Request.Path, validationErrorCode);
    }


    public static bool PrepareAndCheckHeaderInvalidFormatProperties(RequestHeaderDto header, HttpContext context,
        List<OBErrorCodeDetail> errorCodeDetails, out OBCustomErrorResponseDto errorResponse)
    {
        var validationErrorCode = errorCodeDetails.FirstOrDefault(e =>
                                      e.InternalCode == OBErrorCodeConstants.ErrorCodesEnum.InvalidFormatValidationError
                                          .GetHashCode()) ??
                                  BuildDefaultErrorCodeDetail(OBErrorCodeConstants.HttpMessage
                                      .BadRequest.GetHashCode());

        errorResponse = BuildErrorResponse(HttpStatusCode.BadRequest,
            OBErrorCodeConstants.HttpMessage.BadRequest, context.Request.Path, validationErrorCode);
        errorResponse.FieldErrors = new List<FieldError>();

        var fieldErrorCode = errorCodeDetails.FirstOrDefault(e =>
                                 e.InternalCode ==
                                 OBErrorCodeConstants.ErrorCodesEnum.FieldCanNotBeNull.GetHashCode()) ??
                             BuildDefaultFieldErrorCodeDetail(OBErrorCodeConstants.ErrorCodesEnum
                                 .FieldCanNotBeNull
                                 .GetHashCode());

        // Check each header property and add errors if necessary
        CheckHeaderInvalidFormatProperty(header.PSUInitiated, OBErrorCodeConstants.FieldNames.HeaderPsuInitiated,
            fieldErrorCode, errorResponse);
        CheckHeaderInvalidFormatProperty(header.XGroupID, OBErrorCodeConstants.FieldNames.HeaderXGroupId,
            fieldErrorCode, errorResponse);
        CheckHeaderInvalidFormatProperty(header.XASPSPCode, OBErrorCodeConstants.FieldNames.HeaderXaspspCode,
            fieldErrorCode, errorResponse);
        CheckHeaderInvalidFormatProperty(header.XRequestID, OBErrorCodeConstants.FieldNames.HeaderXRequestId,
            fieldErrorCode, errorResponse);
        CheckHeaderInvalidFormatProperty(header.XTPPCode, OBErrorCodeConstants.FieldNames.HeaderXtppCode,
            fieldErrorCode, errorResponse);

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