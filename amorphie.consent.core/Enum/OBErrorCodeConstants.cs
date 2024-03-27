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
    }
    public static class ObjectNames
    {
        public const string HesapBilgisiRizasiIstegi = "HesapBilgisiRizasiIstegi";
        public const string KatilimciBlg = "katilimciBlg";
        public const string Gkd = "gkd";
        public const string Kmlk = "kmlk";
        public const string HspBlg = "hspBlg";
        public const string HspBlgIznBlg = "hspBlg.iznBlg";
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
        InvalidFieldHhsCodeYosCodeLength = 3,
        
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