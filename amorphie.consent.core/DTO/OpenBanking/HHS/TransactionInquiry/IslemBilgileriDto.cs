﻿namespace amorphie.consent.core.DTO.OpenBanking.HHS;
public class IslemBilgileriDto
{
    public string hspRef { get; set; } = string.Empty;
    public List<IslemDto>? isller { get; set; }
}
