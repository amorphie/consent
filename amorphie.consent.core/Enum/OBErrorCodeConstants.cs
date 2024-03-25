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
    }

    public static class HttpMessage
    {
        public const string BadRequest = "Bad Request";
        public const string NotFound = "Not Found";
        public const string InternalServerError = "Internal Server Error";
    }

    public enum ErrorCodesEnum
    {
        FieldCanNotBeNullWithName = 1,
        FieldCanNotBeNull = 2,

        InvalidFormatValidationError = 100,
        InvalidAspsp= 101,
        InvalidTpp = 102,
        InvalidTppRole = 103,
        NotFound = 150,
        InternalServerError = 151,
        ConsentMismatch = 160,
        ConsentMismatchStateNotValidToDelete = 161,
        InvalidContentPsuInitiated = 200,
        InvalidContentUserReference = 201
       
    }
}