using CoreSample.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Globalization;
using System.Threading.Tasks;

namespace CoreSample
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class Custom1Middleware
    {
        private readonly RequestDelegate _next;

        public Custom1Middleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var cultureQuery = httpContext.Request.Query["culture"];
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {
                var culture = new CultureInfo(cultureQuery);

                CultureInfo.CurrentCulture = culture;
                CultureInfo.CurrentUICulture = culture;

              // Location = culture.ToString();
            }

            // Call the next delegate/middleware in the pipeline.


            await _next(httpContext);
            //return _next(httpContext);
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class Custom1MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustom1Middleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<Custom1Middleware>();
        }
    }
}
