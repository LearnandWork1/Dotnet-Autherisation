using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CoreSample
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(
            RequestDelegate next,
            ILogger<ExceptionHandlingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception, "Exception occurred: {Message}", exception.Message);

                var problemDetails = new ProblemDetails
                {
                    Status = StatusCodes.Status500InternalServerError,
                    Title = "Server Error",
                    Detail = exception.Message
                };

                context.Response.StatusCode =
                    StatusCodes.Status500InternalServerError;

                await context.Response.WriteAsJsonAsync(problemDetails);
            }
        }

       // private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        //{
        //    context.Response.ContentType = "application/json";
        //    var response = context.Response;

        //    var errorResponse = new ErrorResponse
        //    {
        //        Success = false
        //    };
        //    switch (exception)
        //    {
        //        case ApplicationException ex:
        //            if (ex.Message.Contains("Invalid Token"))
        //            {
        //                response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;
        //                errorResponse.Message = ex.Message;
        //                break;
        //            }
        //            response.StatusCode = (int)HttpStatusCode.BadRequest;
        //            errorResponse.Message = ex.Message;
        //            break;
        //        default:
        //            response.StatusCode = (int)HttpStatusCode.InternalServerError;
        //            errorResponse.Message = "Internal server error!";
        //            break;
        //    }
        //    _logger.LogError(exception.Message);
        //    var result = JsonSerializer.Serialize(errorResponse);
        //    await context.Response.WriteAsync(result);
        //}
    }



    internal sealed class GlobalExceptionHandlerGlobal : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandlerGlobal> _logger;

        public GlobalExceptionHandlerGlobal(ILogger<GlobalExceptionHandlerGlobal> logger)
        {
            _logger = logger;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            _logger.LogError(
                exception, "Exception occurred: {Message}", exception.Message);

            var ExecProblemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Server error"
            };

            httpContext.Response.StatusCode = ExecProblemDetails.Status.Value;

            await httpContext.Response
                .WriteAsJsonAsync(ExecProblemDetails, cancellationToken);

            return true;
        }
    }

    //internal sealed class BadRequestExceptionHandler : IExceptionHandler
    //{
    //    private readonly ILogger<BadRequestExceptionHandler> _logger;

    //    public BadRequestExceptionHandler(ILogger<BadRequestExceptionHandler> logger)
    //    {
    //        _logger = logger;
    //    }

    //    public async ValueTask<bool> TryHandleAsync(
    //        HttpContext httpContext,
    //        Exception exception,
    //        CancellationToken cancellationToken)
    //    {
    //        if (exception is not BadRequestException badRequestException)
    //        {
    //            return false;
    //        }

    //        _logger.LogError(
    //            badRequestException,
    //            "Exception occurred: {Message}",
    //            badRequestException.Message);

    //        var problemDetails = new ProblemDetails
    //        {
    //            Status = StatusCodes.Status400BadRequest,
    //            Title = "Bad Request",
    //            Detail = badRequestException.Message
    //        };

    //        httpContext.Response.StatusCode = problemDetails.Status.Value;

    //        await httpContext.Response
    //            .WriteAsJsonAsync(problemDetails, cancellationToken);

    //        return true;
    //    }


    //    internal sealed class NotFoundExceptionHandler : IExceptionHandler
    //    {
    //        private readonly ILogger<NotFoundExceptionHandler> _logger;

    //        public GlobalExceptionHandler(ILogger<NotFoundExceptionHandler> logger)
    //        {
    //            _logger = logger;
    //        }

    //        public async ValueTask<bool> TryHandleAsync(
    //            HttpContext httpContext,
    //            Exception exception,
    //            CancellationToken cancellationToken)
    //        {
    //            if (exception is not NotFoundException notFoundException)
    //            {
    //                return false;
    //            }

    //            _logger.LogError(
    //                notFoundException,
    //                "Exception occurred: {Message}",
    //                notFoundException.Message);

    //            var problemDetails = new ProblemDetails
    //            {
    //                Status = StatusCodes.Status404NotFound,
    //                Title = "Not Found",
    //                Detail = notFoundException.Message
    //            };

    //            httpContext.Response.StatusCode = problemDetails.Status.Value;

    //            await httpContext.Response
    //                .WriteAsJsonAsync(problemDetails, cancellationToken);

    //            return true;
    //        }
    //    }
    //}
}
