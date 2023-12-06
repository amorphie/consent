using System.Diagnostics;

namespace amorphie.consent.core.Enum;
public static class OpenBankingConstants
{
    static OpenBankingConstants()
    {
    }

    public static class KimlikTur
    {
        public static readonly string TCKN = "K";
        public static readonly string MNO = "M";
        public static readonly string YKN = "Y";
        public static readonly string PNO = "P";
    }

    public static class KurumKimlikTur
    {
        public static readonly string TCKN = "K";
        public static readonly string MNO = "M";
        public static readonly string VKN = "V";
    }


    public static class OHKTur
    {
        public static readonly string Bireysel = "B";
        public static readonly string Kurumsal = "K";
    }

    public static class KolasTur
    {
        public static readonly string TelefonNumarasi = "T";
        public static readonly string EPosta = "E";
        public static readonly string TCKN = "K";
        public static readonly string VKN = "V";
        public static readonly string YKN = "Y";
        public static readonly string PasaportNumarasi = "P";
    }
    
    public static class KareKodAksTur
    {
        public static readonly string FastDinamik = "01";
        public static readonly string FastStatik = "02";
        public static readonly string KisidenKisiye = "03";
    }
    
    public static class OdemeKaynak
    {
        public static readonly string InternetBankaciligindanGonderilenOdemelerde = "I";
        public static readonly string OtomatikParaMakineleriATMIleGonderilenOdemelerde = "A";
        public static readonly string TelefonBankaciligiIleGonderilenOdemelerde = "T";
        public static readonly string KiosklarAraciligiylaGonderilenOdemelerde = "K";
        public static readonly string SubedenGirilenOdemelerde = "S";
        public static readonly string MobilUygulamadanGonderilenOdemelerde = "M";
        public static readonly string YukarıdakilerDisindakiOdemeKaynaklarindenGonderilenOdemelerde = "D";
        public static readonly string AcikBankacilikAraciligiIleGonderilenOdemelerde = "O";
    }
    
    public static class OdemeAmaci
    {
        public static readonly string KonutKirasiOdemesi = "01";
        public static readonly string IsYeriKirasiOdemesi = "02";
        public static readonly string DigerKiraOdemesi = "03";
        public static readonly string ETicaretOdemesi = "04";
        public static readonly string CalisanOdemesi = "05";
        public static readonly string TicariOdeme = "06";
        public static readonly string BireyselOdeme = "07";
        public static readonly string YatirimOdemesi = "08";
        public static readonly string FinansalOdeme = "09";
        public static readonly string EgitimOdemesi = "10";
        public static readonly string AidatOdemesi = "11";
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
        public static readonly string Gerceklesti ="01";
        /// <summary>
        /// Ödeme ilgili ödeme sistemine iletildi ancak alıcı hesabına ulaştığına dair teyit alınmadı.
        /// İşlemin son durumunun teyidi için YÖS sorgulama yapmalıdır.
        /// </summary>
        public static readonly string Gonderildi="02";
        /// <summary>
        /// Ödemenin ilgili ödeme sistemine işletiminde veya işlenmesi sırasında hata, zaman aşımı, sistemin gün sonu işlemlerine başlaması vb. durum oluştu
        /// ve ödeme gereçeklemedi veya iade edildi.
        /// </summary>
        public static readonly string Gerceklesmedi="03";
        /// <summary>
        /// Ödeme emri alındı. Çoklu onay akışının tamamlanması bekleniyor.
        /// </summary>
        public static readonly string OnaydaBekliyor="04";
        /// <summary>
        /// ( Ödeme emri alındı. ) YÖS tarafından müşteriye gösterilmeyen bir koddur.
        /// Bu kod dönüldüğünde YÖS uygulamasında ÖHK’ya ödeme durumu
        /// </summary>
        public static readonly string IslemeAlindi="05";
    }
    
