using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Net;
using System.Net.Http;
using zmgTestBack.Helpers;

namespace zmgTestBack.Filters
{
    public class BlogExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            ExceptionHelper response = new ExceptionHelper()
            {
                ErrorMessage = context.Exception.Message
            };
            switch (context.Exception.GetType().Name.ToString())
            {
                case nameof(ArgumentNullException):
                    response.ErrorCode = (int)HttpStatusCode.NotFound;
                    break;
                default:
                    response.ErrorCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            context.HttpContext.Response.StatusCode = response.ErrorCode;
            context.Result = new JsonResult(response);
        }
    }
}
