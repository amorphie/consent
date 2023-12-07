using System.Reflection;
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
    /// Get GKDTur class items as string list
    /// </summary>
    /// <returns>GKDTur values list</returns>
    public static List<string> GetGKDTurList()
    {
        return typeof(OpenBankingConstants.GKDTur).GetAllPublicConstantValues<string>();
    }
    
    /// <summary>
    /// Get KimlikTur class items as string list
    /// </summary>
    /// <returns>KimlikTur values list</returns>
    public static List<string> GetKimlikTurList()
    {
        return typeof(OpenBankingConstants.KimlikTur).GetAllPublicConstantValues<string>();
    }
    
    /// <summary>
    /// Get KurumKimlikTur class items as string list
    /// </summary>
    /// <returns>KurumKimlikTur values list</returns>
    public static List<string> GetKurumKimlikTurList()
    {
        return typeof(OpenBankingConstants.KurumKimlikTur).GetAllPublicConstantValues<string>();
    }
    
    /// <summary>
    /// Get OHKTur class items as string list
    /// </summary>
    /// <returns>OHKTur values list</returns>
    public static List<string> GetOHKTurList()
    {
        return typeof(OpenBankingConstants.OHKTur).GetAllPublicConstantValues<string>();
    }
    
    /// <summary>
    /// Get IzinTur class items as string list
    /// </summary>
    /// <returns>IzinTur values list</returns>
    public static List<string> GetIzinTurList()
    {
        return typeof(OpenBankingConstants.IzinTur).GetAllPublicConstantValues<string>();
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
    
    public static List<T> GetAllPublicConstantValues<T>(this Type type)
    {
        return type
            .GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy)
            .Where(fi => fi.IsLiteral && !fi.IsInitOnly && fi.FieldType == typeof(T))
            .Select(x => (T)x.GetRawConstantValue())
            .ToList();
    }
}