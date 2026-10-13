namespace ClinicManagement.WebApi.Infrastructure
{
    public class CorsMiddleware(RequestDelegate next)
    {
        private readonly RequestDelegate _next = next;

        public Task Invoke(HttpContext httpContext)
        {
            httpContext.Response.Headers.Append("Access-Control-Allow-Origin", "*");
            httpContext.Response.Headers.Append("Access-Control-Allow-Credentials", "true");
            httpContext.Response.Headers.Append("Access-Control-Allow-Headers", "Content-Type, Accept");
            httpContext.Response.Headers.Append("Access-Control-Allow-Methods", "POST,GET,PUT,PATCH,DELETE,OPTIONS");
            return _next(httpContext);
        }
    }
}
