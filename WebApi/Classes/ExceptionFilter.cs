using DomainServices.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;

namespace WebApi.Classes
{
    public class ExceptionFilter : IExceptionFilter
    {
        private readonly ILogger _logger;

        public ExceptionFilter(ILoggerFactory loggerFactory) 
        { 
            _logger = loggerFactory.CreateLogger("Exception Filter");
        }

        public void OnException(ExceptionContext context)
        {
            int responseCode = StatusCodes.Status500InternalServerError;

            if (context.Exception is NotFoundException)
            {
                responseCode = StatusCodes.Status404NotFound;
            }
            else if (context.Exception is ArgumentException)
            {
                responseCode = StatusCodes.Status400BadRequest;
            }

            DefaultResponse response = new()
            {
                ResponseMessage = context.Exception.Message,
            };

            context.Result = new ObjectResult(response)
            {
                StatusCode = responseCode,
                DeclaredType = typeof(DefaultResponse)
            };
        }
    }
}
