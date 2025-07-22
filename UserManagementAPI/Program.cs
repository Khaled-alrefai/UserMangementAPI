var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddControllers();

var app = builder.Build();


app.UseHttpsRedirection();

// MiddleWare

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<TokenValidationMiddleware>();
app.UseMiddleware<LoggingMiddleware>();

app.MapControllers();



app.Run();
