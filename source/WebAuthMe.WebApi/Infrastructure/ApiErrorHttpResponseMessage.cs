using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace WebAuthMe.WebApi.Infrastructure
{
    public class ApiErrorHttpResponseMessage : HttpResponseMessage
    {
        public ApiErrorHttpResponseMessage(ApiError error) : this(new List<ApiError>(new[] {error}))
        {
        }

        public ApiErrorHttpResponseMessage(List<ApiError> errors)
        {
            this.StatusCode = HttpStatusCode.BadRequest;

            if (errors.Count == 1)
            {
                this.Content = new StringContent(JsonConvert.SerializeObject(errors[0]), Encoding.UTF8, "application/json");
            }
            else
            {
                this.Content = new StringContent(JsonConvert.SerializeObject(errors), Encoding.UTF8, "application/json");
            }
                        
        }
    }
}