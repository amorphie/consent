using amorphie.consent.core.Model;
using FluentValidation;

namespace amorphie.consent.Validator;
public sealed class YosInfoalidator : AbstractValidator<OBYosInfo>
{
    public YosInfoalidator()
    {
        RuleFor(x => x.marka).NotNull();
        RuleFor(x => x.unv).NotNull();
    }
}