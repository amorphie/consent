using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

/// <summary>
/// Permission detail of consent
/// </summary>
public class PermissionInformationDto
{
    /// <summary>
    /// Permissions in consent is set.
    /// Turkish description is set for now.
    /// </summary>
    public List<PermissionTypeResponseDto> PermissionTypes { get; set; } = new();
    public DateTime LastValidAccessDate { get; set; }
    public DateTime? TransactionInquiryStartTime { get; set; }
    public DateTime? TransactionInquiryEndTime { get; set; }
}