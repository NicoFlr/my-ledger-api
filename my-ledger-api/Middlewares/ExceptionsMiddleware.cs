using Data.Exceptions;
using Newtonsoft.Json;
using System.Net;
using Serilog;

namespace Presentation.Middlewares
{
    public class ExceptionsMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionsMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await ProcessError(httpContext, ex);
            }
        }

        private static int GetCode(Exception ex)
        {
            int errorCode = 500;
            switch (ex)
            {
                case UnexpectedError:
                    errorCode = (int)HttpStatusCode.InternalServerError;
                    break;
                case UserForbiddenException:
                    errorCode = (int)HttpStatusCode.Forbidden;
                    break;
                case UserAuthorizationException:
                    errorCode = (int)HttpStatusCode.Unauthorized;
                    break;
                case EntityNotFoundError:
                    errorCode = (int)HttpStatusCode.NotFound;
                    break;
                case EntityAlreadyExistsException:
                    errorCode = (int)HttpStatusCode.Conflict;
                    break;
                case NoContentException:
                    errorCode = (int)HttpStatusCode.NoContent;
                    break;
                case BadRequestException:
                    errorCode = (int)HttpStatusCode.BadRequest;
                    break;
            }

            return errorCode;
        }

        private static Task ProcessError(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";

            var errorObj = new
            {
                Code = GetCode(ex),
                ErrorMessage = ex.Message
            };

            Log.Error("Ledger-API " + ex.Message + ", stacktrace: \n" + ex.StackTrace);

            string jsonObj = JsonConvert.SerializeObject(errorObj);
            context.Response.StatusCode = GetCode(ex);
            return context.Response.WriteAsync(jsonObj);
        }
    }

    public static class ExceptionsMiddlewareExtensions
    {
        public static IApplicationBuilder UseExceptionsMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionsMiddleware>();
        }
    }
}

