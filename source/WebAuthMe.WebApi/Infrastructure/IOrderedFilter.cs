using System.Web.Http.Filters;

namespace WebAuthMe.WebApi.Infrastructure
{
    public interface IOrderedFilter : IFilter
    {
        int Order { get; set; }
    }
}