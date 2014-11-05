using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using WebAuthMe.WebApi.Infrastructure;

namespace WebAuthMe.WebApi.Filter
{
    public class ValidateModelFilter : ActionFilterAttribute, IOrderedFilter
    {
        private readonly bool removeModelName;

        public ValidateModelFilter(bool removeModelName = true)
        {
            this.removeModelName = removeModelName;
            this.Order = 30;
        }

        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var apiErrors = new List<ApiError>();

                foreach (var var in actionContext.ModelState)
                {
                    var field = var.Key;

                    if (this.removeModelName && field.Contains("."))
                    {
                        field = field.Substring(field.IndexOf('.') + 1);
                    }

                    var errorsForField = var.Value.Errors.Where(e => !string.IsNullOrEmpty(e.ErrorMessage)).ToList();

                    if (errorsForField.Any())
                    {
                        apiErrors.AddRange(errorsForField.Select(modelError => new FieldApiError {FieldName = field, Message = modelError.ErrorMessage}));
                    }
                }
                
                if (apiErrors.Any()) {
                    throw new HttpResponseException(new ApiErrorHttpResponseMessage(apiErrors));
                }
            }

            base.OnActionExecuting(actionContext);
        }

        public int Order { get; set; }
    }
}