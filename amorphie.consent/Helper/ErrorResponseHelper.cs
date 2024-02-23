using amorphie.consent.core.DTO;

namespace amorphie.consent.Helper;

public static class ErrorResponseHelper
{
    public static CustomErrorResponse BuildErrorResponse(string path, int httpCode, string httpMessage, string moreInformation, string moreInformationTr, List<FieldError> fieldErrors = null, string errorCode = null)
    {
        return new CustomErrorResponse
        {
            Path = path,
            Id = Guid.NewGuid().ToString(),
            Timestamp = DateTime.Now,
            HttpCode = httpCode,
            HttpMessage = httpMessage,
            MoreInformation = moreInformation,
            MoreInformationTr = moreInformationTr,
            FieldErrors = fieldErrors,
            ErrorCode = errorCode
        };
    }
}