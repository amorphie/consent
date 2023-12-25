using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.Contract.ContractInstance;
using amorphie.consent.core.DTO.Contract.DocumentInstance;
using amorphie.consent.core.DTO.Contract.TemplateRender;

namespace amorphie.consent.Service.Interface;

public interface IContractService
{
    /// <summary>
    /// Call contractInstance method of contract service and process the response
    /// It gets the not authorized documents information of user
    /// </summary>
    /// <param name="instanceRequest">Object to be send</param>
    /// <returns>contractInstance post process result. Users unauthorized documents informations.</returns>
    Task<ApiResult> ContractInstance(InstanceRequestDto instanceRequest);
    
    /// <summary>
    /// Call templaterender post method of contract service and process the response
    /// It gets the document in base64 
    /// </summary>
    /// <param name="templateRenderRequest"></param>
    /// <returns>Template render post message response. File data in base64</returns>
    Task<ApiResult> TemplateRender(TemplateRenderRequestDto templateRenderRequest);
    
    /// <summary>
    /// Call documentinstance method of contract service and process the response
    /// It sends the authorized document to service
    /// </summary>
    /// <param name="instanceRequest">Object to be send</param>
    /// <returns>documentInstance post process result. Post response of authorized document</returns>
    Task<ApiResult> DocumentInstance(DocumentInstanceRequestDto instanceRequest);
}