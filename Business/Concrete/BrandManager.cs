using Business.Abstract;
using Business.Constants;
using Core.Aspect.Autofac.Caching;
using Core.Utilities.Result;
using DataAccess.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Business.Concrete
{
    public class BrandManager : IBrandService
    {
        IBrandDal _brandDal;

        public BrandManager(IBrandDal brandDal)
        {
            _brandDal = brandDal;
        }

        [RemoveCacheAspect("IBrandService.Get")]
        public IResult Add(Brand brand)
        {
            _brandDal.Add(brand);
            return new SuccessResult(Messages.Added);
        }

        [RemoveCacheAspect("IBrandService.Get")]
        public IResult Delete(Brand brand)
        {
            _brandDal.Delete(brand);
            return new SuccessResult(Messages.Deleted);
        }

        [CacheAspect]
        public IDataResult<List<Brand>> GetAll()
        {
            var result = _brandDal.GetAll();
            return new SuccessDataResult<List<Brand>>(result, Messages.Listed);
        }

        [CacheAspect]
        public IDataResult<Brand> GetById(int id)
        {
            var result = _brandDal.Get(b => b.Id == id);
            return new SuccessDataResult<Brand>(result, Messages.Geted);
        }

        [RemoveCacheAspect("IBrandService.Get")]
        public IResult Update(Brand brand)
        {
            _brandDal.Update(brand);
            return new SuccessResult(Messages.Updated);
        }
    }
}
