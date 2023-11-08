using amorphie.consent.core.Model;
using FluentValidation;

namespace amorphie.consent.Validator;
public sealed class ConsentValidator : AbstractValidator<Consent>
{
    public ConsentValidator()
    {
        RuleFor(x => x.ConsentType).NotNull();
        RuleFor(x => x.AdditionalData).MinimumLength(5);
    }
}
