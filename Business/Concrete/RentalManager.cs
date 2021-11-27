using Business.Abstract;
using Business.Constants;
using Core.Aspect.Autofac.Transaction;
using Core.Utilities.Business;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Business.Concrete
{
    public class RentalManager : IRentalService
    {
        IRentalDal _rentalDal;
        IPaymentService _paymentService;

        public RentalManager(IRentalDal rentalDal, IPaymentService paymentService)
        {
            _rentalDal = rentalDal;
            _paymentService = paymentService;
        }

        [TransactionAspect]
        public IResult Add(Rental rental,Payment payment)
        {
            var result=RulesForAdd(rental);
            if (!result.Success) return result;

            var paymentResult = _paymentService.Add(payment);
            if (!paymentResult.Success) return new SuccessResult(paymentResult.Message);

            _rentalDal.Add(rental);

            return new SuccessResult(Messages.RentalSuccessful);
        }

        public IResult Delete(Rental rental)
        {

            _rentalDal.Delete(rental);
            return new SuccessResult(Messages.Deleted);
        }

        public IDataResult<List<Rental>> GetAll()
        {
            var result = _rentalDal.GetAll();
            return new SuccessDataResult<List<Rental>>(result,Messages.Listed);
        }

        public IDataResult<Rental> GetById(int id)
        {
            var result = _rentalDal.Get(r => r.Id == id);
            return new SuccessDataResult<Rental>(result,Messages.Geted);
        }

        public IDataResult<List<RentalDetailDto>> GetRentalDetails()
        {
            var result = _rentalDal.GetRentalDetails();
            return new SuccessDataResult<List<RentalDetailDto>>(result,Messages.DetailsGeted+"\n"+Messages.Listed);
        }

        public IResult Update(Rental rental)
        {
            _rentalDal.Update(rental);
            return new SuccessResult(Messages.Updated);
        }

        private IResult CarMustBeDelivered(Rental rental)
        {
            var result = _rentalDal.Get(r => r.CarId == rental.CarId
            &&r.ReturnDate==null
            &&r.RentDate<=rental.ReturnDate);
            if(result!=null)
            {
                return new ErrorResult(Messages.CanMustBeDelivered);
            }

            return new SuccessResult();
        }

        private IResult RentDateControl(Rental rental)
        {
            var result = _rentalDal.GetAll(r => r.CarId == rental.CarId
            && r.RentDate.Date >= rental.RentDate
            &&(rental.ReturnDate==null?true:r.RentDate<=rental.ReturnDate));
            if (result.Any())
            {
                return new ErrorResult(Messages.DateRangeError);
            }

            return new SuccessResult();
        }

        private IResult NotBefore(DateTime rentDate)
        {
            if (rentDate < DateTime.Now)
                return new ErrorResult(Messages.NotBefore);

            return new SuccessResult();
        }

        private IResult NotBeforeRentDate(Rental rental)
        {
            if (rental.RentDate > rental.ReturnDate)
                return new ErrorResult(Messages.NotBeforeRentDate);

            return new SuccessResult();
        }

        public IResult RulesForAdd(Rental rental)
        {
            var result = BusinessRules.Run(CarMustBeDelivered(rental),
               RentDateControl(rental),
               NotBefore(rental.RentDate),
               NotBeforeRentDate(rental));
            if (result != null) return result;

            return new SuccessResult();
        }
    }
}
