using amorphie.consent.core.Model;
using FluentValidation;

namespace amorphie.consent.Validator;
public sealed class OpenBankingConsentValidator : AbstractValidator<Consent>
    {
        public OpenBankingConsentValidator()
        {
            RuleFor(x => x.ConsentType).NotNull();
            RuleFor(x => x.AdditionalData).MinimumLength(5);
        }
    }
