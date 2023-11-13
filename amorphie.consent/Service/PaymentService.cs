using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace amorphie.consent.Service;

public class PaymentService : IPaymentService
{
    private readonly IPaymentClientService _paymentClientService;
    private readonly IMapper _mapper;
    

    public PaymentService(IPaymentClientService paymentClientService,
    IMapper mapper)
    {
        _paymentClientService = paymentClientService;
        _mapper = mapper;
    }

    public async Task<ApiResult> SendOdemeEmriRizasi(OdemeEmriRizaIstegiHHSDto odemeEmriRizaIstegi)
    {
        ApiResult result = new();
        try
        {
            //Send odemeemririzasi to servie
            OdemeEmriRizasiServiceResponseDto serviceResponse = await _paymentClientService.SendOdemeEmriRizasi(odemeEmriRizaIstegi);
            if (string.IsNullOrEmpty(serviceResponse.error))//Success
            {
                OdemeEmriRizasiHHSDto odemeEmriRizasi = new OdemeEmriRizasiHHSDto()
                {
                    isyOdmBlg = serviceResponse.isyOdmBlg,
                    rzBlg = serviceResponse.rzBlg,
                    gkd =  _mapper.Map<GkdDto>(serviceResponse.gkd),
                    katilimciBlg = serviceResponse.katilimciBlg,
                    odmBsltm = serviceResponse.odmBsltm
                };
                result.Data = odemeEmriRizasi;
            }
            else
            {//Error in service
                result.Result = false;
                result.Message = serviceResponse.error;
            }
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }

    public async Task<ApiResult> SendOdemeEmri(OdemeEmriIstegiHHSDto odemeEmriIstegi)
    {
        ApiResult result = new();
        try
        {
            //Send odemeemri to servie
            result.Data = await _paymentClientService.SendOdemeEmri(odemeEmriIstegi);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }
}