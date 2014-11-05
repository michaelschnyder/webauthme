namespace WebAuthMe.WebApi.Infrastructure
{
    class FieldApiError : ApiError
    {
        public string FieldName { get; set; }
    }
}