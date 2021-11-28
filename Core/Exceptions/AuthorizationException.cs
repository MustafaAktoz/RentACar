using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Exceptions
{
    public class AuthorizationException:Exception,ISimpleCustomException
    {
        public AuthorizationException(string message):base(message)
        {

        }
    }
}
