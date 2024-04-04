using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.Event;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Enum;
using amorphie.consent.Helper;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace amorphie.consent.Service;

public class BKMService : IBKMService
{
    private readonly IBKMClientService _bkmClientService;
    private readonly IMapper _mapper;
    private readonly IConfiguration _configuration;


    public BKMService(IBKMClientService bkmClientService,
    IMapper mapper,
    IConfiguration configuration)
    {
        _bkmClientService = bkmClientService;
        _mapper = mapper;
        _configuration = configuration;
    }

    public async Task<ApiResult> GetHhs(string hhsKod)
    {
        ApiResult apiResult = new();
        try
        {
            ApiResult tokenServiceResponse = await GetToken(OpenBankingConstants.BKMServiceScope.HhsRead);
            if (!tokenServiceResponse.Result)
                return tokenServiceResponse;

            string authorizationValue = $"Bearer {tokenServiceResponse.Data}";

            var httpResponse = await _bkmClientService.GetHhs(authorizationValue, hhsKod);

            if (!httpResponse.IsSuccessStatusCode)
            {
                apiResult.Result = false;
                apiResult.Message = await httpResponse.Content.ReadAsStringAsync();
                return apiResult;
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var hhsResponse = JsonConvert.DeserializeObject<OBHhsInfoDto>(content);
            apiResult.Data = hhsResponse;
            apiResult.Result = true;
        }
        catch (Exception e)
        {
            apiResult.Result = false;
            apiResult.Message = e.Message;
        }
        return apiResult;
    }
    public async Task<ApiResult> GetYos(string yosKod)
    {
        ApiResult apiResult = new();
        try
        {
            ApiResult tokenServiceResponse = await GetToken(OpenBankingConstants.BKMServiceScope.YosRead);
            if (!tokenServiceResponse.Result)
                return tokenServiceResponse;

            string authorizationValue = $"Bearer {tokenServiceResponse.Data}";

            var httpResponse = await _bkmClientService.GetYos(authorizationValue, yosKod);

            if (!httpResponse.IsSuccessStatusCode)
            {
                apiResult.Result = false;
                apiResult.Message = await httpResponse.Content.ReadAsStringAsync();
                return apiResult;
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var hhsResponse = JsonConvert.DeserializeObject<OBYosInfoDto>(content);
            apiResult.Data = hhsResponse;
            apiResult.Result = true;
        }
        catch (Exception e)
        {
            apiResult.Result = false;
            apiResult.Message = e.Message;
        }
        return apiResult;
    }
    public async Task<ApiResult> GetAllHhs()
    {
        ApiResult apiResult = new();
        try
        {
            ApiResult tokenServiceResponse = await GetToken(OpenBankingConstants.BKMServiceScope.HhsRead);
            if (!tokenServiceResponse.Result)
                return tokenServiceResponse;

            string authorizationValue = $"Bearer {tokenServiceResponse.Data}";

            var httpResponse = await _bkmClientService.GetAllHhs(authorizationValue);

            if (!httpResponse.IsSuccessStatusCode)
            {
                apiResult.Result = false;
                apiResult.Message = await httpResponse.Content.ReadAsStringAsync();
                return apiResult;
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var hhsResponse = JsonConvert.DeserializeObject<List<OBHhsInfoDto>>(content);
            apiResult.Data = hhsResponse;
            apiResult.Result = true;
        }
        catch (Exception e)
        {
            apiResult.Result = false;
            apiResult.Message = e.Message;
        }
        return apiResult;
    }

    public async Task<ApiResult> GetAllYos()
    {
        ApiResult apiResult = new();
        try
        {
            ApiResult tokenServiceResponse = await GetToken(OpenBankingConstants.BKMServiceScope.YosRead);
            if (!tokenServiceResponse.Result)
                return tokenServiceResponse;

            string authorizationValue = $"Bearer {tokenServiceResponse.Data}";

            var httpResponse = await _bkmClientService.GetAllYos(authorizationValue);

            if (!httpResponse.IsSuccessStatusCode)
            {
                apiResult.Result = false;
                apiResult.Message = await httpResponse.Content.ReadAsStringAsync();
                return apiResult;
            }

            var content = await httpResponse.Content.ReadAsStringAsync();
            var hhsResponse = JsonConvert.DeserializeObject<List<OBYosInfoDto>>(content);
            apiResult.Data = hhsResponse;
            apiResult.Result = true;
        }
        catch (Exception e)
        {
            apiResult.Result = false;
            apiResult.Message = e.Message;
        }
        return apiResult;
    }

    public async Task<ApiResult> GetToken(string bkmServiceScope)
    {
        ApiResult result = new();
        try
        {
            //Get token request object
            var bkmTokenRequest = GetTokenRequestDto(bkmServiceScope);
            //Get token from bkm service
            var httpResponse = await _bkmClientService.GetToken(bkmTokenRequest);
            if (httpResponse.IsSuccessStatusCode)
            {//Get accesstoken
                var content = await httpResponse.Content.ReadAsStringAsync();
                var tokenResponse = JsonConvert.DeserializeObject<BKMTokenResponseDto>(content);
                if (tokenResponse == null || string.IsNullOrEmpty(tokenResponse.AccessToken))
                {
                    result.Result = false;
                    result.Message = "Token response is empty";
                    return result;
                }
                //Set token to response data
                result.Data = tokenResponse.AccessToken;
            }
            else
            {//Error in service
                result.Result = false;
                result.Message = await httpResponse.Content.ReadAsStringAsync();
                return result;
            }
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }


    public async Task<ApiResult> SendEventToYos(OlayIstegiDto olayIstegi)
    {
        ApiResult result = new();
        try
        {
            //Get token to use in olay-dinleme 
            ApiResult tokenServiceResponse = await GetToken(OpenBankingConstants.BKMServiceScope.OlayDinleme);
            if (!tokenServiceResponse.Result)//Error in service
                return tokenServiceResponse;
            string authorizationValue = $"Bearer {tokenServiceResponse.Data}";
            var xjwsHeader = OBModuleHelper.GetXJwsSignature(olayIstegi, _configuration);
            //Send event to YOS
            var httpResponse = await _bkmClientService.SendEventToYos(authorizationValue,
                Guid.NewGuid().ToString(),
                olayIstegi.katilimciBlg.hhsKod,
                olayIstegi.katilimciBlg.yosKod,
                xjwsHeader,
                olayIstegi);
            if (!httpResponse.IsSuccessStatusCode)
            {
                result.Result = false;
                result.Message = await httpResponse.Content.ReadAsStringAsync();
            }
            result.Data = httpResponse.StatusCode.GetHashCode();
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }


    /// <summary>
    /// Generates BKMTokenRequest Object by service scope
    /// </summary>
    /// <param name="bkmServiceScope">Service scope</param>
    /// <returns>BKM Token Request Dto object</returns>
    private BKMTokenRequestDto GetTokenRequestDto(string bkmServiceScope)
    {
        string clientId, clientSecret;//Set from config by service scope
        switch (bkmServiceScope)
        {
            case OpenBankingConstants.BKMServiceScope.OlayDinleme:
            case OpenBankingConstants.BKMServiceScope.YosRead:
                clientId = _configuration["BKM:HHSClientId"];
                clientSecret = _configuration["BKM:HHSClientSecret"];
                break;
            case OpenBankingConstants.BKMServiceScope.HhsRead:
                clientId = _configuration["BKM:YOSClientId"];
                clientSecret = _configuration["BKM:YOSClientSecret"];
                break;
            default:
                clientId = _configuration["BKM:HHSClientId"];
                clientSecret = _configuration["BKM:HHSClientSecret"];
                break;
        }
        //Generate object
        return new BKMTokenRequestDto
        {
            ClientId = clientId,
            ClientSecret = clientSecret,
            GrantType = "client_credentials",
            Scope = bkmServiceScope
        };
    }
}