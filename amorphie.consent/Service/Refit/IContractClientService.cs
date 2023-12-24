using amorphie.consent.core.DTO.Contract.ContractInstance;
using amorphie.consent.core.DTO.Contract.DocumentInstance;
using amorphie.consent.core.DTO.Contract.TemplateRender;
using Refit;

namespace amorphie.consent.Service.Refit;

public interface IContractClientService
{
    [Post("/contract/Instance")]
    Task<InstanceResponseDto> ContractInstance([Body] InstanceRequestDto instanceRequest);
    
    [Post("/template-render/render/pdf")]
    Task<byte[]> TemplateRender([Body] TemplateRenderRequestDto templateRenderRequest);
    
    [Post("/document/instance")]
    Task DocumentInstance([Body] DocumentInstanceRequestDto documentInstanceRequest);
}