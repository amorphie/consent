using amorphie.consent.core.DTO.OpenBanking.Event;
using amorphie.consent.core.DTO.OpenBanking.HHS;
using amorphie.consent.core.Model;

namespace amorphie.consent.Mapper;

using AutoMapper;
using System.Linq;

public class CustomerNumberResolver : IValueResolver<Consent, HHSAccountConsentDto, string?>
{
    public string Resolve(Consent source, HHSAccountConsentDto destination, string? destMember, ResolutionContext context)
    {
        return source.OBAccountConsentDetails?.FirstOrDefault()?.CustomerNumber ?? string.Empty;
    }
}

public class InstitutionCustomerNumberResolver : IValueResolver<Consent, HHSAccountConsentDto, string?>
{
    public string Resolve(Consent source, HHSAccountConsentDto destination, string? destMember, ResolutionContext context)
    {
        return source.OBAccountConsentDetails?.FirstOrDefault()?.InstitutionCustomerNumber ?? string.Empty;
    }
}

public class CustomerNumberResolverPayment : IValueResolver<Consent, HHSPaymentConsentDto, string?>
{
    public string Resolve(Consent source, HHSPaymentConsentDto destination, string? destMember, ResolutionContext context)
    {
        return source.OBPaymentConsentDetails?.FirstOrDefault()?.CustomerNumber ?? string.Empty;
    }
}

public class InstitutionCustomerNumberResolverPayment : IValueResolver<Consent, HHSPaymentConsentDto, string?>
{
    public string Resolve(Consent source, HHSPaymentConsentDto destination, string? destMember, ResolutionContext context)
    {
        return source.OBPaymentConsentDetails?.FirstOrDefault()?.InstitutionCustomerNumber ?? string.Empty;
    }
}

public class OlaylarResolver : IValueResolver<OBEvent, OlayIstegiDto, List<OlaylarDto>>
{
    public List<OlaylarDto> Resolve(OBEvent source, OlayIstegiDto destination, List<OlaylarDto> destMember, ResolutionContext context)
    {
        return new List<OlaylarDto> { context.Mapper.Map<OlaylarDto>(source) };
    }
}

