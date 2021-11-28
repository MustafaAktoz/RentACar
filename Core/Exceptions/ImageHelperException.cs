using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Exceptions
{
    public class ImageHelperException:Exception,ICustomException
    {
        public ImageHelperException(string message):base(message)
        {

        }
    }
}
