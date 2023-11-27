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
    /// Get authorized account consent status list 
    /// </summary>
    /// <returns>Authorized account consent status list</returns>
    public static List<string> GetAuthorizedConsentStatusListForAccount()
    {
        return new List<string>()
        {
            OpenBankingConstants.RizaDurumu.YetkiKullanildi
        };
    }

    /// <summary>
    /// Get authorized payment consent status list 
    /// </summary>
    /// <returns>Authorized payment consent status list</returns>
    public static List<string> GetAuthorizedConsentStatusListForPayment()
    {
        return new List<string>()
        {
            OpenBankingConstants.RizaDurumu.YetkiOdemeEmrineDonustu
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

    /// <summary>
    /// Get OdemeAmaci constants values list
    /// </summary>
    /// <returns>OdemeAmaci constants values list</returns>
    public static List<string> GetOdemeAmaciList()
    {
        return new List<string>()
        {
            OpenBankingConstants.OdemeAmaci.BireyselOdeme,
            OpenBankingConstants.OdemeAmaci.TicariOdeme,
            OpenBankingConstants.OdemeAmaci.FinansalOdeme,
            OpenBankingConstants.OdemeAmaci.AidatOdemesi,
            OpenBankingConstants.OdemeAmaci.CalisanOdemesi,
            OpenBankingConstants.OdemeAmaci.EgitimOdemesi,
            OpenBankingConstants.OdemeAmaci.YatirimOdemesi,
            OpenBankingConstants.OdemeAmaci.DigerKiraOdemesi,
            OpenBankingConstants.OdemeAmaci.ETicaretOdemesi,
            OpenBankingConstants.OdemeAmaci.KonutKirasiOdemesi,
            OpenBankingConstants.OdemeAmaci.IsYeriKirasiOdemesi
        };
    }
}