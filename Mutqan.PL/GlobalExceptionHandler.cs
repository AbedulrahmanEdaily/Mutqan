using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Mutqan.DAL.DTO.Response;

namespace Mutqan.PL
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            var exceptionMessage = new ExceptionResponse
            {
                StatusCode = StatusCodes.Status500InternalServerError,
                Message = exception.Message
            };
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(exceptionMessage);
            return true;
        }
    }
}
