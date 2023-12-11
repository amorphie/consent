using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking.HHS;

namespace amorphie.consent.Service.Interface;

public interface IYosInfoService
{

    /// <summary>
    /// Checks if any yos in the system with identified yosCode
    /// </summary>
    /// <param name="yosCode">To be checked yosCode</param>
    /// <returns>Any yos in the system with given yosCode</returns>
    public Task<ApiResult> IsYosInApplication(string yosCode);

}