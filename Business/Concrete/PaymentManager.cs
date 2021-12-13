using Business.Abstract;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspect.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class PaymentManager : IPaymentService
    {
        IPaymentDal _paymentDal;

        public PaymentManager(IPaymentDal paymentDal)
        {
            _paymentDal = paymentDal;
        }

        [ValidationAspect(typeof(PaymentValidator))]
        public IResult Add(Payment payment)
        {
            var result = BusinessRules.Run(IsThisCardRegisteredForThisUser(payment));
            if (result != null) return result;

            _paymentDal.Add(payment);
            return new SuccessResult(Messages.CardSaved);
        }

        public IDataResult<List<Payment>> GetByUserId(int userId)
        {
            var result= _paymentDal.GetAll(p=>p.UserId== userId);
            return new SuccessDataResult<List<Payment>>(result,Messages.Listed);
        }

        [ValidationAspect(typeof(PaymentValidator))]
        public IResult Pay(Payment payment)
        {
            return new SuccessResult(Messages.PaymentSuccessful);
        }

        private IResult IsThisCardRegisteredForThisUser(Payment payment)
        {
            var payments = _paymentDal.GetAll(p => p.UserId == payment.UserId);
            if (!payments.Any()) return new SuccessResult();

            var result = payments.SingleOrDefault(p => p.CardNumber == payment.CardNumber);
            if (result != null) return new ErrorResult(Messages.YouAlreadyExistARegisteredCardWithThisCardNumber);

            return new SuccessResult();
        }
    }
}
