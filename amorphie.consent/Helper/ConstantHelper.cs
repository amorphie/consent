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
    /// Get account consent status list that can be marked as active statuses.
    /// In those statuses if new consent comes, it should be handled
    /// </summary>
    /// <returns>Active Consent Statuses</returns>
    public static List<string> GetActiveAccountConsentStatusList()
    {
        return new List<string>()
        {
            OpenBankingConstants.RizaDurumu.YetkiBekleniyor,
            OpenBankingConstants.RizaDurumu.Yetkilendirildi,
            OpenBankingConstants.RizaDurumu.YetkiKullanildi
        };
    }

    /// <summary>
    /// Get psu initiated values as a string list
    /// </summary>
    /// <returns>psu initiated values as a string list</returns>
    public static List<string> GetPSUInitiatedValues()
    {
        return new List<string>()
        {
            OpenBankingConstants.PSUInitiated.OHKStarted,
            OpenBankingConstants.PSUInitiated.SystemStarted
        };
    }
}