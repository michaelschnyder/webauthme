using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebAuthMe.WebApi.Infrastructure;

namespace WebAuthMe.WebApi.Filter
{
    public class ValidateHeaderFilter : ActionFilterAttribute, IOrderedFilter
    {
        private readonly string headerName;

        private static string HeaderMissingCode = "HeaderMissing";
        private static string HeaderMissingMessage = "Missing header '{0}'";

        public ValidateHeaderFilter(string headerName)
        {
            this.headerName = headerName;
            this.Order = 10;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.Request.Headers.Select(h => h.Key.ToLowerInvariant()).Contains(this.headerName.ToLowerInvariant()))
            {
                throw new HttpResponseException(new ApiErrorHttpResponseMessage(new GeneralApiError { Code = HeaderMissingCode, Message = string.Format(HeaderMissingMessage, this.headerName)}));
            }

            base.OnActionExecuting(actionContext);
        }

        public async Task<HttpResponseMessage> ExecuteAuthorizationFilterAsync(HttpActionContext actionContext, CancellationToken cancellationToken,
            Func<Task<HttpResponseMessage>> continuation)
        {
            return await continuation();
        }

        public int Order { get; set; }
    }
}