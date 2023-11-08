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

    public static class GKDTur
    {
        public static readonly string Yonlendirmeli = "Y";
        public static readonly string Ayrik = "A";
    }


    public static class IzinTur
    {
        public static readonly string HesapBilgisi = "01";
        public static readonly string AyrintiliHesapBilgisi = "02";
        public static readonly string BakiyeBilgisi = "03";
        public static readonly string TemelIslem = "04";
        public static readonly string AyrintiliIslem = "05";
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

    public static class ConsentType
    {
        public static readonly string OpenBankingAccount = "OB_Account";
        public static readonly string OpenBankingPayment = "OB_Payment";
        public static readonly string OpenBankingPaymentOrder = "OB_PaymentOrder";

    }

    public static class PSUInitiated
    {
        public static readonly string OHKStarted = "E";
        public static readonly string SystemStarted = "H";
    }
    
    public static class  OdemeKaynak
    {
        public static readonly string InternetBankaciligindanGonderilenOdemelerde ="I";
        public static readonly string OtomatikParaMakineleriATMIleGonderilenOdemelerde="A";
        public static readonly string TelefonBankaciligiIleGonderilenOdemelerde="T";
        public static readonly string KiosklarAraciligiylaGonderilenOdemelerde="K";
        public static readonly string SubedenGirilenOdemelerde="S";
        public static readonly string MobilUygulamadanGonderilenOdemelerde="M";
        public static readonly string YukarıdakilerDisindakiOdemeKaynaklarindenGonderilenOdemelerde="D";
        public static readonly string AcikBankacilikAraciligiIleGonderilenOdemelerde="O";
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

}