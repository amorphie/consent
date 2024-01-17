using amorphie.consent.core.Model;
using FluentValidation;

namespace amorphie.consent.Validator;
public sealed class YosInfoalidator : AbstractValidator<OBYosInfo>
{
    public YosInfoalidator()
    {
        RuleFor(x => x.Marka).NotNull();
        RuleFor(x => x.Unv).NotNull();
    }
}