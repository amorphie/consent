using amorphie.consent.core.Model;
using FluentValidation;

namespace amorphie.consent.Validator;
public sealed class TokenValidator : AbstractValidator<Token>
    {
        public TokenValidator()
        {
            RuleFor(x => x.TokenValue).NotNull();
            RuleFor(x => x.TokenType).NotNull();        }
    }