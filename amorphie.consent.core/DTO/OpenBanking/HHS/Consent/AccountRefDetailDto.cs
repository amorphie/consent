using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using amorphie.core.Base;

namespace amorphie.consent.core.DTO.OpenBanking.HHS;

/// <summary>
/// Used to list account consents
/// </summary>
public class AccountRefDetailDto
{
  public string AccountReference { get; set; } = string.Empty;
  public string? AccountName { get; set; } = String.Empty;
}


