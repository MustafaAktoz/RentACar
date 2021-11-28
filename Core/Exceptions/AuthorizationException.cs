using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Exceptions
{
    public class AuthorizationException:Exception,ICustomException
    {
        public AuthorizationException(string message):base(message)
        {

        }
    }
}
