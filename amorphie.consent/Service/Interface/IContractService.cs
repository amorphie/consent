using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.Contract;

namespace amorphie.consent.Service.Interface;

public interface IContractService
{
    /// <summary>
    /// Call contractInstance method of contract service and process the response
    /// </summary>
    /// <param name="instanceRequest">Object to be send</param>
    /// <returns>contractInstance post process result</returns>
    Task<ApiResult> ContractInstance(InstanceRequestDto instanceRequest);
}