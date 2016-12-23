using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using EasyRESTFramework.Client.Exceptions;

namespace EasyRESTFramework.Client.Extensions
{
    public static class HttpResponseMessageExtensions
    {
        public static void EnsureIsSuccessStatusCode(this HttpResponseMessage response)
        {
            if (response.IsSuccessStatusCode)
                return;

            var content = response.Content.ReadAsStringAsync().Result;
            if (response.Content != null)
            {
                response.Content.Dispose();
            }
            if (content.Length == 0)
            {
                content = response.ReasonPhrase;
            }
            throw new RESTResponseException(response.StatusCode, content);
        }

    }
}
