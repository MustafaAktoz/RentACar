using Castle.DynamicProxy;
using Core.CrossCuttingConcerns.Validation;
using Core.Utilities.Interceptors;
using Core.Utilities.Messages;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Core.Aspect.Autofac.Validation
{
    public class FluentValidationAspect:MethodInterception
    {
        Type _validatorType;

        public FluentValidationAspect(Type validatorType)
        {
            if(!typeof(IValidator).IsAssignableFrom(validatorType))
            {
                throw new Exception(AspectMessages.NotValidatorType);
            }
            _validatorType = validatorType;
        }

        protected override void OnBefore(IInvocation invocation)
        {
            var validator = (IValidator)Activator.CreateInstance(_validatorType);
            var entityType = _validatorType.BaseType.GetGenericArguments()[0];
            var entities = invocation.Arguments.Where(e => e.GetType() == entityType);
            foreach (var entity in entities)
            {
                FluentValidationTool.Validate(validator, entity);
            }
        }
    }
}
