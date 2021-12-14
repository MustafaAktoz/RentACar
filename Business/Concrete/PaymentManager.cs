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

        [FluentValidationAspect(typeof(FVPaymentValidator))]
        public IResult Add(Payment payment)
        {
            var result = BusinessRules.Run(IsThisCardSavedForThisUser(payment));
            if (result != null) return result;

            _paymentDal.Add(payment);
            return new SuccessResult(Messages.CardSaved);
        }

        public IResult Delete(Payment payment)
        {
            _paymentDal.Delete(payment);
            return new SuccessResult(Messages.Deleted);
        }

        public IDataResult<List<Payment>> GetByUserId(int userId)
        {
            var result= _paymentDal.GetAll(p=>p.UserId== userId);
            return new SuccessDataResult<List<Payment>>(result,Messages.Listed);
        }

        [FluentValidationAspect(typeof(FVPaymentValidator))]
        public IResult Pay(Payment payment)
        {
            return new SuccessResult(Messages.PaymentSuccessful);
        }

        private IResult IsThisCardSavedForThisUser(Payment payment)
        {
            var result = _paymentDal.Get(p => p.UserId == payment.UserId&&p.CardNumber==payment.CardNumber);
            if (result != null) return new ErrorResult(Messages.YouAlreadyExistASavedCardWithThisCardNumber);

            return new SuccessResult();
        }
    }
}
