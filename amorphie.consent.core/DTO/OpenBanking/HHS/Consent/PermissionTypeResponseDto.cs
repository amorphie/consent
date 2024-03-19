using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

public class PermissionTypeResponseDto
{
    public string GroupName { get; set; } = string.Empty;
    public int GroupId { get; set; }
    public List<string> PermissionNames { get; set; } = new();
}