using System.Text.Json.Nodes;
using amorphie.consent.core.DTO.Contract;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using Refit;

namespace amorphie.consent.Service;

public interface IContractClientService
{
    [Post("/contract/Instance")]
    Task ContractInstance([Body] InstanceRequestDto instanceRequest);
}