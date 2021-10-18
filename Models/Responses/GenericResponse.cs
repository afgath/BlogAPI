using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace zmgTestBack.Models.Responses
{
    public class GenericResponse
    {
        public GenericResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }
        public int StatusCode { get; set; }
        public string Message { get; set; }

    }
}
