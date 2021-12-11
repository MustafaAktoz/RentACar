using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspect.Autofac.Caching;
using Core.Aspect.Autofac.Performance;
using Core.Aspect.Autofac.Transaction;
using Core.Aspect.Autofac.Validation;
using Core.Utilities.Business;
using Core.Utilities.Helpers;
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
    public class CarManager:ICarService
    {
        ICarDal _carDal;

        public CarManager(ICarDal carDal)
        {
            _carDal = carDal;
        }

        [ValidationAspect(typeof(CarValidator))]
        [SecuredOperation("car.add,admin")]
        [RemoveCacheAspect("ICarService.Get")]
        [TransactionAspect]
        [PerformanceAspect(2)]
        public IResult Add(Car car)
        {
            var result = BusinessRules.Run(CarNameAlreadyExist(car.Name));
            if (result!=null) return result;

            _carDal.Add(car);
            return new SuccessResult(Messages.Added);
        }

        [RemoveCacheAspect("ICarService.Get")]
        public IResult Delete(Car car)
        {
            _carDal.Delete(car);
            return new SuccessResult(Messages.Deleted);
        }

        //[SecuredOperation("car.getall")]
        [CacheAspect]
        [PerformanceAspect(1)]
        public IDataResult<List<Car>> GetAll()
        {
            var result = _carDal.GetAll();
            return new SuccessDataResult<List<Car>>(result,Messages.Listed);
        }

        [CacheAspect]
        public IDataResult<Car> GetById(int id)
        {
            var result = _carDal.Get(c => c.Id == id);
            return new SuccessDataResult<Car>(result,Messages.Geted);
        }

        [CacheAspect]
        public IDataResult<List<CarDetailDto>> GetCarDetails()
        {
            var result = _carDal.GetCarDetails();
            CarImageControl(result);
            return new SuccessDataResult<List<CarDetailDto>>(result,Messages.DetailsGeted+"\n"+Messages.Listed);
        }

        public IDataResult<List<CarDetailDto>> GetCarDetailsByBrandId(int brandId)
        {
            var result = _carDal.GetCarDetails(c=>c.BrandId==brandId);
            CarImageControl(result);
            return new SuccessDataResult<List<CarDetailDto>>(result,Messages.DetailsGeted+"\n"+Messages.Filtered);
        }

        public IDataResult<List<CarDetailDto>> GetCarDetailsByColorId(int colorId)
        {
            var result = _carDal.GetCarDetails(c => c.ColorId == colorId);
            CarImageControl(result);
            return new SuccessDataResult<List<CarDetailDto>>(result,Messages.DetailsGeted+"\n"+Messages.Filtered);
        }

        public IDataResult<CarDetailDto> GetCarDetailById(int id)
        {
            var result = _carDal.GetCarDetail(c=>c.Id==id);
            CarImageControlForSingleData(result);
            return new SuccessDataResult<CarDetailDto>(result,Messages.DetailsGeted);
        }

        [CacheAspect]
        public IDataResult<List<Car>> GetByBrandId(int brandId)
        {
            var result = _carDal.GetAll(c => c.BrandId == brandId);
            return new SuccessDataResult<List<Car>>(result,Messages.Filtered);
        }

        [CacheAspect]
        public IDataResult<List<Car>> GetByColorId(int colorId)
        {
            var result = _carDal.GetAll(c => c.ColorId == colorId);
            return new SuccessDataResult<List<Car>>(result,Messages.Filtered);

        }

        [ValidationAspect(typeof(CarValidator))]
        [RemoveCacheAspect("ICarService.Get")]
        public IResult Update(Car car)
        {
            _carDal.Update(car);
            return new SuccessResult(Messages.Updated);
        }


        private IResult CarNameAlreadyExist(string name)
        {
            var result = _carDal.Get(c => c.Name == name);
            if (result != null) return new ErrorResult(Messages.CarNameAlreadyExist);

            return new SuccessResult();
        }

        private IResult CarImageControl(List<CarDetailDto> carDetailsDto)
        {
            foreach (var carDetailDto in carDetailsDto)
            {
                if (!carDetailDto.CarImages.Any())
                {
                    carDetailDto.CarImages.Add(new CarImage { ImagePath = ImageHelper.DefaultImagePath });
                }
            }

            return new SuccessResult();
        }

        private IResult CarImageControlForSingleData(CarDetailDto carDetailDto)
        {
            if (!carDetailDto.CarImages.Any())
                carDetailDto.CarImages.Add(new CarImage { ImagePath = ImageHelper.DefaultImagePath });

            return new SuccessResult();
        }
    }
}
