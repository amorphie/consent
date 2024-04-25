using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

/// <summary>
/// Used to list account consents
/// </summary>
public class ListAccountConsentDto
{
    public DateTime CreatedAt { get; set; }
    public OBYosInfoDto YosInfo { get; set; }
    public Guid ConsentId { get; set; }
    public PermissionInformationDto PermissionInformation { get; set; }
    /// <summary>
    /// Authorized Accounts' reference number and names
    /// </summary>
    public List<AccountRefDetailDto> AccountReferences { get; set; }
}


