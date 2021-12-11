using Business.Constants;
using Entities.Concrete;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.ValidationRules.FluentValidation
{
    public class CarValidator:AbstractValidator<Car>
    {
        public CarValidator()
        {
            RuleFor(c => c.Name).MinimumLength(2);
            RuleFor(c => c.DailyPrice).GreaterThan(0);
            RuleFor(c => c.DailyPrice).GreaterThanOrEqualTo(400).When(c => c.BrandId == 1).WithMessage(Messages.LowPriceForThisBrand);
            RuleFor(c => c.FindeksPoint).GreaterThanOrEqualTo(0);
            RuleFor(c => c.FindeksPoint).LessThanOrEqualTo(1900);
        }
    }
}
