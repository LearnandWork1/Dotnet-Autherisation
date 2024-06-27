using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Net;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace CoreSample
{
    [Route("api/[controller]")]
    [ApiController]
    public sealed class ExceptionHandlerMiddleware : IMiddleware
    {
        private readonly IWebHostEnvironment _env;
        private readonly ILogger<ExceptionHandlerMiddleware> _logger;

        public ExceptionHandlerMiddleware(IWebHostEnvironment env, ILogger<ExceptionHandlerMiddleware> logger)
        {
            _env = env;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }
        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            _logger.LogCritical(exception, exception.Message);
            var result = _env.IsProduction() ? "An unexpected fault happened. Try again later." : exception.Message;
            context.Response.ContentType = MediaTypeNames.Application.Json;
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            return context.Response.WriteAsync(result);
        }

            #region gets

                    // GET: api/<ValuesController>
                    [HttpGet]
                    public IEnumerable<string> Get()
                    {
                        return new string[] { "value1", "value2" };
                    }

                    // GET api/<ValuesController>/5
                    [HttpGet("{id}")]
                    public string Get(int id)
                    {
                        return "value";
                    }

                    // POST api/<ValuesController>
                    [HttpPost]
                    public void Post([FromBody] string value)
                    {
                    }

                    // PUT api/<ValuesController>/5
                    [HttpPut("{id}")]
                    public void Put(int id, [FromBody] string value)
                    {
                    }

                    // DELETE api/<ValuesController>/5
                    [HttpDelete("{id}")]
                    public void Delete(int id)
                    {
                    }
            #endregion
    }
    public static class ExceptionHandlerMiddlewareExtensions
    {
        public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }

}
