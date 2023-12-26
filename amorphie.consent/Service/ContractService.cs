using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.Contract.ContractInstance;
using amorphie.consent.core.DTO.Contract.DocumentInstance;
using amorphie.consent.core.DTO.Contract.TemplateRender;
using amorphie.consent.Service.Interface;
using amorphie.consent.Service.Refit;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace amorphie.consent.Service;

public class ContractService : IContractService
{
    private readonly IContractClientService _contractClientService;
    private readonly IMapper _mapper;


    public ContractService(IContractClientService contractClientService,
    IMapper mapper)
    {
        _contractClientService = contractClientService;
        _mapper = mapper;
    }

    public async Task<ApiResult> ContractInstance(InstanceRequestDto instanceRequest)
    {
        ApiResult result = new();
        try
        {
            //Send contractrequest to servie
            result.Data = await _contractClientService.ContractInstance(instanceRequest);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }

    public async Task<ApiResult> TemplateRender(TemplateRenderRequestDto templateRenderRequest)
    {
        ApiResult result = new();
        try
        {
            //Get file from service
            result.Data = await _contractClientService.TemplateRender(templateRenderRequest);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }

    public async Task<ApiResult> DocumentInstance(DocumentInstanceRequestDto instanceRequest)
    {
        ApiResult result = new();
        try
        {
            //Send contractrequest to servie
            await _contractClientService.DocumentInstance(instanceRequest);
        }
        catch (Exception e)
        {
            result.Result = false;
            result.Message = e.Message;
        }
        return result;
    }
}