using amorphie.consent.core.DTO;
using amorphie.consent.core.DTO.OpenBanking;
using amorphie.consent.core.DTO.OpenBanking.Event;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Model;
using Elastic.CommonSchema;

namespace amorphie.consent.Service.Interface;

public interface IOBErrorCodeDetailService
{
  
    public Task<List<OBErrorCodeDetail>> GetErrorCodeDetailsAsync();
    
}