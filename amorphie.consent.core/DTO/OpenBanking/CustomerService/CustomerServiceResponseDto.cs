namespace amorphie.consent.core.DTO.OpenBanking;

public class CustomerServiceResponseDto
{
    public DataDto data { get; set; }
    public MetaDto meta { get; set; }
}

// Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
public class DataDto
{
    public ProfileDto profile { get; set; }
    public List<PhoneDto> phones { get; set; }
    public List<RelationDto> relations { get; set; }
    public List<EmailDto> emails { get; set; }
}

public class EmailDto
{
    public string type { get; set; }
    public string address { get; set; }
    public bool isVerified { get; set; }
}

public class MetaDto
{
    public string transactionId { get; set; }
}

public class PhoneDto
{
    public string countryCode { get; set; }
    public string number { get; set; }
    public string prefix { get; set; }
    public string type { get; set; }
}

public class ProfileDto
{
    public string type { get; set; }
    public string status { get; set; }
    public bool isBankPersonel { get; set; }
    public string identificationNumber { get; set; }
    public string citizenshipNumber { get; set; }
    public string taxNo { get; set; }
    public int externalClientNo { get; set; }
    public string businessLine { get; set; }
    public string name { get; set; }
    public string middleName { get; set; }
    public string surname { get; set; }
    public bool hasMobApproval { get; set; }
    public bool hasCollectionRestriction { get; set; }
    public bool isPrivateBanking { get; set; }
    public DateTime birthDate { get; set; }
    public string gender { get; set; }
    public string coreBankingServiceAgreement { get; set; }
    public DateTime retiredSalaryAccCheckDate { get; set; }
    public bool hasAmlApproval { get; set; }
    public bool hasForeignIdentity { get; set; }
    public object userName { get; set; }
    public object companyId { get; set; }
    public object applicationBranchCode { get; set; }
}

public class RelationDto
{
    public int relationTypeCode { get; set; }
    public string relationTypeName { get; set; }
    public string relatedCustomerShortName { get; set; }
    public long relatedCustomerId { get; set; }
    public long relatedCustomerNo { get; set; }
    public double share { get; set; }
}