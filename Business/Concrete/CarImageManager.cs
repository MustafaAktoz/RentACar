using Business.Abstract;
using Business.Constants;
using Core.Aspect.Autofac.Caching;
using Core.Aspect.Autofac.Transaction;
using Core.Utilities.Business;
using Core.Utilities.Helpers;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Business.Concrete
{
    public class CarImageManager : ICarImageService
    {
        ICarImageDal _carImageDal;
        ICarService _carService;

        public CarImageManager(ICarImageDal carImageDal, ICarService carService)
        {
            _carImageDal = carImageDal;
            _carService = carService;
        }

        [TransactionAspect]
        [RemoveCacheAspect("ICarService.Get")]
        public IResult Add(int carId, IFormFile file)
        {
            var result = BusinessRules.Run(CheckIfCarExist(carId), CheckIfImageLimitIsExceededForThisCar(carId));
            if (result != null) return result;

            CarImage carImage = new CarImage
            {
                CarId = carId,
                ImagePath = ImageHelper.Upload(file),
                Date = DateTime.Now
            };

            _carImageDal.Add(carImage);
            return new SuccessResult(Messages.Added);
        }

        [TransactionAspect]
        [RemoveCacheAspect("ICarService.Get")]
        public IResult Delete(int id)
        {
            var carImage = _carImageDal.Get(ci => ci.Id == id);
            if (carImage == null) return new ErrorResult(Messages.ImageNotFound);

            ImageHelper.Delete(carImage.ImagePath);

            _carImageDal.Delete(carImage);
            return new SuccessResult(Messages.Deleted);
        }

        public IDataResult<List<CarImage>> GetByCarId(int carId)
        {
            var result = _carImageDal.GetAll(ci => ci.CarId == carId);
            if (result.Count == 0) result.Add(new CarImage { ImagePath = ImageHelper.DefaultImagePath });

            return new SuccessDataResult<List<CarImage>>(result, Messages.Listed);
        }

        [TransactionAspect]
        [RemoveCacheAspect("ICarService.Get")]
        public IResult Update(int id, IFormFile file)
        {
            var carImage = _carImageDal.Get(ci => ci.Id == id);
            if (carImage == null) return new ErrorResult(Messages.ImageNotFound);

            carImage.ImagePath = ImageHelper.Update(carImage.ImagePath, file);

            _carImageDal.Update(carImage);
            return new SuccessResult(Messages.Updated);
        }

        private IResult CheckIfImageLimitIsExceededForThisCar(int carId)
        {
            var result = _carImageDal.GetAll(ci => ci.CarId == carId);
            if (result.Count >= 5) return new ErrorResult(Messages.MaximumImageLimitExceeded);

            return new SuccessResult();
        }

        private IResult CheckIfCarExist(int carId)
        {
            var result = _carService.GetById(carId);
            if (result.Data == null) return new ErrorResult(Messages.CarNotFound);

            return new SuccessResult();
        }

    }
}
