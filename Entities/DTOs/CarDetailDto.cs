using Core.Entities.Abstract;
using Entities.Concrete;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.DTOs
{
    public class CarDetailDto:IDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string BrandName { get; set; }
        public string ColorName { get; set; }
        public decimal DailyPrice { get; set; }
        public string ModelYear { get; set; }
        public string Description { get; set; }
        public List<CarImage> CarImages { get; set; }
    }
}
