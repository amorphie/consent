using System.Diagnostics;

namespace amorphie.consent.core.Enum;
public static class OpenBankingConstants
{
    static OpenBankingConstants()
    {
    }

    public static class KimlikTur
    {
        public const string TCKN = "K";
        public const string MNO = "M";
        public const string YKN = "Y";
        public const string PNO = "P";
    }

    public static class KurumKimlikTur
    {
        public const string TCKN = "K";
        public const string MNO = "M";
        public const string VKN = "V";
    }


    public static class OHKTur
    {
        public const string Bireysel = "B";
        public const string Kurumsal = "K";
    }
    
    public static class OhkTanimTip
    {
        public const string TCKN = "TCKN";
        public const string MNO = "MNO";
        public const string YKN = "YKN";
        public const string PNO = "PNO";
        public const string GSM = "GSM";
        public const string IBAN = "IBAN";
    }

    public static class KolasTur
    {
        public const string TelefonNumarasi = "T";
        public const string EPosta = "E";
        public const string TCKN = "K";
        public const string VKN = "V";
        public const string YKN = "Y";
        public const string PasaportNumarasi = "P";
    }

    public static class KareKodAksTur
    {
        public const string FastDinamik = "01";
        public const string FastStatik = "02";
        public const string KisidenKisiye = "03";
    }

    public static class OdemeKaynak
    {
        public const string InternetBankaciligindanGonderilenOdemelerde = "I";
        public const string OtomatikParaMakineleriATMIleGonderilenOdemelerde = "A";
        public const string TelefonBankaciligiIleGonderilenOdemelerde = "T";
        public const string KiosklarAraciligiylaGonderilenOdemelerde = "K";
        public const string SubedenGirilenOdemelerde = "S";
        public const string MobilUygulamadanGonderilenOdemelerde = "M";
        public const string YukarıdakilerDisindakiOdemeKaynaklarindenGonderilenOdemelerde = "D";
        public const string AcikBankacilikAraciligiIleGonderilenOdemelerde = "O";
    }

    public static class OdemeAmaci
    {
        public const string KonutKirasiOdemesi = "01";
        public const string IsYeriKirasiOdemesi = "02";
        public const string DigerKiraOdemesi = "03";
        public const string ETicaretOdemesi = "04";
        public const string CalisanOdemesi = "05";
        public const string TicariOdeme = "06";
        public const string BireyselOdeme = "07";
        public const string YatirimOdemesi = "08";
        public const string FinansalOdeme = "09";
        public const string EgitimOdemesi = "10";
        public const string AidatOdemesi = "11";
    }

    /// <summary>
    /// Ödemenin durumunu gösterir. 
    /// Comment from document
    /// </summary>
    public static class OdemeDurumu
    {
        /// <summary>
        /// Ödeme ilgili ödeme sistemine iletildi ve alıcı hesabına ulaştı.
        /// </summary>
        public const string Gerceklesti = "01";
        /// <summary>
        /// Ödeme ilgili ödeme sistemine iletildi ancak alıcı hesabına ulaştığına dair teyit alınmadı.
        /// İşlemin son durumunun teyidi için YÖS sorgulama yapmalıdır.
        /// </summary>
        public const string Gonderildi = "02";
        /// <summary>
        /// Ödemenin ilgili ödeme sistemine işletiminde veya işlenmesi sırasında hata, zaman aşımı, sistemin gün sonu işlemlerine başlaması vb. durum oluştu
        /// ve ödeme gereçeklemedi veya iade edildi.
        /// </summary>
        public const string Gerceklesmedi = "03";
        /// <summary>
        /// Ödeme emri alındı. Çoklu onay akışının tamamlanması bekleniyor.
        /// </summary>
        public const string OnaydaBekliyor = "04";
        /// <summary>
        /// ( Ödeme emri alındı. ) YÖS tarafından müşteriye gösterilmeyen bir koddur.
        /// Bu kod dönüldüğünde YÖS uygulamasında ÖHK’ya ödeme durumu
        /// </summary>
        public const string IslemeAlindi = "05";
    }

