using Core.Entities.Abstract;
using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Concrete
{
    public class Customer:IEntity
    {
        public int Id { get; set; }//Id kaldırılıp, user tablosu ile bire bir ilişki yapılacak
        public int UserId { get; set; }
        public string CompanyName { get; set; }
        public int FindeksPoint { get; set; }
    }
}
