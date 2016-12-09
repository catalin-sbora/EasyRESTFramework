using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace EasyRESTFramework.Client.Exceptions
{
    public class RESTResponseException: Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public RESTResponseException(HttpStatusCode statusCode, string content):base(content)
        {
            this.StatusCode = statusCode;
        } 
    }
}
