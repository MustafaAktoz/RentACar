using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Payment:IEntity
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CardholderName { get; set; }
        public string CardNumber { get; set; }
        public int Year { get; set; }
        public int Month { get; set; }
        public int CVV { get; set; }
    }
}
