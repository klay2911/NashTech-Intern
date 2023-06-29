
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseHttpsRedirection();

//app.Run(MyRunMiddleware);
app.UseMiddleware<LoggingMiddleware>();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;

    public LoggingMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var request = context.Request;
        var requestBodyStream = new MemoryStream();
        var originalRequestBody = request.Body;

        await request.Body.CopyToAsync(requestBodyStream);
        requestBodyStream.Seek(0, SeekOrigin.Begin);

        var requestBodyText = new StreamReader(requestBodyStream).ReadToEnd();
        var logMessage = $"Request: {request.Scheme} {request.Host} {request.Path} {request.QueryString} {requestBodyText}";

        // Write logMessage to file here
        using (var writer = new StreamWriter("log.txt", true))
        {
            writer.WriteLine(logMessage);
        }

        requestBodyStream.Seek(0, SeekOrigin.Begin);
        request.Body = requestBodyStream;

        await _next(context);

        request.Body = originalRequestBody;
    }
}