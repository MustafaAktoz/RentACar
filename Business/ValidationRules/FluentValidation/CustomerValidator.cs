using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class CustomerValidator:AbstractValidator<Customer>
    {
        public CustomerValidator()
        {
            RuleFor(c => c.FindeksPoint).GreaterThanOrEqualTo(0);
            RuleFor(c => c.FindeksPoint).LessThanOrEqualTo(1900);
        }
    }
}
