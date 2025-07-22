// this class is a middleware to log all incoming request and outgoing response

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        // سجل بيانات الطلب
        var request = context.Request;
        _logger.LogInformation($"Request: {request.Method} {request.Path}");

        // نسخ الاستجابة داخل Stream مؤقت
        var originalBodyStream = context.Response.Body;
        using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        await _next(context);  // تابع إلى باقي الـ Pipeline

        // سجل بيانات الاستجابة
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseText = await new StreamReader(context.Response.Body).ReadToEndAsync();
        context.Response.Body.Seek(0, SeekOrigin.Begin);

        _logger.LogInformation($"Response: {context.Response.StatusCode}, Body: {responseText}");

        // إعادة النسخة الأصلية للـ Stream
        await responseBody.CopyToAsync(originalBodyStream);
    }
}
