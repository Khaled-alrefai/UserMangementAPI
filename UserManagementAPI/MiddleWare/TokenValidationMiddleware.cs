using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

public class TokenValidationMiddleware
{
    private readonly RequestDelegate _next;

    public TokenValidationMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var token = context.Request.Headers["Authorization"].FirstOrDefault();

        if (string.IsNullOrEmpty(token) || !IsValidToken(token))
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized: Invalid or missing token.");
            return;
        }

        await _next(context); // مرر الطلب إذا كان التوكن صحيحًا
    }

    private bool IsValidToken(string token)
    {
        // 👇 للتحقق من التوكن بشكل مبسط (مثلاً توكن ثابت)
        return token == "Bearer my-secret-token";
    }
}
