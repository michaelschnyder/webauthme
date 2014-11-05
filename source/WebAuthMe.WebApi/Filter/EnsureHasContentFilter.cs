using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebAuthMe.WebApi.Infrastructure;

namespace WebAuthMe.WebApi.Filter
{
    public class EnsureHasContentFilter : ActionFilterAttribute, IOrderedFilter
    {
        private const string ContentMissingCode = "ContentMissing";
        private const string ContentMissingMessage = "Missing content";

        public EnsureHasContentFilter()
        {
            this.Order = 20;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (actionContext.Request.Content == null)
            {
                throw new HttpResponseException(new ApiErrorHttpResponseMessage(new GeneralApiError { Code = ContentMissingCode, Message = ContentMissingMessage }));
            }

            base.OnActionExecuting(actionContext);
        }

        public int Order { get; set; }
    }
}