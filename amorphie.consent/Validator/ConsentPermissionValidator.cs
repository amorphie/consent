using amorphie.consent.core.Model;
using FluentValidation;

namespace amorphie.consent.Validator;
public sealed class ConsentPermissionValidator : AbstractValidator<ConsentPermission>
{
    public ConsentPermissionValidator()
    {
        RuleFor(x => x.Permission).NotNull();
    }
}