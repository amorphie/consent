using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using amorphie.template.core.Model;

namespace amorphie.template.Validator;
    public sealed class StudentValidator : AbstractValidator<Student>
    {
        public StudentValidator()
        {
            RuleFor(x => x.FirstMidName).NotNull();
            RuleFor(x => x.LastName).MinimumLength(10);
        }
    }
