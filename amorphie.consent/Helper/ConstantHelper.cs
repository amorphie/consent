using amorphie.consent.core.Enum;

namespace amorphie.consent.Helper;

public static class ConstantHelper
{

    /// <summary>
    /// Gets account consent status list that consent can be marked as deleted
    /// </summary>
    /// <returns>Deletable account consent status list</returns>
    public static List<string> GetAccountConsentCanBeDeleteStatusList()
    {
        return new List<string>() { OpenBankingConstants.RizaDurumu.Yetkilendirildi,
            OpenBankingConstants.RizaDurumu.YetkiBekleniyor,
            OpenBankingConstants.RizaDurumu.YetkiKullanildi
        };
    }

    /// <summary>
    /// Gets account consent status name list that consent can be marked as deleted
    /// </summary>
    /// <returns>Deletable account consent status name list</returns>
    public static List<string> GetAccountConsentCanBeDeleteStatusNameList()
    {
        return new List<string>() { OpenBankingConstants.RizaDurumuString.Yetkilendirildi,
            OpenBankingConstants.RizaDurumuString.YetkiBekleniyor,
            OpenBankingConstants.RizaDurumuString.YetkiKullanildi
        };
    }
}