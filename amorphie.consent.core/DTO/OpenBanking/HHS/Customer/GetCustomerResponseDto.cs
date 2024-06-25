public class GetCustomerResponseDto
{
    public bool isCustomer { get; set; }
    public string customerNumber { get; set; }
    public string citizenshipNumber { get; set; }
    public string passportNo { get; set; }
    public bool krmIsCustomer { get; set; }
    public string krmCustomerNumber { get; set; }
    public string krmCitizenshipNumber { get; set; }
    public string krmTaxNo { get; set; }
}

public class VerificationUserJsonData
{
    public string[] account { get; set; }
}