using amorphie.consent.core.Model;
using FluentValidation;

namespace amorphie.consent.Validator;
public sealed class ConsentDefinitionValidator : AbstractValidator<ConsentDefinition>
{
    public ConsentDefinitionValidator()
    {
        RuleFor(x => x.Scope).NotNull();
        RuleFor(x => x.RoleAssignment).MinimumLength(3);
    }
}