using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TweyesBackend.Core.Extension
{
    public class ApiException : Exception
    {
        public ApiException() : base() { }

        public ApiException(string message, int errorCode = 99, HttpStatusCode httpStatusCode = HttpStatusCode.InternalServerError, Exception innerException = null) : base(message, innerException)
        {
            ErrorCode = errorCode;
            HttpStatusCode = httpStatusCode;
        }

        public int ErrorCode { get; protected set; }
        public HttpStatusCode HttpStatusCode { get; protected set; }
    }
}