    public static class IslemAmaci
    {
        public static readonly string KonutKirasiOdemesi = "01";
        public static readonly string IsYeriKirasiOdemesi = "02";
        public static readonly string DigerKiraOdemesi = "03";
        public static readonly string ETicaretOdemesi = "04";
        public static readonly string CalisanOdemesi = "05";
        public static readonly string TicariOdeme = "06";
        public static readonly string BireyselOdeme = "07";
        public static readonly string YatirimOdemesi = "08";
        public static readonly string FinansalOdeme = "09";
        public static readonly string EgitimOdemesi = "10";
        public static readonly string AidatOdemesi = "11";
        public static readonly string Diger = "12";
    }
    
    public static class RizaDurumu
    {
        public static readonly string YetkiBekleniyor = "B";
        public static readonly string Yetkilendirildi = "Y";
        public static readonly string YetkiKullanildi = "K";
        public static readonly string YetkiOdemeEmrineDonustu = "E";
        public static readonly string YetkiSonlandirildi = "S";
        public static readonly string YetkiIptal = "I";
    }
    
    /// <summary>
    /// GKD yapılmasını gerekli bulduğu durumda HHS, YÖS’ün belirlediği yöntemi dikkate alarak kendi belirlediği GKD yöntemini bildirir:
    /// Comment from document
    /// </summary>
    public static class GKDTur
    {
        public static readonly string Yonlendirmeli = "Y";
        public static readonly string Ayrik = "A";
    }

    public static class RizaTip
    {
        public static readonly string HesapBilgisiRizasi = "H";
        public static readonly string OdemeEmrizRizasi = "O";

    }

    public static class IzinTur
    {
        public static readonly string HesapBilgisi = "01";
        public static readonly string AyrintiliHesapBilgisi = "02";
        public static readonly string BakiyeBilgisi = "03";
        public static readonly string TemelIslem = "04";
        public static readonly string AyrintiliIslem = "05";
    }

    public static class OdemeSistemi
    {
        public static readonly string Havale ="H";
        public static readonly string Fast ="F";
        public static readonly string EFT_POS="E";
    }

    public static class IslemTuru
    {
        public  static readonly string HAVALE="HAVALE";
        public  static readonly string EFT="EFT";
        public  static readonly string FAST="FAST";
        public  static readonly string PARA_YATIRMA="PARA_YATIRMA";
        public  static readonly string PARA_CEKME="PARA_CEKME";
        public  static readonly string YABANCI_PARA_HAVALE="YABANCI_PARA_HAVALE";
        public  static readonly string YATIRIM_HESABINA_AKTARIM="YATIRIM_HESABINA_AKTARIM";
        public  static readonly string YATIRIM_HESABINDAN_AKTARIM="YATIRIM_HESABINDAN_AKTARIM";
        public  static readonly string KURUM_FATURA_ODEMESI="KURUM_FATURA_ODEMESI";
        public  static readonly string CEK="CEK";
        public  static readonly string SENET="SENET";
        public  static readonly string SIGORTA_ODEMESI="SIGORTA_ODEMESI";
        public  static readonly string UCRET_KOMISYON_FAIZ="UCRET_KOMISYON_FAIZ";
        public  static readonly string SGK_ODEMESI="SGK_ODEMESI";
        public  static readonly string VERGI_ODEMESI="VERGI_ODEMESI";
        public  static readonly string DOVIZ_ALIM="DOVIZ_ALIM";
        public  static readonly string DOVIZ_SATIM="DOVIZ_SATIM";
        public  static readonly string KREDI_ODEMESI="KREDI_ODEMESI";
        public  static readonly string KREDI_KULLANIM="KREDI_KULLANIM";
        public  static readonly string KK_ODEMESI="KK_ODEMESI";
        public  static readonly string KK_NAKIT_AVANS="KK_NAKIT_AVANS";
        public  static readonly string SANS_OYUNU="SANS_OYUNU";
        public  static readonly string UYE_ISYERI_ISLEMLERI="UYE_ISYERI_ISLEMLERI";
        public  static readonly string HGS_OGS_ISLEMLERI="HGS_OGS_ISLEMLERI";
        public  static readonly string DOGRUDAN_BORCLANDIRMA_SISTEMI="DOGRUDAN_BORCLANDIRMA_SISTEMI";
        public  static readonly string DIGER="DIGER";
    }