    public static class IslemAmaci
    {
        public const string KonutKirasiOdemesi = "01";
        public const string IsYeriKirasiOdemesi = "02";
        public const string DigerKiraOdemesi = "03";
        public const string ETicaretOdemesi = "04";
        public const string CalisanOdemesi = "05";
        public const string TicariOdeme = "06";
        public const string BireyselOdeme = "07";
        public const string YatirimOdemesi = "08";
        public const string FinansalOdeme = "09";
        public const string EgitimOdemesi = "10";
        public const string AidatOdemesi = "11";
        public const string Diger = "12";
    }

    public static class RizaDurumu
    {
        public const string YetkiBekleniyor = "B";
        public const string Yetkilendirildi = "Y";
        public const string YetkiKullanildi = "K";
        public const string YetkiOdemeEmrineDonustu = "E";
        public const string YetkiSonlandirildi = "S";
        public const string YetkiIptal = "I";
    }

    /// <summary>
    /// GKD yapılmasını gerekli bulduğu durumda HHS, YÖS’ün belirlediği yöntemi dikkate alarak kendi belirlediği GKD yöntemini bildirir:
    /// Comment from document
    /// </summary>
    public static class GKDTur
    {
        public const string Yonlendirmeli = "Y";
        public const string Ayrik = "A";
    }

    public static class RizaTip
    {
        public const string HesapBilgisiRizasi = "H";
        public const string OdemeEmrizRizasi = "O";

    }

    public static class IzinTur
    {
        public const string TemelHesapBilgisi = "01";
        public const string AyrintiliHesapBilgisi = "02";
        public const string BakiyeBilgisi = "03";
        public const string TemelIslemBilgisi = "04";
        public const string AyrintiliIslemBilgisi = "05";
        public const string AnlikBakiyeBildirimi = "06";
    }

    public static class OdemeSistemi
    {
        public const string Havale = "H";
        public const string Fast = "F";
        public const string EFT_POS = "E";
    }

    public static class IslemTuru
    {
        public const string HAVALE = "HAVALE";
        public const string EFT = "EFT";
        public const string FAST = "FAST";
        public const string PARA_YATIRMA = "PARA_YATIRMA";
        public const string PARA_CEKME = "PARA_CEKME";
        public const string YABANCI_PARA_HAVALE = "YABANCI_PARA_HAVALE";
        public const string YATIRIM_HESABINA_AKTARIM = "YATIRIM_HESABINA_AKTARIM";
        public const string YATIRIM_HESABINDAN_AKTARIM = "YATIRIM_HESABINDAN_AKTARIM";
        public const string KURUM_FATURA_ODEMESI = "KURUM_FATURA_ODEMESI";
        public const string CEK = "CEK";
        public const string SENET = "SENET";
        public const string SIGORTA_ODEMESI = "SIGORTA_ODEMESI";
        public const string UCRET_KOMISYON_FAIZ = "UCRET_KOMISYON_FAIZ";
        public const string SGK_ODEMESI = "SGK_ODEMESI";
        public const string VERGI_ODEMESI = "VERGI_ODEMESI";
        public const string DOVIZ_ALIM = "DOVIZ_ALIM";
        public const string DOVIZ_SATIM = "DOVIZ_SATIM";
        public const string KREDI_ODEMESI = "KREDI_ODEMESI";
        public const string KREDI_KULLANIM = "KREDI_KULLANIM";
        public const string KK_ODEMESI = "KK_ODEMESI";
        public const string KK_NAKIT_AVANS = "KK_NAKIT_AVANS";
        public const string SANS_OYUNU = "SANS_OYUNU";
        public const string UYE_ISYERI_ISLEMLERI = "UYE_ISYERI_ISLEMLERI";
        public const string HGS_OGS_ISLEMLERI = "HGS_OGS_ISLEMLERI";
        public const string DOGRUDAN_BORCLANDIRMA_SISTEMI = "DOGRUDAN_BORCLANDIRMA_SISTEMI";
        public const string DIGER = "DIGER";
    }

