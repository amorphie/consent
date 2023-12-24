using System.Text.Json.Nodes;
using amorphie.consent.core.DTO.Contract;
using amorphie.consent.core.DTO.Contract.TemplateRender;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using Refit;

namespace amorphie.consent.Service.Refit;

public interface IContractClientService
{
    [Post("/contract/Instance")]
    Task<InstanceResponseDto> ContractInstance([Body] InstanceRequestDto instanceRequest);
    
    [Post("/template-render/render/pdf")]
    Task<byte[]> TemplateRender([Body] TemplateRenderRequestDto templateRenderRequest);
}