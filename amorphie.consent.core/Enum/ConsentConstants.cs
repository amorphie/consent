namespace amorphie.consent.core.Enum;

public static class ConsentConstants
{
    static ConsentConstants()
    {
    }

    public static class ConsentType
    {
        public const string OpenBankingAccount = "OB_Account";
        public const string OpenBankingPayment = "OB_Payment";
        public const string OpenBankingYOSAccount = "OB_YOSAccount";
        public const string OpenBankingYOSPayment = "OB_YOSPayment";
        public const string IBLogin = "IB_Login";
        public const string IBIVR = "IB_IVR";

    }
}