    public static class HspTur
    {
        public const string Bireysel = "B";
        public const string Ticari = "T";
    }

    public static class KolasHspTur
    {
        public const string Bireysel = "B";
        public const string Ticari = "T";
    }

    /// <summary>
    /// Hesap çeşidini belirtir
    /// Comment from document
    /// </summary>
    public static class HspTip
    {
        public const string VADESIZ = "VADESIZ";
        public const string VADELI = "VADELI";
        public const string KREDILI_MEVDUAT_HESABI = "KREDILI MEVDUAT HESABI";
        public const string POS = "POS";
        public const string CEK = "CEK";
        public const string YATIRIM = "YATIRIM";
    }

    /// <summary>
    /// Hesabın durumunu belirtir
    /// Comment from document
    /// </summary>
    public static class HspDrm
    {
        public const string AKTIF = "AKTIF";
        public const string PASIF = "PASIF";
        public const string KAPALI = "KAPALI";
    }

    /// <summary>
    /// Alabileceği değerlere göre hangi belirtecin döneceğine karar verilir
    /// Comment from document
    /// </summary>
    public static class YetTip
    {
        public const string yet_kod = "yet_kod";
        public const string yenileme_belirteci = "yenileme_belirteci";
    }

    /// <summary>
    /// İşlemin hesabı borçlandırdığı ya da alacaklandırdığı bilgisidir.
    /// Comment from document
    /// </summary>
    public static class BrcAlc
    {
        public const string Borc = "B";
        public const string Alacak = "A";
    }

    public static class RizaIptalDetayKodu
    {
        public const string YeniRizaTalebiIleIptal = "01";
        public const string KullaniciIstegiIleHHSUzerindenIptal = "02";
        public const string KullaniciIstegiIleYOSUzerindenIptal = "03";
        public const string SureAsimi_YetkiBekleniyor = "04";
        public const string SureAsimi_Yetkilendirildi = "05";
        public const string SureAsimi_YetkiOdemeyeDonusmedi = "06";
        public const string GKDIptali_AyniRizaNoIleMukerrerCagri = "07";
        public const string GKDIptali_RizaNoIleTCKNUyusmamasi = "08";
        public const string GKDIptali_UygunUrunBulunmuyor = "09";
        public const string GKDIptali_HHSAcikBankacilikKanaliIslemeKapali = "10";
        public const string GKDIptali_HesapYetkiSorunu = "11";
        public const string GKDIptali_OHKHHSKontrolleriniAsamadi = "12";
        public const string GKDIptali_OHKIstegiIleGKDdenVazgecildi = "13";
        public const string GKDIptali_FraudSuphesi = "14";
        public const string GKDIptali_Diger = "99";
    }

    public static class SrlmYon
    {
        public const string Azalan = "A";
        public const string Artan = "Y";
    }

    public static class ZmnAralik
    {
        public const int KayitYok = 0;
        public const int Sifir2Saat = 1;
        public const int Iki24Saat = 2;
        public const int Bir3Gun = 3;
        public const int Dort15Gun = 4;
        public const int OnAltiGunVeUzeri = 5;
    }

    public static class PSUInitiated
    {
        public const string OHKStarted = "E";
        public const string SystemStarted = "H";
    }

    public static class ConsentType
    {
        public const string OpenBankingAccount = "OB_Account";
        public const string OpenBankingPayment = "OB_Payment";
        public const string OpenBankingYOSAccount = "OB_YOSAccount";
        public const string OpenBankingYOSPayment = "OB_YOSPayment";

    }
    public static class ConsentDetailType
    {
        public const string OpenBankingPaymentOrder = "OB_PaymentOrder";
    }

}