    public static class HspTur
    {
        public static readonly string Bireysel="B";
        public static readonly string Ticari="T";
    }
    
    public static class KolasHspTur
    {
        public static readonly string Bireysel="B";
        public static readonly string Ticari="T";
    }

    /// <summary>
    /// Hesap çeşidini belirtir
    /// Comment from document
    /// </summary>
    public static class HspTip
    {
        public static readonly string VADESIZ="VADESIZ";
        public static readonly string VADELI="VADELI";
        public static readonly string KREDILI_MEVDUAT_HESABI="KREDILI MEVDUAT HESABI";
        public static readonly string POS="POS";
        public static readonly string CEK="CEK";
        public static readonly string YATIRIM="YATIRIM";
    }
    
    /// <summary>
    /// Hesabın durumunu belirtir
    /// Comment from document
    /// </summary>
    public static class HspDrm
    {
        public static readonly string AKTIF="AKTIF";
        public static readonly string PASIF="PASIF";
        public static readonly string KAPALI="KAPALI";
    }
    
    /// <summary>
    /// Alabileceği değerlere göre hangi belirtecin döneceğine karar verilir
    /// Comment from document
    /// </summary>
    public static class YetTip
    {
        public static readonly string yet_kod="yet_kod";
        public static readonly string yenileme_belirteci="yenileme_belirteci";
    }
    
    /// <summary>
    /// İşlemin hesabı borçlandırdığı ya da alacaklandırdığı bilgisidir.
    /// Comment from document
    /// </summary>
    public static class BrcAlc
    {
       public static readonly string Borc ="B";
       public static readonly string Alacak="A";
    }

    public static class RizaIptalDetayKodu
    {
        public static readonly string YeniRizaTalebiIleIptal = "01";
        public static readonly string KullaniciIstegiIleHHSUzerindenIptal = "02";
        public static readonly string KullaniciIstegiIleYOSUzerindenIptal = "03";
        public static readonly string SureAsimi_YetkiBekleniyor = "04";
        public static readonly string SureAsimi_Yetkilendirildi = "05";
        public static readonly string SureAsimi_YetkiOdemeyeDonusmedi = "06";
        public static readonly string GKDIptali_AyniRizaNoIleMukerrerCagri = "07";
        public static readonly string GKDIptali_RizaNoIleTCKNUyusmamasi = "08";
        public static readonly string GKDIptali_UygunUrunBulunmuyor = "09";
        public static readonly string GKDIptali_HHSAcikBankacilikKanaliIslemeKapali = "10";
        public static readonly string GKDIptali_HesapYetkiSorunu = "11";
        public static readonly string GKDIptali_OHKHHSKontrolleriniAsamadi = "12";
        public static readonly string GKDIptali_OHKIstegiIleGKDdenVazgecildi = "13";
        public static readonly string GKDIptali_FraudSuphesi = "14";
        public static readonly string GKDIptali_Diger = "99";
    }

    public static class PSUInitiated
    {
        public static readonly string OHKStarted = "E";
        public static readonly string SystemStarted = "H";
    }
    
    public static class ConsentType
    {
        public static readonly string OpenBankingAccount = "OB_Account";
        public static readonly string OpenBankingPayment = "OB_Payment";
        public static readonly string OpenBankingYOSAccount = "OB_YOSAccount";
        public static readonly string OpenBankingYOSPayment = "OB_YOSPayment";

    }
    public static class ConsentDetailType
    {
        public static readonly string OpenBankingPaymentOrder = "OB_PaymentOrder";
    }

    

   

    

}