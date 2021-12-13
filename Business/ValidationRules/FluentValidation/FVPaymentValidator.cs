using Business.Constants;
using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class FVPaymentValidator:AbstractValidator<Payment>
    {
        public FVPaymentValidator()
        {
            RuleFor(p => p.CardNumber).Length(16);
            RuleFor(p => p.Year.ToString()).Length(2).WithMessage(Messages.YearMustBeTwoCharacters);
            RuleFor(p => p.Month).LessThanOrEqualTo(12);
            RuleFor(p => p.Month).GreaterThan(0);
            RuleFor(p => p.Cvv.ToString()).Length(3).WithMessage(Messages.CvvMustBeThreeCharacters); ;
        }

    }
}
