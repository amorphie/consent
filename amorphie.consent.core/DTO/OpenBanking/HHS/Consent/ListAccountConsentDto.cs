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
    public OBYosInfoDto YosInfo { get; set; }
    public Guid ConsentId { get; set; }
    public PermissionInformationDto PermissionDetail { get; set; }
    /// <summary>
    /// Authorized Accounts' reference numbers
    /// </summary>
    public List<string> AccountReferences { get; set; }
}

/// <summary>
/// Account detail by hsapRef.
/// Contains account information and balance information if balance permission is given.
/// </summary>
public class AccountDetailDto
{
    /// <summary>
    /// Account service response by hsapref
    /// </summary>
    public HesapBilgileriDto AccountInformation { get; set; }
    /// <summary>
    /// Balance service response by hsapRef.
    /// If no balance permission, this object will be null
    /// </summary>
    public BakiyeBilgileriDto? BalanceInformation { get; set; }
}
