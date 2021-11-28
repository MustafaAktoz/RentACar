using Core.Exceptions;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Extensions
{
    public class ExceptionMiddleware
    {
        RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception e)
            {
                await HandleExceptionAsync(httpContext,e);
            }
        }

        private Task HandleExceptionAsync(HttpContext httpContext,Exception e)
        {
            httpContext.Response.ContentType="application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            string message = "Internal Server Error";

            if (e.GetType() == typeof(ValidationException)) return ConfigureByValidationExceptionAsync(httpContext, e);
          
            if (typeof(ICustomException).IsAssignableFrom(e.GetType())) ConfigureByCustomException(out message, httpContext, e);

            return httpContext.Response.WriteAsync(new ErrorDetails
            {
                Message = message,
                StatusCode=httpContext.Response.StatusCode
            }.ToString());
        }

        private Task ConfigureByValidationExceptionAsync(HttpContext httpContext,Exception e)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            return httpContext.Response.WriteAsync(new ValidationErrorDetails
            {
                Message = "Validation Exception",
                StatusCode = StatusCodes.Status400BadRequest,
                ValidationErrors = ((ValidationException)e).Errors
            }.ToString());
        }

        private void ConfigureByCustomException(out string message, HttpContext httpContext, Exception e)
        {
            httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            message = e.Message;
        }
    }
}