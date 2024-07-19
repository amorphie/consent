using System.Reflection;
using System.Text.RegularExpressions;
using amorphie.consent.core.Enum;

namespace amorphie.consent.Helper;

public static class ConstantHelper
{

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
    /// Gets consent next step status list when consent is authorized
    /// </summary>
    /// <returns>Consent next step status list</returns>
    public static List<string> GetConsentNexStepFromAuthorizedStatusList()
    {
        return new List<string>() { OpenBankingConstants.RizaDurumu.YetkiKullanildi,
            OpenBankingConstants.RizaDurumu.YetkiIptal
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
    /// Get OhkTanimTip class items as string list
    /// </summary>
    /// <returns>OhkTanimTip values list</returns>
    public static List<string> GetOhkTanimTipList()
    {
        return typeof(OpenBankingConstants.OhkTanimTip).GetAllPublicConstantValues<string>();
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
    /// Get KolasTur class items as string list
    /// </summary>
    /// <returns>KolasTur values list</returns>
    public static List<string> GetKolasTurList()
    {
        return typeof(OpenBankingConstants.KolasTur).GetAllPublicConstantValues<string>();
    }
    


    /// <summary>
    /// Get KareKodAksTur class items as string list
    /// </summary>
    /// <returns>KareKodAksTur values list</returns>
    public static List<string> GetKareKodAksTurList()
    {
        return typeof(OpenBankingConstants.KareKodAksTur).GetAllPublicConstantValues<string>();
    }


    /// <summary>
    /// Get OdemeAmaci constants values list
    /// </summary>
    /// <returns>OdemeAmaci constants values list</returns>
    public static List<string> GetOdemeAmaciList()
    {
        return typeof(OpenBankingConstants.OdemeAmaci).GetAllPublicConstantValues<string>();
    }

    /// <summary>
    /// Get OdemeKaynak constants values list
    /// </summary>
    /// <returns>OdemeKaynak constants values list</returns>
    public static List<string> GetOdemeKaynakList()
    {
        return typeof(OpenBankingConstants.OdemeKaynak).GetAllPublicConstantValues<string>();
    }
    
    /// <summary>
    /// Get OdemeDurumu constants values list
    /// </summary>
    /// <returns>OdemeDurumu constants values list</returns>
    public static List<string> GetOdemeDurumuList()
    {
        return typeof(OpenBankingConstants.OdemeDurumu).GetAllPublicConstantValues<string>();
    }

    /// <summary>
    /// Get RizaDurumu constants values list
    /// </summary>
    /// <returns>RizaDurumu constants values list</returns>
    public static List<string> GetRizaDurumuList()
    {
        return typeof(OpenBankingConstants.RizaDurumu).GetAllPublicConstantValues<string>();
    }

    /// <summary>
    /// Get KolasHspTur constants values list
    /// </summary>
    /// <returns>KolasHspTur constants values list</returns>
    public static List<string> GetKolasHspTurList()
    {
        return typeof(OpenBankingConstants.KolasHspTur).GetAllPublicConstantValues<string>();
    }

    /// <summary>
    /// Get OdemeSistemi constants values list
    /// </summary>
    /// <returns>OdemeSistemi constants values list</returns>
    public static List<string> GetOdemeSistemiList()
    {
        return typeof(OpenBankingConstants.OdemeSistemi).GetAllPublicConstantValues<string>();
    }

    /// <summary>
    /// Get OlayTip constants values list
    /// </summary>
    /// <returns>OlayTip constants values list</returns>
    public static List<string> GetOlayTipList()
    {
        return typeof(OpenBankingConstants.OlayTip).GetAllPublicConstantValues<string>();
    }

    /// <summary>
    /// Get BrcAlc constants values list
    /// </summary>
    /// <returns>BrcAlc constants values list</returns>
    public static List<string> GetBrcAlcList()
    {
        return typeof(OpenBankingConstants.BrcAlc).GetAllPublicConstantValues<string>();
    }

    /// <summary>
    /// Get KaynakTip constants values list
    /// </summary>
    /// <returns>KaynakTip constants values list</returns>
    public static List<string> GetKaynakTipList()
    {
        return typeof(OpenBankingConstants.KaynakTip).GetAllPublicConstantValues<string>();
    }

    /// <summary>
    /// Get SrlmYon constants values list
    /// </summary>
    /// <returns>SrlmYon constants values list</returns>
    public static List<string> GetSrlmYonList()
    {
        return typeof(OpenBankingConstants.SrlmYon).GetAllPublicConstantValues<string>();
    }
    
    /// <summary>
    /// Get ZmnAralik constants values list
    /// </summary>
    /// <returns>ZmnAralik constants values list</returns>
    public static List<int> GetZmnAralikList()
    {
        return typeof(OpenBankingConstants.ZmnAralik).GetAllPublicConstantValues<int>();
    }
    
    /// <summary>
    /// Get VarYok constants values
    /// </summary>
    /// <returns>VarYok constants values</returns>
    public static List<int> GetVarYok()
    {
        return typeof(OpenBankingConstants.VarYok).GetAllPublicConstantValues<int>();
    }
    
    /// <summary>
    /// Get RizaIptalDetayKodu constants values list
    /// </summary>
    /// <returns>RizaIptalDetayKodu constants values list</returns>
    public static List<string> GetRizaIptalDetayKoduList()
    {
        return typeof(OpenBankingConstants.RizaIptalDetayKodu).GetAllPublicConstantValues<string>();
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