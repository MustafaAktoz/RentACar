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
        ICustomerService _customerService;
        ICarService _carService;

        public RentalManager(IRentalDal rentalDal, IPaymentService paymentService, ICustomerService customerService, ICarService carService)
        {
            _rentalDal = rentalDal;
            _paymentService = paymentService;
            _customerService = customerService;
            _carService = carService;
        }

        [TransactionAspect]
        public IResult Add(Rental rental, Payment payment)
        {
            var result = RulesForAdd(rental);
            if (!result.Success) return result;

            var paymentResult = _paymentService.Pay(payment);
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
            return new SuccessDataResult<List<Rental>>(result, Messages.Listed);
        }

        public IDataResult<Rental> GetById(int id)
        {
            var result = _rentalDal.Get(r => r.Id == id);
            return new SuccessDataResult<Rental>(result, Messages.Geted);
        }

        public IDataResult<List<RentalDetailDto>> GetRentalDetails()
        {
            var result = _rentalDal.GetRentalDetails();
            return new SuccessDataResult<List<RentalDetailDto>>(result, Messages.DetailsGeted + "\n" + Messages.Listed);
        }

        public IResult Update(Rental rental)
        {
            _rentalDal.Update(rental);
            return new SuccessResult(Messages.Updated);
        }

        public IResult Deliver(Rental rental)
        {
            var result = BusinessRules.Run(CheckIfReturnDateIsReallyEmpty(rental.ReturnDate));
            if (result != null) return result;

            rental.ReturnDate = DateTime.Now;
            _rentalDal.Update(rental);
            return new SuccessResult(Messages.CarWasDelivered);
        }

        public IDataResult<List<RentalDetailDto>> GetRentalDetailsByCustomerId(int customerId)
        {
            var result = _rentalDal.GetRentalDetails(r => r.CustomerId == customerId);
            return new SuccessDataResult<List<RentalDetailDto>>(result, Messages.Listed);
        }

        private IResult CheckIfTheCarHasBeenDelivered(Rental rental)
        {
            var result = _rentalDal.Get(r => r.CarId == rental.CarId 
            && r.ReturnDate == null);

            if (result != null)
                if (rental.ReturnDate == null || rental.ReturnDate > result.RentDate)
                    return new ErrorResult(Messages.CanMustBeDelivered);

            return new SuccessResult();
        }

        private IResult CheckIfReturnDateIsReallyEmpty(DateTime? rentDate)
        {
            if (rentDate != null) return new ErrorResult(Messages.CarHasAlreadyBeenDelivered);

            return new SuccessResult();
        }

        private IResult CheckIfTheLeaseDateIsBeforeToday(DateTime rentDate)
        {
            if (rentDate < DateTime.Today)
                return new ErrorResult(Messages.RentalDateCannotBeBeforeThanToday);

            return new SuccessResult();
        }

        private IResult CheckIfTheVehicleIsAlreadyRentedBetweenTheseDates(Rental rental)
        {
            var result = _rentalDal.GetAll(r => r.CarId == rental.CarId
            && r.RentDate.Date >= rental.RentDate
            && (rental.ReturnDate == null ? true : r.RentDate <= rental.ReturnDate));
            if (result.Any())
            {
                return new ErrorResult(Messages.DateRangeError);
            }

            return new SuccessResult();
        }

        private IResult CheckIfTheReturnDateIsBeforeTheRentalDate(Rental rental)
        {
            if (rental.RentDate > rental.ReturnDate)
                return new ErrorResult(Messages.ReturnDateCannotBeBeforeThanRentalDate);

            return new SuccessResult();
        }

        private IResult CheckIfFindeksPointNotEnough(int customerId, int carId)
        {
            var car = _carService.GetById(carId);
            if (!car.Success) return new ErrorResult(car.Message);

            var customer = _customerService.GetById(customerId);
            if (!customer.Success) return new ErrorResult(customer.Message);

            if (customer.Data.FindeksPoint < car.Data.FindeksPoint) return new ErrorResult(Messages.NotEnoughFindeksPoints);

            return new SuccessResult();
        }

        public IResult RulesForAdd(Rental rental)
        {
            var result = BusinessRules.Run(
               CheckIfFindeksPointNotEnough(rental.CustomerId, rental.CarId),
               CheckIfTheLeaseDateIsBeforeToday(rental.RentDate),
               CheckIfTheVehicleIsAlreadyRentedBetweenTheseDates(rental),
               CheckIfTheCarHasBeenDelivered(rental),
               CheckIfTheReturnDateIsBeforeTheRentalDate(rental)
               );

            if (result != null) return result;

            return new SuccessResult();
        }


    }
}
