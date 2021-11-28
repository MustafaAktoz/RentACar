using Core.Utilities.Interceptors;
using Core.Utilities.IoC;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using Castle.DynamicProxy;
using Core.Extensions;
using Business.Constants;
using Core.Utilities.Messages;
using Core.Exceptions;

namespace Business.BusinessAspect.Autofac
{
    public class SecuredOperation:MethodInterception
    {
        string[] _roles;
        IHttpContextAccessor _httpContextAccessor;
        public SecuredOperation(string roles)
        {
            _roles = roles.Split(',');
            _httpContextAccessor = ServiceTool.ServiceProvider.GetService<IHttpContextAccessor>();
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var roles = _httpContextAccessor.HttpContext.User.ClaimRoles();
            foreach (var role in _roles)
            {
                if (roles.Contains(role))
                {
                    return;
                }
            }

            throw new AuthorizationException(AspectMessages.AuthorizationDenied);
        }
    }
}
