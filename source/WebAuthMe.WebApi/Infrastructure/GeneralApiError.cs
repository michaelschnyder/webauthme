namespace WebAuthMe.WebApi.Infrastructure
{
    class GeneralApiError : ApiError
    {
        public string Code { get; set; }
    }
}