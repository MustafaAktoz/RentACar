﻿using Business.Abstract;
using Business.BusinessAspect.Autofac;
using Business.Constants;
using Business.ValidationRules.FluentValidation;
using Core.Aspect.Autofac.Caching;
using Core.Aspect.Autofac.Performance;
using Core.Aspect.Autofac.Transaction;
using Core.Aspect.Autofac.Validation;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using Entities.DTOs;
using System;
using System.Collections.Generic;
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
            _carDal.Add(car);
            return new SuccessResult(Messages.Added);
        }

        [RemoveCacheAspect("ICarService.Get")]
        public IResult Delete(Car car)
        {
            _carDal.Delete(car);
            return new SuccessResult(Messages.Deleted);
        }

        [SecuredOperation("car.getall")]
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
            return new SuccessDataResult<List<CarDetailDto>>(Messages.GetDetails);
        }

        [CacheAspect]
        public IDataResult<List<Car>> GetCarsByBrandId(int brandId)
        {
            var result = _carDal.GetAll(c => c.Id == brandId);
            return new SuccessDataResult<List<Car>>(result,Messages.Filtered);
        }

        [CacheAspect]
        public IDataResult<List<Car>> GetCarsByColorId(int colorId)
        {
            var result = _carDal.GetAll(c => c.Id == colorId);
            return new SuccessDataResult<List<Car>>(result,Messages.Filtered);

        }

        [RemoveCacheAspect("ICarService.Get")]
        public IResult Update(Car car)
        {
            _carDal.Update(car);
            return new SuccessResult(Messages.Updated);
        }
    }
}