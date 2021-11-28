using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Exceptions
{
    public class ImageHelperException:Exception,ISimpleCustomException
    {
        public ImageHelperException(string message):base(message)
        {

        }
    }
}
